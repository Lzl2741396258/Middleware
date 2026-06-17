using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_MeasuringChannel
{
	[XmlAttribute("name")]
	public string m_strName { get; set; }

	[XmlAttribute("UnitOfMeasure")]
	public string m_strUnitOfMeasure { get; set; }

	[XmlElement("sample")]
	public Edc_MeasuringSample m_strSample { get; set; }

	[XmlElement("limit_hh")]
	public Edc_MeasuringLimit_HH m_sttLimitHH { get; set; }

	[XmlElement("nominalValue")]
	public Edc_MeasuringNominalValue m_strNominalValue { get; set; }

	[XmlElement("limit_ll")]
	public Edc_MeasuringLimit_LL m_sttLimitLL { get; set; }
}
