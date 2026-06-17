using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_Database
{
	[XmlAttribute("Active")]
	public bool Active { get; set; }

	[XmlElement("ProviderName")]
	public string ProviderName { get; set; }

	[XmlElement("Server")]
	public string Server { get; set; }

	[XmlElement("Port")]
	public int Port { get; set; }

	[XmlElement("UserId")]
	public string UserId { get; set; }

	[XmlElement("Password")]
	public string Password { get; set; }

	[XmlElement("DataDirectory")]
	public string DataDirectory { get; set; }

	[XmlElement("BinaryDirectory")]
	public string BinaryDirectory { get; set; }

	[XmlElement("MinPoolSize")]
	public int MinPoolSize { get; set; }

	[XmlElement("MaxPoolSize")]
	public int MaxPoolSize { get; set; }

	[XmlElement("ConnectionLifeTime")]
	public int ConnectionLifeTime { get; set; }

	[XmlElement("CommandTimeout")]
	public int CommandTimeout { get; set; }

	[XmlElement("PersistSecurityInfo")]
	public bool PersistSecurityInfo { get; set; }

	[XmlElement("IntegratedSecurity")]
	public bool IntegratedSecurity { get; set; }

	[XmlElement("MultipleActiveResultSets")]
	public bool MultipleActiveResultSets { get; set; }
}
