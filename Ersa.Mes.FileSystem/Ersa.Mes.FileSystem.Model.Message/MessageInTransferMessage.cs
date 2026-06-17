using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.Message;

[Serializable]
public class MessageInTransferMessage
{
	[XmlAttribute("NummerMeldeort1")]
	[JsonProperty(PropertyName = "NumberFacility1")]
	public string m_strNumberFacility1 { get; set; }

	[XmlAttribute("NummerMeldeort2")]
	[JsonProperty(PropertyName = "NumberFacility2")]
	public string m_strNumberFacility2 { get; set; }

	[XmlAttribute("NummerMeldeort3")]
	[JsonProperty(PropertyName = "NumberFacility3")]
	public string m_strNumberFacility3 { get; set; }

	[XmlAttribute("NummerMeldetext")]
	[JsonProperty(PropertyName = "NumberMessageText")]
	public string m_strNumberMessageText { get; set; }

	[XmlAttribute("TextMeldeort1")]
	[JsonProperty(PropertyName = "TextFacility1")]
	public string m_strTextFacility1 { get; set; }

	[XmlAttribute("TextMeldeort2")]
	[JsonProperty(PropertyName = "TextFacility2")]
	public string m_strTextFacility2 { get; set; }

	[XmlAttribute("TextMeldeort3")]
	[JsonProperty(PropertyName = "TextFacility3")]
	public string m_strTextFacility3 { get; set; }

	[XmlAttribute("TextMeldetext")]
	[JsonProperty(PropertyName = "MessageText")]
	public string m_strMessageText { get; set; }

	[XmlAttribute("Status")]
	[JsonProperty(PropertyName = "State")]
	public Enum_MessageState m_enuState { get; set; }

	[XmlAttribute("Betriebsart")]
	[JsonProperty(PropertyName = "OperatingMode")]
	public Enum_OperatingMode m_enuOperatingMode { get; set; }

	[XmlAttribute("ZeitstempelAufgetreten")]
	[JsonProperty(PropertyName = "TimestampOccurred")]
	public string m_strTimestampOccurred { get; set; }

	[XmlAttribute("Typ")]
	[JsonProperty(PropertyName = "Type")]
	public Enum_MessageType m_enuType { get; set; }

	[XmlAttribute("Benutzer")]
	[JsonProperty(PropertyName = "User")]
	public string m_strUser { get; set; }

	[XmlAttribute("ZeitstempelQuittiert")]
	[JsonProperty(PropertyName = "DatetimeAcknowledged")]
	public string m_strDatetimeAcknowledged { get; set; }
}
