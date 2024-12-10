﻿using System;
using Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard.Models;

namespace Xcalibur.HardwareMonitor.Framework.Hardware.Motherboard;

/// <summary>
/// Identification Helper
/// </summary>
internal static class IdentificationHelper
{
    /// <summary>
    /// Gets the manufacturer.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static Manufacturer GetManufacturer(string name) =>
        name switch
        {
            _ when name.IndexOf("abit.com.tw", StringComparison.OrdinalIgnoreCase) > -1 => Manufacturer.Acer,
            _ when name.StartsWith("Acer", StringComparison.OrdinalIgnoreCase) => Manufacturer.Acer,
            _ when name.StartsWith("AMD", StringComparison.OrdinalIgnoreCase) => Manufacturer.AMD,
            _ when name.Equals("Alienware", StringComparison.OrdinalIgnoreCase) => Manufacturer.Alienware,
            _ when name.StartsWith("AOpen", StringComparison.OrdinalIgnoreCase) => Manufacturer.AOpen,
            _ when name.StartsWith("Apple", StringComparison.OrdinalIgnoreCase) => Manufacturer.Apple,
            _ when name.Equals("ASRock", StringComparison.OrdinalIgnoreCase) => Manufacturer.ASRock,
            _ when name.StartsWith("ASUSTeK", StringComparison.OrdinalIgnoreCase) => Manufacturer.ASUS,
            _ when name.StartsWith("ASUS ", StringComparison.OrdinalIgnoreCase) => Manufacturer.ASUS,
            _ when name.StartsWith("Biostar", StringComparison.OrdinalIgnoreCase) => Manufacturer.Biostar,
            _ when name.StartsWith("Clevo", StringComparison.OrdinalIgnoreCase) => Manufacturer.Clevo,
            _ when name.StartsWith("Dell", StringComparison.OrdinalIgnoreCase) => Manufacturer.Dell,
            _ when name.Equals("DFI", StringComparison.OrdinalIgnoreCase) => Manufacturer.DFI,
            _ when name.StartsWith("DFI Inc", StringComparison.OrdinalIgnoreCase) => Manufacturer.DFI,
            _ when name.Equals("ECS", StringComparison.OrdinalIgnoreCase) => Manufacturer.ECS,
            _ when name.StartsWith("ELITEGROUP", StringComparison.OrdinalIgnoreCase) => Manufacturer.ECS,
            _ when name.Equals("EPoX COMPUTER CO., LTD", StringComparison.OrdinalIgnoreCase) => Manufacturer.EPoX,
            _ when name.StartsWith("EVGA", StringComparison.OrdinalIgnoreCase) => Manufacturer.EVGA,
            _ when name.Equals("FIC", StringComparison.OrdinalIgnoreCase) => Manufacturer.FIC,
            _ when name.StartsWith("First International Computer", StringComparison.OrdinalIgnoreCase) => Manufacturer.FIC,
            _ when name.Equals("Foxconn", StringComparison.OrdinalIgnoreCase) => Manufacturer.Foxconn,
            _ when name.StartsWith("Fujitsu", StringComparison.OrdinalIgnoreCase) => Manufacturer.Fujitsu,
            _ when name.StartsWith("Gigabyte", StringComparison.OrdinalIgnoreCase) => Manufacturer.Gigabyte,
            _ when name.StartsWith("Hewlett-Packard", StringComparison.OrdinalIgnoreCase) => Manufacturer.HP,
            _ when name.Equals("HP", StringComparison.OrdinalIgnoreCase) => Manufacturer.HP,
            _ when name.Equals("IBM", StringComparison.OrdinalIgnoreCase) => Manufacturer.IBM,
            _ when name.Equals("Intel", StringComparison.OrdinalIgnoreCase) => Manufacturer.Intel,
            _ when name.StartsWith("Intel Corp", StringComparison.OrdinalIgnoreCase) => Manufacturer.Intel,
            _ when name.StartsWith("Jetway", StringComparison.OrdinalIgnoreCase) => Manufacturer.Jetway,
            _ when name.StartsWith("Lenovo", StringComparison.OrdinalIgnoreCase) => Manufacturer.Lenovo,
            _ when name.Equals("LattePanda", StringComparison.OrdinalIgnoreCase) => Manufacturer.LattePanda,
            _ when name.StartsWith("Medion", StringComparison.OrdinalIgnoreCase) => Manufacturer.Medion,
            _ when name.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) => Manufacturer.Microsoft,
            _ when name.StartsWith("Micro-Star International", StringComparison.OrdinalIgnoreCase) => Manufacturer.MSI,
            _ when name.Equals("MSI", StringComparison.OrdinalIgnoreCase) => Manufacturer.MSI,
            _ when name.StartsWith("NEC ", StringComparison.OrdinalIgnoreCase) => Manufacturer.NEC,
            _ when name.Equals("NEC", StringComparison.OrdinalIgnoreCase) => Manufacturer.NEC,
            _ when name.StartsWith("Pegatron", StringComparison.OrdinalIgnoreCase) => Manufacturer.Pegatron,
            _ when name.StartsWith("Samsung", StringComparison.OrdinalIgnoreCase) => Manufacturer.Samsung,
            _ when name.StartsWith("Sapphire", StringComparison.OrdinalIgnoreCase) => Manufacturer.Sapphire,
            _ when name.StartsWith("Shuttle", StringComparison.OrdinalIgnoreCase) => Manufacturer.Shuttle,
            _ when name.StartsWith("Sony", StringComparison.OrdinalIgnoreCase) => Manufacturer.Sony,
            _ when name.StartsWith("Supermicro", StringComparison.OrdinalIgnoreCase) => Manufacturer.Supermicro,
            _ when name.StartsWith("Toshiba", StringComparison.OrdinalIgnoreCase) => Manufacturer.Toshiba,
            _ when name.Equals("XFX", StringComparison.OrdinalIgnoreCase) => Manufacturer.XFX,
            _ when name.StartsWith("Zotac", StringComparison.OrdinalIgnoreCase) => Manufacturer.Zotac,
            _ when name.Equals("To be filled by O.E.M.", StringComparison.OrdinalIgnoreCase) => Manufacturer.Unknown,
            _ => Manufacturer.Unknown
        };

    /// <summary>
    /// Gets the model.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static MotherboardModel GetModel(string name) =>
        name switch
        {
            _ when name.Equals("880GMH/USB3", StringComparison.OrdinalIgnoreCase) => MotherboardModel._880GMH_USB3,
            _ when name.Equals("B85M-DGS", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B85MDgs,
            _ when name.Equals("ASRock AOD790GX/128M", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Aod790Gx128M,
            _ when name.Equals("AB350 Pro4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ab350Pro4,
            _ when name.Equals("AB350M Pro4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ab350MPro4,
            _ when name.Equals("AB350M", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ab350M,
            _ when name.Equals("B450 Steel Legend", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450SteelLegend,
            _ when name.Equals("B450M Steel Legend", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MSteelLegend,
            _ when name.Equals("B450 Pro4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450Pro4,
            _ when name.Equals("B450M Pro4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MPro4,
            _ when name.Equals("Fatal1ty AB350 Gaming K4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Fatal1TyAb350GamingK4,
            _ when name.Equals("AB350M-HDV", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ab350MHdv,
            _ when name.Equals("X399 Phantom Gaming 6", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X399PhantomGaming6,
            _ when name.Equals("A320M-HDV", StringComparison.OrdinalIgnoreCase) => MotherboardModel.A320MHdv,
            _ when name.Equals("P55 Deluxe", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P55Deluxe,
            _ when name.Equals("Crosshair III Formula", StringComparison.OrdinalIgnoreCase) => MotherboardModel.CrosshairIiiFormula,
            _ when name.Equals("ROG CROSSHAIR VIII HERO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairViiiHero,
            _ when name.Equals("ROG CROSSHAIR VIII HERO (WI-FI)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairViiiHeroWifi,
            _ when name.Equals("ROG CROSSHAIR VIII DARK HERO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairViiiDarkHero,
            _ when name.Equals("ROG CROSSHAIR VIII FORMULA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairViiiFormula,
            _ when name.Equals("ROG CROSSHAIR VIII IMPACT", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairViiiImpact,
            _ when name.Equals("PRIME B650-PLUS", StringComparison.OrdinalIgnoreCase) => MotherboardModel.PrimeB650Plus,
            _ when name.Equals("ROG CROSSHAIR X670E EXTREME", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairX670EExtreme,
            _ when name.Equals("ROG CROSSHAIR X670E HERO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairX670EHero,
            _ when name.Equals("ROG CROSSHAIR X670E GENE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogCrosshairX670EGene,
            _ when name.Equals("PROART X670E-CREATOR WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.ProartX670ECreatorWifi,
            _ when name.Equals("M2N-SLI DELUXE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.M2NSliDeluxe,
            _ when name.Equals("M4A79XTD EVO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.M4A79XtdEvo,
            _ when name.Equals("P5W DH Deluxe", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P5WDhDeluxe,
            _ when name.Equals("P6T", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P6T,
            _ when name.Equals("P6X58D-E", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P6X58DE,
            _ when name.Equals("P8P67", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8P67,
            _ when name.Equals("P8P67 REV 3.1", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8P67,
            _ when name.Equals("P8P67 EVO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8P67Evo,
            _ when name.Equals("P8P67 PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8P67Pro,
            _ when name.Equals("P8P67-M PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8P67MPro,
            _ when name.Equals("P8Z77-V", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8Z77V,
            _ when name.Equals("P9X79", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P9X79,
            _ when name.Equals("Rampage Extreme", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RampageExtreme,
            _ when name.Equals("Rampage II GENE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RampageIiGene,
            _ when name.Equals("LP BI P45-T2RS Elite", StringComparison.OrdinalIgnoreCase) => MotherboardModel.LpBiP45T2RsElite,
            _ when name.Equals("ROG STRIX B550-F GAMING (WI-FI)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixB550FGamingWifi,
            _ when name.Equals("ROG STRIX X470-I GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX470I,
            _ when name.Equals("ROG STRIX B550-E GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixB550EGaming,
            _ when name.Equals("ROG STRIX B550-I GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixB550IGaming,
            _ when name.Equals("ROG STRIX X570-E GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX570EGaming,
            _ when name.Equals("ROG STRIX X570-E GAMING WIFI II", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX570EGamingWifiIi,
            _ when name.Equals("ROG STRIX X570-I GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX570IGaming,
            _ when name.Equals("ROG STRIX X570-F GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX570FGaming,
            _ when name.Equals("LP DK P55-T3eH9", StringComparison.OrdinalIgnoreCase) => MotherboardModel.LpDkP55T3Eh9,
            _ when name.Equals("A890GXM-A", StringComparison.OrdinalIgnoreCase) => MotherboardModel.A890GxmA,
            _ when name.Equals("X58 SLI Classified", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X58SliClassified,
            _ when name.Equals("132-BL-E758", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X583XSli,
            _ when name.Equals("965P-S3", StringComparison.OrdinalIgnoreCase) => MotherboardModel._965P_S3,
            _ when name.Equals("EP45-DS3R", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ep45Ds3R,
            _ when name.Equals("EP45-UD3R", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ep45Ud3R,
            _ when name.Equals("EX58-EXTREME", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ex58Extreme,
            _ when name.Equals("EX58-UD3R", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ex58Ud3R,
            _ when name.Equals("G41M-Combo", StringComparison.OrdinalIgnoreCase) => MotherboardModel.G41MCombo,
            _ when name.Equals("G41MT-S2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.G41MtS2,
            _ when name.Equals("G41MT-S2P", StringComparison.OrdinalIgnoreCase) => MotherboardModel.G41MtS2P,
            _ when name.Equals("970A-DS3P", StringComparison.OrdinalIgnoreCase) => MotherboardModel._970A_DS3P,
            _ when name.Equals("970A-DS3P FX", StringComparison.OrdinalIgnoreCase) => MotherboardModel._970A_DS3P,
            _ when name.Equals("GA-970A-UD3", StringComparison.OrdinalIgnoreCase) => MotherboardModel._970A_UD3,
            _ when name.Equals("GA-MA770T-UD3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ma770TUd3,
            _ when name.Equals("GA-MA770T-UD3P", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ma770TUd3P,
            _ when name.Equals("GA-MA785GM-US2H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ma785GmUs2H,
            _ when name.Equals("GA-MA785GMT-UD2H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ma785GmtUd2H,
            _ when name.Equals("GA-MA78LM-S2H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ma78LmS2H,
            _ when name.Equals("GA-MA790X-UD3P", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ma790XUd3P,
            _ when name.Equals("H55-USB3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H55Usb3,
            _ when name.Equals("H55N-USB3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H55NUsb3,
            _ when name.Equals("H61M-DGS", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H61MDgs,
            _ when name.Equals("H61M-DS2 REV 1.2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H61MDs2Rev12,
            _ when name.Equals("H61M-USB3-B3 REV 2.0", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H61MUsb3B3Rev20,
            _ when name.Equals("H67A-UD3H-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H67AUd3HB3,
            _ when name.Equals("H67A-USB3-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H67AUsb3B3,
            _ when name.Equals("H97-D3H-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H97D3H,
            _ when name.Equals("H81M-HD3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.H81MHd3,
            _ when name.Equals("B75M-D3H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B75MD3H,
            _ when name.Equals("P35-DS3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P35Ds3,
            _ when name.Equals("P35-DS3L", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P35Ds3L,
            _ when name.Equals("P55-UD4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P55Ud4,
            _ when name.Equals("P55A-UD3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P55AUd3,
            _ when name.Equals("P55M-UD4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P55MUd4,
            _ when name.Equals("P67A-UD3-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P67AUd3B3,
            _ when name.Equals("P67A-UD3R-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P67AUd3RB3,
            _ when name.Equals("P67A-UD4-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P67AUd4B3,
            _ when name.Equals("P8Z68-V PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.P8Z68VPro,
            _ when name.Equals("X38-DS5", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X38Ds5,
            _ when name.Equals("X58A-UD3R", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X58AUd3R,
            _ when name.Equals("Z270 PC MATE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z270PcMate,
            _ when name.Equals("Z270 PC MATE (MS-7A72)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z270PcMate,
            _ when name.Equals("Z77 MPower", StringComparison.OrdinalIgnoreCase) => // MS-7751 Rev 4.x
                MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77 MPower (MS-7751)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD65", StringComparison.OrdinalIgnoreCase) => // MS-7751 Rev >1.x
                MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD65 (MS-7751)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD65 GAMING", StringComparison.OrdinalIgnoreCase) => // MS-7751 Rev 2.x
                MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD65 GAMING (MS-7751)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD55", StringComparison.OrdinalIgnoreCase) => // MS-7751 Rev 1.x
                MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD55 (MS-7751)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD80", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z77A-GD80 (MS-7757)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Ms7751,
            _ when name.Equals("Z68A-GD80", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68Ms7672,
            _ when name.Equals("Z68A-GD80 (MS-7672)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68Ms7672,
            _ when name.Equals("P67A-GD80", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68Ms7672,
            _ when name.Equals("P67A-GD80 (MS-7672)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68Ms7672,
            _ when name.Equals("X79-UD3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X79Ud3,
            _ when name.Equals("Z68A-D3H-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68AD3HB3,
            _ when name.Equals("Z68AP-D3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68ApD3,
            _ when name.Equals("Z68X-UD3H-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68XUd3HB3,
            _ when name.Equals("Z68X-UD7-B3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68XUd7B3,
            _ when name.Equals("Z68XP-UD3R", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z68XpUd3R,
            _ when name.Equals("Z170N-WIFI-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z170NWifi,
            _ when name.Equals("Z390 M GAMING-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z390MGaming,
            _ when name.Equals("Z390 AORUS ULTRA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z390AorusUltra,
            _ when name.Equals("Z390 AORUS PRO-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z390AorusPro,
            _ when name.Equals("Z390 UD", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z390Ud,
            _ when name.Equals("Z690 AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690AorusPro,
            _ when name.Equals("Z690 AORUS ULTRA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690AorusUltra,
            _ when name.Equals("Z690 AORUS MASTER", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690AorusMaster,
            _ when name.Equals("Z690 GAMING X DDR4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690GamingXDdr4,
            _ when name.Equals("Z790 AORUS PRO X", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790AorusProX,
            _ when name.Equals("Z790 AORUS PRO X WIFI7", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790AorusProX,
            _ when name.Equals("Z790 UD", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790Ud,
            _ when name.Equals("Z790 UD AC", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790UdAc,
            _ when name.Equals("Z790 GAMING X", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790GamingX,
            _ when name.Equals("Z790 GAMING X AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790GamingXAx,
            _ when name.Equals("FH67", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Fh67,
            _ when name.Equals("AX370-Gaming K7", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ax370GamingK7,
            _ when name.Equals("PRIME X370-PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.PrimeX370Pro,
            _ when name.Equals("PRIME X470-PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.PrimeX470Pro,
            _ when name.Equals("PRIME X570-PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.PrimeX570Pro,
            _ when name.Equals("ProArt X570-CREATOR WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.ProartX570CreatorWifi,
            _ when name.Equals("Pro WS X570-ACE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.ProWsX570Ace,
            _ when name.Equals("ROG MAXIMUS X APEX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusXApex,
            _ when name.Equals("AB350-Gaming 3-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ab350Gaming3,
            _ when name.Equals("X399 AORUS Gaming 7", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X399AorusGaming7,
            _ when name.Equals("ROG ZENITH EXTREME", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogZenithExtreme,
            _ when name.Equals("ROG ZENITH II EXTREME", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogZenithIiExtreme,
            _ when name.Equals("Z170-A", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z170A,
            _ when name.Equals("B150M-C", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B150MC,
            _ when name.Equals("B150M-C D3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B150MCD3,
            _ when name.Equals("Z77 Pro4-M", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z77Pro4M,
            _ when name.Equals("X570 Pro4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570Pro4,
            _ when name.Equals("X570 Taichi", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570Taichi,
            _ when name.Equals("X570 Phantom Gaming-ITX/TB3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570PhantomGamingItx,
            _ when name.Equals("X570 Phantom Gaming 4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570PhantomGaming4,
            _ when name.Equals("AX370-Gaming 5", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Ax370Gaming5,
            _ when name.Equals("TUF X470-PLUS GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.TufX470PlusGaming,
            _ when name.Equals("B360M PRO-VDH (MS-7B24)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B360MProVdh,
            _ when name.Equals("B550-A PRO (MS-7C56)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550APro,
            _ when name.Equals("PRO B550-VC (MS-7C56)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550APro,
            _ when name.Equals("B450-A PRO (MS-7B86)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450APro,
            _ when name.Equals("B350 GAMING PLUS (MS-7A34)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B350GamingPlus,
            _ when name.Equals("B450 AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450AorusPro,
            _ when name.Equals("B450 AORUS PRO WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450AorusPro,
            _ when name.Equals("B450 GAMING X", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450GamingX,
            _ when name.Equals("B450 AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450AorusElite,
            _ when name.Equals("B450 AORUS ELITE V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450AorusElite,
            _ when name.Equals("B450M AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MAorusElite,
            _ when name.Equals("B450M AORUS ELITE-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MAorusElite,
            _ when name.Equals("B450M GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MGaming,
            _ when name.Equals("B450M GAMING-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MGaming,
            _ when name.Equals("B450M AORUS M", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450AorusM,
            _ when name.Equals("B450M AORUS M-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450AorusM,
            _ when name.Equals("B450M DS3H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MDs3H,
            _ when name.Equals("B450M DS3H WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MDs3H,
            _ when name.Equals("B450M DS3H-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MDs3H,
            _ when name.Equals("B450M DS3H WIFI-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MDs3H,
            _ when name.Equals("B450M DS3H V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MDs3H,
            _ when name.Equals("B450M DS3H V2-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MDs3H,
            _ when name.Equals("B450M S2H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MS2H,
            _ when name.Equals("B450M S2H V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MS2H,
            _ when name.Equals("B450M S2H-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MS2H,
            _ when name.Equals("B450M S2H V2-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MS2H,
            _ when name.Equals("B450M H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MH,
            _ when name.Equals("B450M H-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MH,
            _ when name.Equals("B450M K", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MK,
            _ when name.Equals("B450M K-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450MK,
            _ when name.Equals("B450M I AORUS PRO WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450IAorusProWifi,
            _ when name.Equals("B450M I AORUS PRO WIFI-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B450IAorusProWifi,
            _ when name.Equals("X470 AORUS GAMING 7 WIFI-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X470AorusGaming7Wifi,
            _ when name.Equals("X570 AORUS MASTER", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570AorusMaster,
            _ when name.Equals("X570 AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570AorusPro,
            _ when name.Equals("X570 AORUS ULTRA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570AorusUltra,
            _ when name.Equals("X570 GAMING X", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570GamingX,
            _ when name.Equals("TUF GAMING X570-PLUS (WI-FI)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.TufGamingX570PlusWifi,
            _ when name.Equals("TUF GAMING B550M-PLUS (WI-FI)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.TufGamingB550MPlusWifi,
            _ when name.Equals("B360 AORUS GAMING 3 WIFI-CF", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B360AorusGaming3WifiCf,
            _ when name.Equals("B550I AORUS PRO AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550IAorusProAx,
            _ when name.Equals("B550M AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MAorusPro,
            _ when name.Equals("B550M AORUS PRO-P", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MAorusPro,
            _ when name.Equals("B550M AORUS PRO AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MAorusProAx,
            _ when name.Equals("B550M AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MAorusElite,
            _ when name.Equals("B550M GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MGaming,
            _ when name.Equals("B550M DS3H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MDs3H,
            _ when name.Equals("B550M DS3H AC", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MDs3HAc,
            _ when name.Equals("B550M S2H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MS2H,
            _ when name.Equals("B550M H", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550MH,
            _ when name.Equals("B550 AORUS MASTER", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusMaster,
            _ when name.Equals("B550 AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusPro,
            _ when name.Equals("B550 AORUS PRO V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusPro,
            _ when name.Equals("B550 AORUS PRO AC", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusProAc,
            _ when name.Equals("B550 AORUS PRO AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusProAx,
            _ when name.Equals("B550 VISION D", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550VisionD,
            _ when name.Equals("B550 VISION D-P", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550VisionD,
            _ when name.Equals("B550 AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusElite,
            _ when name.Equals("B550 AORUS ELITE V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusElite,
            _ when name.Equals("B550 AORUS ELITE AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusEliteAx,
            _ when name.Equals("B550 AORUS ELITE AX V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusEliteAx,
            _ when name.Equals("B550 AORUS ELITE AX V3", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550AorusEliteAx,
            _ when name.Equals("B550 GAMING X", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550GamingX,
            _ when name.Equals("B550 GAMING X V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550GamingX,
            _ when name.Equals("B550 UD AC", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550UdAc,
            _ when name.Equals("B550 UD AC-Y1", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B550UdAc,
            _ when name.Equals("B560M AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B560MAorusElite,
            _ when name.Equals("B560M AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B560MAorusPro,
            _ when name.Equals("B560M AORUS PRO AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B560MAorusProAx,
            _ when name.Equals("B560I AORUS PRO AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B560IAorusProAx,
            _ when name.Equals("B650 AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650AorusElite,
            _ when name.Equals("B650 AORUS ELITE AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650AorusEliteAx,
            _ when name.Equals("B650 AORUS ELITE V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650AorusEliteV2,
            _ when name.Equals("B650 AORUS ELITE AX V2", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650AorusEliteAxV2,
            _ when name.Equals("B650 AORUS ELITE AX ICE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650AorusEliteAxIce,
            _ when name.Equals("B650E AORUS ELITE AX ICE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650EAorusEliteAxIce,
            _ when name.Equals("B650M AORUS PRO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MAorusPro,
            _ when name.Equals("B650M AORUS PRO AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MAorusProAx,
            _ when name.Equals("B650M AORUS ELITE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MAorusElite,
            _ when name.Equals("B650M AORUS ELITE AX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MAorusEliteAx,
            _ when name.Equals("ROG STRIX Z390-E GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixZ390EGaming,
            _ when name.Equals("ROG STRIX Z390-F GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixZ390FGaming,
            _ when name.Equals("ROG STRIX Z390-I GAMING", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixZ390IGaming,
            _ when name.Equals("ROG STRIX Z690-A GAMING WIFI D4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixZ690AGamingWifiD4,
            _ when name.Equals("ROG MAXIMUS XI FORMULA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusXiFormula,
            _ when name.Equals("ROG MAXIMUS XII FORMULA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusXiiZ490Formula,
            _ when name.Equals("ROG MAXIMUS X HERO (WI-FI AC)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusXHeroWifiAc,
            _ when name.Equals("ROG MAXIMUS Z690 FORMULA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusZ690Formula,
            _ when name.Equals("ROG MAXIMUS Z690 HERO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusZ690Hero,
            _ when name.Equals("ROG MAXIMUS Z690 EXTREME GLACIAL", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusZ690ExtremeGlacial,
            _ when name.Equals("ROG STRIX X670E-E GAMING WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX670EEGamingWifi,
            _ when name.Equals("ROG STRIX X670E-F GAMING WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixX670EFGamingWifi,
            _ when name.Equals("B660GTN", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B660Gtn,
            _ when name.Equals("X670E VALKYRIE", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X670EValkyrie,
            _ when name.Equals("ROG MAXIMUS Z790 HERO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusZ790Hero,
            _ when name.Equals("ROG MAXIMUS Z790 DARK HERO", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusZ790DarkHero,
            _ when name.Equals("PRIME Z690-A", StringComparison.OrdinalIgnoreCase) => MotherboardModel.PrimeZ690A,
            _ when name.Equals("Z690 Steel Legend WiFi 6E", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690SteelLegend,
            _ when name.Equals("Z690 Steel Legend", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690SteelLegend,
            _ when name.Equals("Z690 Extreme WiFi 6E", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690Extreme,
            _ when name.Equals("Z690 Extreme", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z690Extreme,
            _ when name.Equals("Z790 Pro RS", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790ProRs,
            _ when name.Equals("Z790 Pro RS WiFi", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790ProRs,
            _ when name.Equals("Z790 Taichi", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790Taichi,
            _ when name.Equals("Z790 Taichi Carrara", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Z790Taichi,
            _ when name.Equals("B650M-C", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MC,
            _ when name.Equals("B650M-CW", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MC,
            _ when name.Equals("B650M-CX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MC,
            _ when name.Equals("B650M-CWX", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B650MC,
            _ when name.Equals("B660 DS3H DDR4-Y1", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B660Ds3HDdr4,
            _ when name.Equals("B660 DS3H DDR4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B660Ds3HDdr4,
            _ when name.Equals("B660 DS3H AC DDR4-Y1", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B660Ds3HAcDdr4,
            _ when name.Equals("B660 DS3H AC DDR4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B660Ds3HAcDdr4,
            _ when name.Equals("B660M DS3H AX DDR4", StringComparison.OrdinalIgnoreCase) => MotherboardModel.B660MDs3HAxDdr4,
            _ when name.Equals("ROG STRIX Z790-I GAMING WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixZ790IGamingWifi,
            _ when name.Equals("ROG STRIX Z790-E GAMING WIFI", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogStrixZ790EGamingWifi,
            _ when name.Equals("MPG X570 GAMING PLUS (MS-7C37)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.X570GamingPlus,
            _ when name.Equals("ROG MAXIMUS Z790 FORMULA", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusZ790Formula,
            _ when name.Equals("ROG MAXIMUS XII HERO (WI-FI)", StringComparison.OrdinalIgnoreCase) => MotherboardModel.RogMaximusXiiHeroWifi,
            _ when name.Equals("Base Board Product Name", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Unknown,
            _ when name.Equals("To be filled by O.E.M.", StringComparison.OrdinalIgnoreCase) => MotherboardModel.Unknown,
            _ => MotherboardModel.Unknown
        };
}
