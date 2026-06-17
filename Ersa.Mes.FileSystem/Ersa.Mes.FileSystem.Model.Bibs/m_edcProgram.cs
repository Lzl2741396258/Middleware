using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

[XmlRoot("PrgName")]
public class m_edcProgram
{
	[XmlElement("stfProg")]
	public string m_strProgramName { get; set; } = string.Empty;

}
