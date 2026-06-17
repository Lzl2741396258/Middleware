using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_MeasuringLimit_HH
{
	[XmlAttribute("value")]
	public string m_strValue { get; set; }

	[XmlAttribute("relative")]
	public string m_strRelative { get; set; }
}
