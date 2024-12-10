using System.Globalization;
using System.Text;
using System.Threading;
using Xcalibur.HardwareMonitor.Framework.Hardware.Kernel;
using Xcalibur.HardwareMonitor.Framework.Hardware.Sensors;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Cpu.AMD;

/// <summary>
/// AMD 0F-series CPU
/// </summary>
/// <seealso cref="AmdCpuBase" />
internal sealed class Amd0FCpu : AmdCpuBase
{
    #region Fields

    private const uint FidVidStatus = 0xC0010042;
    private const ushort MiscellaneousControlDeviceId = 0x1103;
    private const byte MiscellaneousControlFunction = 3;
    private const uint ThermTripStatusRegister = 0xE4;
    
    private readonly uint _miscellaneousControlAddress;

    private Sensor _busClock;
    private Sensor[] _coreClocks;
    private Sensor[] _coreTemperatures;

    private byte _thermSenseCoreSelCpu0;
    private byte _thermSenseCoreSelCpu1;

    #endregion

    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Amd0FCpu"/> class.
    /// </summary>
    /// <param name="processorIndex">Index of the processor.</param>
    /// <param name="cpuId">The cpu identifier.</param>
    /// <param name="settings">The settings.</param>
    public Amd0FCpu(int processorIndex, CpuId[][] cpuId, ISettings settings) : base(processorIndex, cpuId, settings)
    {
        // Set Misc Control Address
        _miscellaneousControlAddress = GetPciAddress(MiscellaneousControlFunction, MiscellaneousControlDeviceId);

        // Sensors
        CreateTemperatureSensors();
        CreateClockSensors();

        // Initialize
        Initialize();

        // Update
        Update();
    }

    #endregion

    #region Methods
    
    /// <summary>
    /// Updates all sensors.
    /// </summary>
    /// <inheritdoc />
    public override void Update()
    {
        // Update Generic CPU
        base.Update();

        // Sensors
        UpdateTemperatureSensors();
        UpdateClockSensors();
    }

    /// <summary>
    /// Gets the MSRS.
    /// </summary>
    /// <returns></returns>
    protected override uint[] GetMsrs() => [FidVidStatus];

    /// <summary>
    /// Create CPU temperature sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateTemperatureSensors()
    {
        uint[,] cpu0ExtData = Cpu0.ExtData;
        float offset = -49.0f;

        // AM2+ 65nm +21 offset
        uint model = Cpu0.Model;
        if (model is >= 0x69 and not 0xc1 and not 0x6c and not 0x7c)
        {
            offset += 21;
        }

        // AMD Athlon 64 Processors
        if (model < 40)
        {
            _thermSenseCoreSelCpu0 = 0x0;
            _thermSenseCoreSelCpu1 = 0x4;
        }
        else
        {
            // AMD NPT Family 0Fh Revision F, G have the core selection swapped
            _thermSenseCoreSelCpu0 = 0x4;
            _thermSenseCoreSelCpu1 = 0x0;
        }

        // Check if processor supports a digital thermal sensor
        if (cpu0ExtData.GetLength(0) > 7 && (cpu0ExtData[7, 3] & 1) != 0)
        {
            _coreTemperatures = new Sensor[CoreCount];
            for (int i = 0; i < CoreCount; i++)
            {
                _coreTemperatures[i] = new Sensor(CpuConstants.CoreNumber + (i + 1),
                    i,
                    SensorType.Temperature,
                    this,
                    [],
                    Settings);
            }
        }
        else
        {
            _coreTemperatures = [];
        }
    }

    /// <summary>
    /// Updates the temperature sensors.
    /// </summary>
    private void UpdateTemperatureSensors()
    {
        // Block
        if (!Mutexes.WaitPciBus(10)) return;

        // Evaluate
        if (_miscellaneousControlAddress != Interop.Ring0.InvalidPciAddress)
        {
            for (uint i = 0; i < _coreTemperatures.Length; i++)
            {
                if (!Ring0.WritePciConfig(_miscellaneousControlAddress,
                        ThermTripStatusRegister,
                        i > 0 ? _thermSenseCoreSelCpu1 : _thermSenseCoreSelCpu0))
                {
                    continue;
                }

                if (Ring0.ReadPciConfig(_miscellaneousControlAddress, ThermTripStatusRegister, out uint value))
                {
                    _coreTemperatures[i].Value = ((value >> 16) & 0xFF) + _coreTemperatures[i].Parameters[0].Value;
                    ActivateSensor(_coreTemperatures[i]);
                }
                else
                {
                    DeactivateSensor(_coreTemperatures[i]);
                }
            }
        }

        // Release
        Mutexes.ReleasePciBus();
    }

    /// <summary>
    /// Create CPU clock sensors.
    /// </summary>
    /// <returns></returns>
    private void CreateClockSensors()
    {
        _busClock = new Sensor(CpuConstants.BusSpeed, 0, SensorType.Clock, this, Settings);
        _coreClocks = new Sensor[CoreCount];
        for (int i = 0; i < _coreClocks.Length; i++)
        {
            _coreClocks[i] = new Sensor(SetCoreName(i), i + 1, SensorType.Clock, this, Settings);
            if (!HasTimeStampCounter) continue;
            ActivateSensor(_coreClocks[i]);
        }
    }

    /// <summary>
    /// Update CPU clock sensors.
    /// </summary>
    /// <returns></returns>
    private void UpdateClockSensors()
    {
        if (!HasTimeStampCounter) return;

         // Bus Clock
        double newBusClock = 0;
        for (int i = 0; i < _coreClocks.Length; i++)
        {
            Thread.Sleep(1);

            if (Ring0.ReadMsr(FidVidStatus, out uint eax, out uint _, CpuId[i][0].Affinity))
            {
                // CurrFID can be found in eax bits 0-5, MaxFID in 16-21
                // 8-13 hold StartFID, we don't use that here.
                double curMp = 0.5 * ((eax & 0x3F) + 8);
                double maxMp = 0.5 * ((eax >> 16 & 0x3F) + 8);
                _coreClocks[i].Value = (float)(curMp * TimeStampCounterFrequency / maxMp);
                newBusClock = (float)(TimeStampCounterFrequency / maxMp);
            }
            else
            {
                // Fail-safe value - if the code above fails, we'll use this instead
                _coreClocks[i].Value = (float)TimeStampCounterFrequency;
            }
        }

        // Set sensor
        if (!(newBusClock > 0)) return;
        _busClock.Value = (float)newBusClock;
        ActivateSensor(_busClock);
    }

    #endregion
}
