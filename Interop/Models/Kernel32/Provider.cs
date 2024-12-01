namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.Kernel32
{
    public enum Provider
    {
        Acpi = (byte)'A' << 24 | (byte)'C' << 16 | (byte)'P' << 8 | (byte)'I',
        Firm = (byte)'F' << 24 | (byte)'I' << 16 | (byte)'R' << 8 | (byte)'M',
        Rsmb = (byte)'R' << 24 | (byte)'S' << 16 | (byte)'M' << 8 | (byte)'B'
    }
}
