using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Struct_ParameterWithoutValue
{
	[XmlAttribute("Active")]
	public bool m_blnActive { get; set; } = true;


	[XmlAttribute("Name")]
	public string m_strName { get; set; }

	[XmlAttribute("Spurnummer")]
	public byte m_bytTrackNumber { get; set; }

	[XmlAttribute("Display")]
	public string m_strDisplay { get; set; } = "";

}
