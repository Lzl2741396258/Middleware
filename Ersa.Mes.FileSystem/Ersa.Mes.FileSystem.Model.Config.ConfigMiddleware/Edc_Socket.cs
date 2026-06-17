using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_Socket
{
	[XmlAttribute("Active")]
	public bool m_blnActive { get; set; }

	[XmlAttribute("IsServer")]
	public bool m_blnServer { get; set; }

	[XmlAttribute("Ip")]
	public string m_strIP { get; set; }

	[XmlAttribute("Port")]
	public int m_i32Port { get; set; }

	[XmlAttribute("Remark")]
	public string m_strRemark { get; set; } = "";

}
