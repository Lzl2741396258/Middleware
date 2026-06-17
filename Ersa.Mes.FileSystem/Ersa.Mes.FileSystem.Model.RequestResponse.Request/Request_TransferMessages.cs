using System;
using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.Message;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_MeldungenUebertragen_Anfo_EL")]
public class Request_TransferMessages
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithoutTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("Meldung")]
	[JsonProperty(PropertyName = "Message")]
	public MessageInTransferMessage m_sttMessage { get; set; }

	[XmlElement("Produktname")]
	[JsonProperty(PropertyName = "ProductName")]
	public string[] ma_strProductName { get; set; }

	[XmlElement("ParameterGruppe")]
	[JsonProperty(PropertyName = "ParameterGroup")]
	public Struct_ParameterGroup[] ma_sttParameterGroup { get; set; }
}
