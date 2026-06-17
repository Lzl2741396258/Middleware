using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public struct Struct_SolderingProgram
{
	[XmlAttribute("Name")]
	[JsonProperty(PropertyName = "Name")]
	public string m_strName { get; set; }

	[XmlAttribute("Bibliothek")]
	[JsonProperty(PropertyName = "Library")]
	public string m_strLibrary { get; set; }

	[XmlAttribute("AenderungsStatus")]
	[JsonProperty(PropertyName = "ChangeStatus")]
	public Enum_ChangeStatus m_enuChangeStatus { get; set; }

	[XmlElement("LoetprogrammParameter")]
	[JsonProperty(PropertyName = "SolderingProgramParameter")]
	public Struct_SolderingProgramParameter[] ma_sttSolderingProgramParameter { get; set; }

	[XmlAttribute("Code")]
	public string m_strCode { get; set; }
}
