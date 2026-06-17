using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Struct_HeaderWithTrack
{
	[XmlAttribute("Zeitstempel")]
	[JsonProperty(PropertyName = "Timestamp")]
	public DateTime m_dtmTimestamp { get; set; }

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

	[XmlAttribute("Spurnummer")]
	[JsonProperty(PropertyName = "TrackNumber")]
	public byte m_bytTrackNumber { get; set; }

	[XmlAttribute("Version")]
	[JsonProperty(PropertyName = "Version")]
	public string m_strVersion { get; set; }
}
