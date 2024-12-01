namespace Xcalibur.HardwareMonitor.Framework.Interop.Models.ADL
{
    internal enum GcnFamilies
    {
        /// <summary>
        /// Unknown
        /// </summary>
        FamilyUnknown = 0,

        /// <summary>
        /// Trinity APUs
        /// </summary>
        FamilyTn = 105,

        /// <summary>
        /// Southern Islands: Tahiti, Pitcairn, CapeVerde, Oland, HainanSouthern Islands: Tahiti, Pitcairn, CapeVerde, Oland, Hainan
        /// </summary>
        FamilySi = 110,

        /// <summary>
        /// Sea Islands: Bonaire, Hawaii
        /// </summary>
        FamilyCi = 120,

        /// <summary>
        /// Kaveri, Kabini, Mullins
        /// </summary>
        FamilyKv = 125,

        /// <summary>
        /// Volcanic Islands: Iceland, Tonga, Fiji
        /// </summary>
        FamilyVi = 130,

        /// <summary>
        /// Carrizo APUs: Carrizo, Stoney
        /// </summary>
        FamilyCz = 135,

        /// <summary>
        /// Vega: 10, 20
        /// </summary>
        FamilyAi = 141,

        /// <summary>
        /// Raven (Vega GCN 5.0)
        /// </summary>
        FamilyRv = 142,

        /// <summary>
        /// Navi10, Navi2x
        /// </summary>
        FamilyNv = 143,

        /// <summary>
        /// Van Gogh (RDNA 2.0)
        /// </summary>
        FamilyVgh = 144,

        /// <summary>
        /// Navi: 3x (GC 11.0.0, RDNA 3.0)
        /// </summary>
        FamilyNv3 = 145,

        /// <summary>
        /// Rembrandt (Yellow Carp, RDNA 2.0)
        /// </summary>
        FamilyYc = 146,

        /// <summary>
        /// Phoenix (GC 11.0.1, RDNA 3.0)
        /// </summary>
        FamilyGc1101 = 148,

        /// <summary>
        /// Raphael (GC 10.3.6, RDNA 2.0)
        /// </summary>
        FamilyGc1036 = 149,

        /// <summary>
        /// GC 11.5.0
        /// </summary>
        FamilyGc1150 = 150,

        /// <summary>
        /// Mendocino (GC 10.3.7, RDNA 2.0)
        /// </summary>
        FamilyGc1037 = 151,
    }
}
