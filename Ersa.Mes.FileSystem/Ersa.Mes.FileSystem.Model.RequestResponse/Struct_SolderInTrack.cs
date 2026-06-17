using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Struct_SolderInTrack
{
	[XmlAttribute("SpurNr")]
	[JsonProperty(PropertyName = "TrackNo")]
	public Enum_TrackNo m_enmTrackNo;

	[XmlAttribute("Anzahl")]
	[JsonProperty(PropertyName = "Number")]
	public int m_i32Number;

	[XmlAttribute("AenderungsStatus")]
	[JsonProperty(PropertyName = "ChangeStatus")]
	public Enum_ChangeStatus m_enmChangeStatus;
}
