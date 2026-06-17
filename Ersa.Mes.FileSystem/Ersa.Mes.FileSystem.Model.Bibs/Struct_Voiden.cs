using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_Voiden
{
	[XmlElement("sttVoid.bytActive")]
	public int? VoidActive { get; set; } = 0;


	[XmlElement("sttVoid.lngSollZeit")]
	public int? VoidTargetTime { get; set; } = 0;


	[XmlElement("sttVoid.lngDiffBreite")]
	public int? VoidDifferentWidth { get; set; } = 0;


	[XmlElement("sttVoid.sttSigGen.lngSweepzeit")]
	public int? VoidSweepTime { get; set; } = 0;


	[XmlElement("sttVoid.sttSigGen.lngStopFreq")]
	public int? VoidStopFrequency { get; set; } = 0;


	[XmlElement("sttVoid.sttSigGen.lngAmplitude")]
	public int? VoidAmplitude { get; set; } = 0;


	[XmlElement("sttVoid.sttSigGen.enmWellenform")]
	public int? VoidWaveform { get; set; } = 0;


	[XmlElement("sttVoid.enmArtKlemmung")]
	public int? VoidClampingType { get; set; } = 0;


	[XmlElement("sttVoid.a_lngBesKlemmen_0_")]
	public int? VoidBesClamp0 { get; set; } = 0;


	[XmlElement("sttVoid.a_lngMomKlemmen_0_")]
	public double? VoidMom0 { get; set; } = 0.0;


	[XmlElement("sttVoid.a_lngBesKlemmen_1_")]
	public int? VoidBesClamp1 { get; set; } = 0;


	[XmlElement("sttVoid.a_lngMomKlemmen_1_")]
	public double? VoidMom1 { get; set; } = 0.0;

}
