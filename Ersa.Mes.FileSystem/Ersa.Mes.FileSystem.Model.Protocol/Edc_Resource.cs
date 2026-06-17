using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_Resource
{
	[XmlAttribute("type")]
	public string m_strType { get; set; }

	[XmlAttribute("name")]
	public string m_strName { get; set; }
}
