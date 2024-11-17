namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Lpc
{
    /// <summary>
    /// Chip name
    /// </summary>
    internal static class ChipHelper
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="chip">The chip.</param>
        /// <returns></returns>
        public static string GetName(Chip chip) =>
            chip switch
            {
                Chip.ATK0110 => "Asus ATK0110",
                Chip.F71858 => "Fintek F71858",
                Chip.F71862 => "Fintek F71862",
                Chip.F71869 => "Fintek F71869",
                Chip.F71878AD => "Fintek F71878AD",
                Chip.F71869A => "Fintek F71869A/F71811",
                Chip.F71882 => "Fintek F71882",
                Chip.F71889AD => "Fintek F71889AD",
                Chip.F71889ED => "Fintek F71889ED",
                Chip.F71889F => "Fintek F71889F",
                Chip.F71808E => "Fintek F71808E",
                Chip.IT8613E => "ITE IT8613E",
                Chip.IT8620E => "ITE IT8620E",
                Chip.IT8625E => "ITE IT8625E",
                Chip.IT8628E => "ITE IT8628E",
                Chip.IT8631E => "ITE IT8631E",
                Chip.IT8655E => "ITE IT8655E",
                Chip.IT8665E => "ITE IT8665E",
                Chip.IT8686E => "ITE IT8686E",
                Chip.IT8688E => "ITE IT8688E",
                Chip.IT8689E => "ITE IT8689E",
                Chip.IT8705F => "ITE IT8705F",
                Chip.IT8712F => "ITE IT8712F",
                Chip.IT8716F => "ITE IT8716F",
                Chip.IT8718F => "ITE IT8718F",
                Chip.IT8720F => "ITE IT8720F",
                Chip.IT8721F => "ITE IT8721F",
                Chip.IT8726F => "ITE IT8726F",
                Chip.IT8728F => "ITE IT8728F",
                Chip.IT8771E => "ITE IT8771E",
                Chip.IT8772E => "ITE IT8772E",
                Chip.IT8790E => "ITE IT8790E",
                Chip.IT8792E => "ITE IT8791E/IT8792E/IT8795E",
                Chip.IT87952E => "ITE IT87952E",
                Chip.NCT610XD => "Nuvoton NCT6102D/NCT6104D/NCT6106D",
                Chip.NCT6771F => "Nuvoton NCT6771F",
                Chip.NCT6776F => "Nuvoton NCT6776F",
                Chip.NCT6779D => "Nuvoton NCT6779D",
                Chip.NCT6791D => "Nuvoton NCT6791D",
                Chip.NCT6792D => "Nuvoton NCT6792D",
                Chip.NCT6792DA => "Nuvoton NCT6792D-A",
                Chip.NCT6793D => "Nuvoton NCT6793D",
                Chip.NCT6795D => "Nuvoton NCT6795D",
                Chip.NCT6796D => "Nuvoton NCT6796D",
                Chip.NCT6796DR => "Nuvoton NCT6796D-R",
                Chip.NCT6797D => "Nuvoton NCT6797D",
                Chip.NCT6798D => "Nuvoton NCT6798D",
                Chip.NCT6799D => "Nuvoton NCT6799D",
                Chip.NCT6686D => "Nuvoton NCT6686D",
                Chip.NCT6687D => "Nuvoton NCT6687D",
                Chip.NCT6683D => "Nuvoton NCT6683D",
                Chip.W83627DHG => "Winbond W83627DHG",
                Chip.W83627DHGP => "Winbond W83627DHG-P",
                Chip.W83627EHF => "Winbond W83627EHF",
                Chip.W83627HF => "Winbond W83627HF",
                Chip.W83627THF => "Winbond W83627THF",
                Chip.W83667HG => "Winbond W83667HG",
                Chip.W83667HGB => "Winbond W83667HG-B",
                Chip.W83687THF => "Winbond W83687THF",
                Chip.IPMI => "IPMI",
                _ => "Unknown"
            };
    }
}
