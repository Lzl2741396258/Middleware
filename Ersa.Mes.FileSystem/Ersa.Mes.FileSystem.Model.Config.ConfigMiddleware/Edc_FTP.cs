using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_FTP
{
	[XmlElement("FtpPath")]
	public string m_strFtpPath { get; set; }

	[XmlElement("FtpName")]
	public string m_strFtpName { get; set; }

	[XmlElement("User")]
	public string m_strUser { get; set; }

	[XmlElement("Password")]
	public string m_strPassword { get; set; }
}
