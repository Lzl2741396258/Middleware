using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_PopupAnzeigen_Anfo_LE")]
public class Request_PopupDialog
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Terminalinformation")]
	[JsonProperty(PropertyName = "TerminalInformation")]
	public string m_strTerminalInformation;

	[XmlElement("Darstellung")]
	[JsonProperty(PropertyName = "ShowPopup")]
	public Enum_PopupType m_enmShowPopup;
}
