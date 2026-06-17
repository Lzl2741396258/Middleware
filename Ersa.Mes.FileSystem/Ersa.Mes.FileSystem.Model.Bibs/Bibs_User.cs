using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Bibs_User
{
	[XmlElement("stfBenutzer")]
	public string m_strUserName { get; set; } = string.Empty;

}
