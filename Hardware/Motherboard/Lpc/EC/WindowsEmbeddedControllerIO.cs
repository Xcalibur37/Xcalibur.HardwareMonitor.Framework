using System.Diagnostics;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC.Exceptions;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc.EC;

/// <summary>
/// An unsafe but universal implementation for the ACPI Embedded Controller IO interface for Windows
/// </summary>
/// <seealso cref="IEmbeddedControllerIo" />
/// <remarks>
/// It is unsafe because of possible race condition between this application and the PC firmware when
/// writing to the EC registers. For a safe approach ACPI/WMI methods have to be used, but those are
/// different for each motherboard model.
/// </remarks>
public class WindowsEmbeddedControllerIo : IEmbeddedControllerIo
{
    #region Fields

    private const int FailuresBeforeSkip = 20;
    private const int MaxRetries = 5;
    private const int WaitSpins = 50;
    private bool _disposed;
    private static int _waitReadFailures;

    /// <summary>
    /// Read operation
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <param name="register">The register.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    private delegate bool ReadOp<TParam>(byte register, out TParam p);

    /// <summary>
    /// Write operation
    /// </summary>
    /// <typeparam name="TParam">The type of the parameter.</typeparam>
    /// <param name="register">The register.</param>
    /// <param name="p">The p.</param>
    /// <returns></returns>
    private delegate bool WriteOp<in TParam>(byte register, TParam p);

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsEmbeddedControllerIo"/> class.
    /// </summary>
    /// <exception cref="BusMutexLockingFailedException"></exception>
    public WindowsEmbeddedControllerIo()
    {
        if (Mutexes.WaitEc(10)) return;
        throw new BusMutexLockingFailedException();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Mutexes.ReleaseEc();
    }

    /// <summary>
    /// Reads the specified registers.
    /// </summary>
    /// <param name="registers">The registers.</param>
    /// <param name="data">The data.</param>
    public void Read(ushort[] registers, byte[] data)
    {
        Trace.Assert(registers.Length <= data.Length, 
                     "data buffer length has to be greater or equal to the registers array length");

        byte bank = 0;
        byte prevBank = SwitchBank(bank);

        // oops... somebody else is working with the EC too
        Trace.WriteLineIf(prevBank != 0, "Concurrent access to the ACPI EC detected.\nRace condition possible.");

        // read registers minimizing bank switches.
        for (int i = 0; i < registers.Length; i++)
        {
            byte regBank = (byte)(registers[i] >> 8);
            byte regIndex = (byte)(registers[i] & 0xFF);
            // registers are sorted by bank
            if (regBank > bank)
            {
                bank = SwitchBank(regBank);
            }
            data[i] = ReadByte(regIndex);
        }

        SwitchBank(prevBank);
    }

    /// <summary>
    /// Reads a byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <returns></returns>
    private static byte ReadByte(byte register) => ReadLoop<byte>(register, ReadByteOp);

    /// <summary>
    /// Writes a byte.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    private static void WriteByte(byte register, byte value) => WriteLoop(register, value, WriteByteOp);

    /// <summary>
    /// Switches the bank.
    /// </summary>
    /// <param name="bank">The bank.</param>
    /// <returns></returns>
    private static byte SwitchBank(byte bank)
    {
        byte previous = ReadByte(0xFF);
        WriteByte(0xFF, bank);
        return previous;
    }

    /// <summary>
    /// Reads the loop.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="register">The register.</param>
    /// <param name="op">The op.</param>
    /// <returns></returns>
    private static TResult ReadLoop<TResult>(byte register, ReadOp<TResult> op) where TResult : new()
    {
        TResult result = new();
        for (int i = 0; i < MaxRetries; i++)
        {
            if (op(register, out result))
            {
                return result;
            }
        }
        return result;
    }

    /// <summary>
    /// Writes the loop.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    /// <param name="op">The op.</param>
    private static void WriteLoop<TValue>(byte register, TValue value, WriteOp<TValue> op)
    {
        for (int i = 0; i < MaxRetries; i++)
        {
            if (op(register, value)) return;
        }
    }

    /// <summary>
    /// Waits for status.
    /// </summary>
    /// <param name="status">The status.</param>
    /// <param name="isSet">if set to <c>true</c> [is set].</param>
    /// <returns></returns>
    private static bool WaitForStatus(Status status, bool isSet)
    {
        for (int i = 0; i < WaitSpins; i++)
        {
            byte value = ReadIoPort(Port.Command);

            if (((byte)status & (!isSet ? value : (byte)~value)) == 0)
            {
                return true;
            }

            Thread.Sleep(1);
        }

        return false;
    }

    /// <summary>
    /// Waits the read.
    /// </summary>
    /// <returns></returns>
    private static bool WaitRead()
    {
        if (_waitReadFailures > FailuresBeforeSkip)
        {
            return true;
        }

        if (WaitForStatus(Status.OutputBufferFull, true))
        {
            _waitReadFailures = 0;
            return true;
        }

        _waitReadFailures++;
        return false;
    }

    /// <summary>
    /// Waits for write status
    /// </summary>
    /// <returns></returns>
    private static bool WaitWrite() => WaitForStatus(Status.InputBufferFull, false);

    /// <summary>
    /// Reads the io port.
    /// </summary>
    /// <param name="port">The port.</param>
    /// <returns></returns>
    private static byte ReadIoPort(Port port) => Ring0.ReadIoPort((uint)port);

    /// <summary>
    /// Writes the io port.
    /// </summary>
    /// <param name="port">The port.</param>
    /// <param name="datum">The datum.</param>
    private static void WriteIoPort(Port port, byte datum) => Ring0.WriteIoPort((uint)port, datum);
    
    #region Read/Write ops

    /// <summary>
    /// Reads the byte op.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    protected static bool ReadByteOp(byte register, out byte value)
    {
        if (WaitWrite())
        {
            WriteIoPort(Port.Command, (byte)Command.Read);

            if (WaitWrite())
            {
                WriteIoPort(Port.Data, register);

                if (WaitWrite() && WaitRead())
                {
                    value = ReadIoPort(Port.Data);
                    return true;
                }
            }
        }

        value = 0;
        return false;
    }

    /// <summary>
    /// Writes the byte op.
    /// </summary>
    /// <param name="register">The register.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    protected static bool WriteByteOp(byte register, byte value)
    {
        if (!WaitWrite()) return false;

        WriteIoPort(Port.Command, (byte)Command.Write);
        if (!WaitWrite()) return false;
        
        WriteIoPort(Port.Data, register);
        if (!WaitWrite()) return false;
        
        WriteIoPort(Port.Data, value);
        return true;
    }

    #endregion

    #endregion
}
