using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Struct_SolderingInMachine
{
	[XmlAttribute("Anzahl")]
	[JsonProperty(PropertyName = "SolderInMachine")]
	public int m_i32Number { get; set; }

	[XmlAttribute("AenderungsStatus")]
	[JsonProperty(PropertyName = "ChangeStatus")]
	public Enum_ChangeStatus m_enmChangeStatus { get; set; }

	[XmlElement("LoetgutInSpur")]
	[JsonProperty(PropertyName = "SolderInTrack")]
	public Struct_SolderInTrack[] ma_sttSolderInTrack { get; set; }
}
