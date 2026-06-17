using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_Initialisierung_Anfo_EL")]
public class Request_Initialize
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enuMachineType;

	[XmlElement("ProzessparameterlistenHolen")]
	[JsonProperty(PropertyName = "GetProcessParametersList")]
	public STRUCT_Aktiv m_sttGetProcessParametersList;
}
