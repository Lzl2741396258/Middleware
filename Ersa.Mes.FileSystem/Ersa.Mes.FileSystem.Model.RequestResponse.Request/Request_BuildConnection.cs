using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_VerbindungAufbauen_Anfo_EL")]
public class Request_BuildConnection
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enuMachineType { get; set; }
}
