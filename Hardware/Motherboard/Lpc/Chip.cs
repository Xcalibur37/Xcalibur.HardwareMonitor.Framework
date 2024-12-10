namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc;

/// <summary>
/// Chip types.
/// </summary>
public enum Chip : ushort
{
    Unknown = 0,

    ATK0110 = 0x0110,

    F71808E = 0x0901,
    F71811 = 0x1007,
    F71858 = 0x0507,
    F71862 = 0x0601,
    F71869 = 0x0814,
    F71869A = 0x1007,
    F71878AD = 0x1106,
    F71882 = 0x0541,
    F71889AD = 0x1005,
    F71889ED = 0x0909,
    F71889F = 0x0723,

    IT8613E = 0x8613,
    IT8620E = 0x8620,
    IT8625E = 0x8625,
    IT8628E = 0x8628,
    IT8631E = 0x8631,
    IT8655E = 0x8655,
    IT8665E = 0x8665,
    IT8686E = 0x8686,
    IT8688E = 0x8688,
    IT8689E = 0x8689,
    IT8705F = 0x8705,
    IT8712F = 0x8712,
    IT8716F = 0x8716,
    IT8718F = 0x8718,
    IT8720F = 0x8720,
    IT8721F = 0x8721,
    IT8726F = 0x8726,
    IT8728F = 0x8728,
    IT8771E = 0x8771,
    IT8772E = 0x8772,
    IT8790E = 0x8790,
    IT8792E = 0x8733, // Could also be IT8791E, IT8795E
    IT87952E = 0x8695,

    NCT610XD = 0xC452,
    NCT6771F = 0xB470,
    NCT6776F = 0xC330,
    NCT6779D = 0xC560,
    NCT6791D = 0xC803,
    NCT6792D = 0xC911,
    NCT6792DA = 0xC913,
    NCT6793D = 0xD121,
    NCT6795D = 0xD352,
    NCT6796D = 0xD423,
    NCT6796DR = 0xD42A,
    NCT6797D = 0xD451,
    NCT6798D = 0xD42B,
    NCT6686D = 0xD440,
    NCT6687D = 0xD592,
    NCT6683D = 0xC732,
    NCT6799D = 0xD802,

    W83627DHG = 0xA020,
    W83627DHGP = 0xB070,
    W83627EHF = 0x8800,
    W83627HF = 0x5200,
    W83627THF = 0x8280,
    W83667HG = 0xA510,
    W83667HGB = 0xB350,
    W83687THF = 0x8541,

    Ipmi = 0x4764,
}
