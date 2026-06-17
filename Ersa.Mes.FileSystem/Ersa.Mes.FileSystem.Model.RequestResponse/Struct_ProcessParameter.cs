using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

[Serializable]
[XmlRoot("Parameter")]
public class Struct_ProcessParameter
{
	[XmlAttribute("Name")]
	[JsonProperty(PropertyName = "Name")]
	public string m_strName { get; set; }

	[XmlAttribute("Wert")]
	[JsonProperty(PropertyName = "Value")]
	public string m_strValue { get; set; }

	[XmlAttribute("Einheit")]
	[JsonProperty(PropertyName = "Unit")]
	public string m_strUnit { get; set; }

	[XmlAttribute("Spurnummer")]
	[JsonProperty(PropertyName = "TrackNumber")]
	public byte m_bytTrackNumber { get; set; }
}
