using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_PLC
{
	[XmlElement("ConnectionType")]
	public string m_strConnectionType { get; set; }

	[XmlElement("IP")]
	public string m_strIP { get; set; }

	[XmlElement("Port")]
	public int m_i32Port { get; set; }

	[XmlElement("ServiceName")]
	public string m_strServiceName { get; set; }

	[XmlElement("CpuName")]
	public string m_strCpuName { get; set; }
}
