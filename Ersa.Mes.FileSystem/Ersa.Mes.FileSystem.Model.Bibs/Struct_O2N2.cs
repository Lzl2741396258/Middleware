using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_O2N2
{
	[XmlElement("sttO2.bytActive")]
	public int? O2Active { get; set; } = 0;


	[XmlElement("a_sttO2_0_.bytActive")]
	public int? O2Active_0 { get; set; } = 0;


	[XmlElement("a_sttO2_1_.bytActive")]
	public int? O2Active_1 { get; set; } = 0;


	[XmlElement("sttO2.bytRegelActive")]
	public int? O2MeasureActive { get; set; } = 0;


	[XmlElement("a_sttO2_0_.bytRegelActive")]
	public int? O2MeasureActive_0 { get; set; } = 0;


	[XmlElement("a_sttO2_1_.bytRegelActive")]
	public int? O2MeasureActive_1 { get; set; } = 0;


	[XmlElement("sttO2.lngSollwert")]
	public int? m_i32O2SetValue { get; set; } = 0;


	[XmlElement("a_sttO2_0_.lngSollwert")]
	public int? m_i32O2SetValue_0 { get; set; } = 0;


	[XmlElement("a_sttO2_1_.lngSollwert")]
	public int? m_i32O2SetValue_1 { get; set; } = 0;


	[XmlElement("sttO2.lngTolPos")]
	public int? m_i32O2TolerancePlus { get; set; } = 0;


	[XmlElement("a_sttO2_0_.lngTolPos")]
	public int? O2TolerancePlus_0 { get; set; } = 0;


	[XmlElement("a_sttO2_1_.lngTolPos")]
	public int? O2TolerancePlus_1 { get; set; } = 0;


	[XmlElement("sttO2.lngTolNeg")]
	public int? O2ToleranceMinus { get; set; } = 0;


	[XmlElement("a_sttO2_0_.lngTolNeg")]
	public int? O2ToleranceMinus_0 { get; set; } = 0;


	[XmlElement("a_sttO2_1_.lngTolNeg")]
	public int? O2ToleranceMinus_1 { get; set; } = 0;


	[XmlElement("sttN2.bytActive")]
	public bool N2Active { get; set; } = false;


	[XmlElement("a_sttN2_0_.bytActive")]
	public bool N2Active_0 { get; set; } = false;


	[XmlElement("a_sttN2_1_.bytActive")]
	public bool N2Active_1 { get; set; } = false;


	[XmlElement("sttN2.lngSollwert")]
	public double? m_dblN2SetValue { get; set; } = 0.0;


	[XmlElement("a_sttN2_0_.lngSollwert")]
	public double? m_dblN2SetValue_0 { get; set; } = 0.0;


	[XmlElement("a_sttN2_1_.lngSollwert")]
	public double? m_dblN2SetValue_1 { get; set; } = 0.0;

}
