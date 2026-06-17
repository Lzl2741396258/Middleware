using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public struct Struct_HeaderWithoutTrack
{
	[XmlAttribute("Zeitstempel")]
	[JsonProperty(PropertyName = "Time")]
	public string m_strTimestamp { get; set; }

	[XmlAttribute("Prozessname")]
	[JsonProperty(PropertyName = "ProcessName")]
	public string m_strProcessName { get; set; }

	[XmlAttribute("Linienname")]
	[JsonProperty(PropertyName = "LineName")]
	public string m_strLineName { get; set; }

	[XmlAttribute("Stationsname")]
	[JsonProperty(PropertyName = "StationName")]
	public string m_strStationName { get; set; }

	[XmlAttribute("MessageID")]
	[JsonProperty(PropertyName = "MessageId")]
	public string m_strMessageId { get; set; }

	[XmlAttribute("Version")]
	[JsonProperty(PropertyName = "Version")]
	public string m_strVersion { get; set; }
}
