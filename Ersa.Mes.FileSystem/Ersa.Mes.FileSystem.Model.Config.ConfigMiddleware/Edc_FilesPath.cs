using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_FilesPath
{
	[XmlElement("PathConfig")]
	public string m_strPathConfig { get; set; } = string.Empty;


	[XmlElement("PathErsasoft")]
	public string m_strPathErsasoft { get; set; } = string.Empty;


	[XmlElement("PathCodeTable")]
	public string m_strPathCodeTable { get; set; } = string.Empty;


	[XmlElement("PathProtocol")]
	public string m_strPathProtocol { get; set; } = string.Empty;


	[XmlElement("PathTrend")]
	public string m_strPathTrend { get; set; } = string.Empty;


	[XmlElement("PathZ")]
	public string m_strPathZtxt { get; set; } = string.Empty;


	[XmlElement("PathBibs")]
	public string m_strPathBibs { get; set; } = string.Empty;


	[XmlElement("PathConfigTrace05")]
	public string m_strPathConfigTrace05 { get; set; } = string.Empty;


	[XmlElement("PathInitialize")]
	public string m_strPathInitialize { get; set; } = string.Empty;


	[XmlElement("PathData")]
	public string m_strPathData { get; set; } = string.Empty;

}
