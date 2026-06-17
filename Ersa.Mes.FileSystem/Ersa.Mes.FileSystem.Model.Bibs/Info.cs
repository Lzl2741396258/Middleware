using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Info
{
	[XmlElement("stfInfo")]
	public string m_strInfo { get; set; } = string.Empty;

}
