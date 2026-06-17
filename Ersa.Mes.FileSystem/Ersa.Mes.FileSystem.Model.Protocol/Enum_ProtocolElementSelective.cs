using System.ComponentModel;
using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public enum Enum_ProtocolElementSelective
{
	[DescriptionGerman("LfdNr")]
	[DescriptionEnglish("Serial board number")]
	[DescriptionChinese("序列卡号")]
	SerialBoardNumber = 0,
	[DescriptionGerman("Produkt")]
	[DescriptionEnglish("Product")]
	[DescriptionChinese("产品")]
	Product = 1,
	[DescriptionGerman("Charge")]
	[DescriptionEnglish("Batch")]
	[DescriptionChinese("批次")]
	Batch = 2,
	[Description("Recipe")]
	[DescriptionGerman("Rezept")]
	[DescriptionEnglish("Recipe")]
	[DescriptionChinese("配方")]
	Recipe = 3,
	[DescriptionGerman("Traeger")]
	[DescriptionEnglish("Carrier")]
	[DescriptionChinese("载具")]
	Carrier = 4,
	[Description("Code")]
	[DescriptionEnglish("Code")]
	[DescriptionChinese("代码")]
	Barcode = 5,
	[Description("First code meaning")]
	BedeutungErsterCode = 6,
	[DescriptionGerman("Bibliothek")]
	[DescriptionEnglish("Library")]
	[DescriptionChinese("程序库")]
	Library = 7,
	[Description("Program")]
	Programm = 8,
	[Description("Error")]
	[DescriptionEnglish("Error")]
	Fehler = 9,
	[Description("Track-Number")]
	SpurNr = 10,
	[Description("Running in moment")]
	EinlaufZeitpunkt = 11,
	[Description("Running out moment")]
	AuslaufZeitpunkt = 12,
	[Description("Conveyor width")]
	Transportbreite = 13,
	Fm2__Gm_ZeitPcbImModul = 14,
	Fm2__FluxerKopfNr1 = 15,
	Fm2__FluxerKopfNr2 = 16,
	Fm2__FluxerKopfNr3 = 17,
	Fm2__FluxerKopfNr4 = 18,
	Fm2__Ftu1_Verbrauch_Ist = 19,
	Fm2__Ftu2_Verbrauch_Ist = 20,
	Fm2__Bda1_Druck_Ist = 21,
	Fm2__Bda2_Druck_Ist = 22,
	Fm2__KorrekturKoordinateX = 23,
	Fm2__KorrekturKoordinateY = 24,
	Fm2__KorrekturDrehwinkel = 25,
	Fm2__FssNpp_Prt = 26,
	Fm2__Fss_Benutzt = 27,
	[Description("Time in unit - Flux unit")]
	Fm1__Gm_ZeitPcbImModul = 28,
	[Description("Flux head 1 active - Flux unit")]
	Fm1__FluxerKopfNr1 = 29,
	Fm1__FluxerKopfNr2 = 30,
	Fm1__FluxerKopfNr3 = 31,
	Fm1__FluxerKopfNr4 = 32,
	Fm1__Ftu1_Verbrauch_Ist = 33,
	Fm1__Ftu2_Verbrauch_Ist = 34,
	Fm1__Bda1_Druck_Ist = 35,
	Fm1__Bda2_Druck_Ist = 36,
	Fm1__KorrekturKoordinateX = 37,
	Fm1__KorrekturKoordinateY = 38,
	Fm1__KorrekturDrehwinkel = 39,
	FM1__FssNpp_Prt = 40,
	Fm1__Fss_Benutzt = 41,
	Vm32_Gm_ZeitPcbImModul = 42,
	Vm32_Oh_EndTemp = 43,
	Vm32_Uh_EndTemp = 44,
	Vm32_IstAnzahlNachheizen = 45,
	Vm31_Gm_ZeitPcbImModul = 46,
	Vm31_Oh_EndTemp = 47,
	Vm31_Uh_EndTemp = 48,
	Vm31_IstAnzahlNachheizen = 49,
	Vm31_Gm_PcbTempEndwert = 50,
	Vm22_Gm_ZeitPcbImModul = 51,
	Vm22_Oh_EndTemp = 52,
	Vm22_Uh_EndTemp = 53,
	Vm22_IstAnzahlNachheizen = 54,
	Vm21_Gm_ZeitPcbImModul = 55,
	[Description("Temperature - Heating - top - Preheat unit 21")]
	Vm21_Oh_EndTemp = 56,
	[Description("Temperature - Heating - bottom - Preheat unit 21")]
	Vm21_Uh_EndTemp = 57,
	Vm21_IstAnzahlNachheizen = 58,
	Vm21_Gm_PcbTempEndwert = 59,
	Vm12_Gm_ZeitPcbImModul = 60,
	Vm12_Oh_EndTemp = 61,
	Vm12_Uh_EndTemp = 62,
	Vm12_IstAnzahlNachheizen = 63,
	[Description("Time in unit - Preheat unit 11")]
	Vm11_Gm_ZeitPcbImModul = 64,
	[Description("Temperature - Heating - top - Preheat unit 11")]
	Vm11_Oh_EndTemp = 65,
	[Description("Temperature - Heating - bottom - Preheat unit 11")]
	Vm11_Uh_EndTemp = 66,
	[Description("Number of preheat repetitions - Preheat unit 11")]
	Vm11_IstAnzahlNachheizen = 67,
	Vm11_Gm_PcbTempEndwert = 68,
	[Description("Time in unit - Soldering unit 1")]
	Lm1__Gm_ZeitPcbImModul = 69,
	Lm1__Oh_EndTemp = 70,
	[Description("Solder temperature - Soldering unit 1 - Solder pot 1")]
	Lm1__Ti1_Ltt_Soll = 71,
	[Description("Offset - Soldering unit 1 - Solder pot 1")]
	Lm1__Ti1_OffsetDynamisch = 72,
	[Description("Gradient - Soldering unit 1 - Solder pot 1")]
	Lm1__Ti1_Gradient = 73,
	Lm1__Ti1_RestO2_Ist = 74,
	[Description("Solder temperature - Soldering unit 1 - Solder pot 2")]
	Lm1__Ti2_Ltt_Soll = 75,
	[Description("Offset - Soldering unit 1 - Solder pot 2")]
	Lm1__Ti2_OffsetDynamisch = 76,
	[Description("Gradient - Soldering unit 1 - Solder pot 2")]
	Lm1__Ti2_Gradient = 77,
	Lm1__LeiterkartenDurchbiegungZ = 78,
	Lm1__KorrekturKoordinateX = 79,
	Lm1__KorrekturKoordinateY = 80,
	Lm1__KorrekturDrehwinkel = 81,
	Lm1__Nh1Npp_Prt = 82,
	[Description("Name Nozzle combination - Soldering unit 1 - Solder pot 1")]
	Lm1__Ti1_DkNpp_Prt = 83,
	[Description("Name Nozzle combination - Soldering unit 1 - Solder pot 2")]
	Lm1__Ti2_DkNpp_Prt = 84,
	Lm1__Ti1_Nda_Istwert = 85,
	Lm1__Ti1_Nda_InTol = 86,
	Lm1__Ti2_Nda_Istwert = 87,
	Lm1__Ti2_Nda_InTol = 88,
	Lm1__Ti1_Lss_Benutzt = 89,
	Lm1__Ti2_Lss_Benutzt = 90,
	Lm1__Ti1_N2hIst = 91,
	Lm1__Ti2_N2hIst = 92,
	Vm42_Gm_ZeitPcbImModul = 93,
	Vm42_Oh_EndTemp = 94,
	Vm42_Uh_EndTemp = 95,
	Vm42_IstAnzahlNachheizen = 96,
	Vm41_Gm_ZeitPcbImModul = 97,
	Vm41_Oh_EndTemp = 98,
	Vm41_Uh_EndTemp = 99,
	Vm41_IstAnzahlNachheizen = 100,
	Vm41_Gm_PcbTempEndwert = 101,
	Lm2__Gm_ZeitPcbImModul = 102,
	Lm2__Oh_EndTemp = 103,
	Lm2__Ti1_Ltt_Soll = 104,
	Lm2__Ti1_OffsetDynamisch = 105,
	Lm2__Ti1_Gradient = 106,
	Lm2__Ti1_RestO2_Ist = 107,
	Lm2__Ti2_Ltt_Soll = 108,
	Lm2__Ti2_OffsetDynamisch = 109,
	Lm2__Ti2_Gradient = 110,
	Lm2__LeiterkartenDurchbiegungZ = 111,
	Lm2__KorrekturKoordinateX = 112,
	Lm2__KorrekturKoordinateY = 113,
	Lm2__KorrekturDrehwinkel = 114,
	Lm2__Nh1Npp_Prt = 115,
	Lm2__Ti1_DkNpp_Prt = 116,
	Lm2__Ti2_DkNpp_Prt = 117,
	Lm2__Ti1_Nda_Istwert = 118,
	Lm2__Ti1_Nda_InTol = 119,
	Lm2__Ti2_Nda_Istwert = 120,
	Lm2__Ti2_Nda_InTol = 121,
	Lm2__Ti1_Lss_Benutzt = 122,
	Lm2__Ti2_Lss_Benutzt = 123,
	Lm2__Ti1_N2hIst = 124,
	Lm2__Ti2_N2hIst = 125,
	Vm62_Gm_ZeitPcbImModul = 126,
	Vm62_Oh_EndTemp = 127,
	Vm62_Uh_EndTemp = 128,
	Vm62_IstAnzahlNachheizen = 129,
	Vm61_Gm_ZeitPcbImModul = 130,
	Vm61_Oh_EndTemp = 131,
	Vm61_Uh_EndTemp = 132,
	Vm61_IstAnzahlNachheizen = 133,
	Vm61_Gm_PcbTempEndwert = 134,
	Lm3__Gm_ZeitPcbImModul = 135,
	Lm3__Oh_EndTemp = 136,
	Lm3__Ti1_Ltt_Soll = 137,
	Lm3__Ti1_OffsetDynamisch = 138,
	Lm3__Ti1_Gradient = 139,
	Lm3__Ti1_RestO2_Ist = 140,
	Lm3__Ti2_Ltt_Soll = 141,
	Lm3__Ti2_OffsetDynamisch = 142,
	Lm3__Ti2_Gradient = 143,
	Lm3__LeiterkartenDurchbiegungZ = 144,
	Lm3__KorrekturKoordinateX = 145,
	Lm3__KorrekturKoordinateY = 146,
	Lm3__KorrekturDrehwinkel = 147,
	Lm3__Nh1Npp_Prt = 148,
	Lm3__Ti1_DkNpp_Prt = 149,
	Lm3__Ti2_DkNpp_Prt = 150,
	Lm3__Ti1_Nda_Istwert = 151,
	Lm3__Ti1_Nda_InTol = 152,
	Lm3__Ti2_Nda_Istwert = 153,
	Lm3__Ti2_Nda_InTol = 154,
	Lm3__Ti1_Lss_Benutzt = 155,
	Lm3__Ti2_Lss_Benutzt = 156,
	Lm3__Ti1_N2hIst = 157,
	Lm3__Ti2_N2hIst = 158,
	Am1__Gm_ZeitPcbImModul = 159,
	[Description("User name")]
	BenutzerBeiPcbEinlauf = 160,
	NichtDefiniert = 255
}
