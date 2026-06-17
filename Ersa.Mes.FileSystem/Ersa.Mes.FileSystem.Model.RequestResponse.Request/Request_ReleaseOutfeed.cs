using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_AuslaufFreigeben_Anfo_EL")]
public class Request_ReleaseOutfeed
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enmMachineType;

	[XmlElement("Loetergebnis")]
	[JsonProperty(PropertyName = "SolderingResult")]
	public Enum_Status m_enmStatus;

	[XmlElement("Identifier")]
	[JsonProperty(PropertyName = "Identifier")]
	public Edc_Identifier[] ma_sttIdentifier;

	[XmlElement("Code")]
	[JsonProperty(PropertyName = "Code")]
	public string[] ma_strCode;
}
