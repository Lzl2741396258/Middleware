using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Bib
{
	[XmlElement("stfBib")]
	public string m_strLibraryName { get; set; } = string.Empty;

}
