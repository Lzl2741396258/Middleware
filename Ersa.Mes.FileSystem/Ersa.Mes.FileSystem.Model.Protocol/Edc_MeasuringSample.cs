using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_MeasuringSample
{
	[XmlAttribute("value")]
	public string m_strValue { get; set; }

	[XmlAttribute("time")]
	public string m_strTime { get; set; }
}
