using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_PcbAusgelaufen_Anfo_EL")]
public class Request_PCBOutfeed
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enmMachineType;

	[XmlElement("Identifier")]
	[JsonProperty(PropertyName = "Identifier")]
	public Edc_Identifier[] ma_sttIdentifier;

	[XmlElement("Code")]
	[JsonProperty(PropertyName = "Code")]
	public string[] ma_strCode;

	[XmlElement("Produktname")]
	[JsonProperty(PropertyName = "ProductName")]
	public string[] ma_strProductName;

	[XmlElement("ParameterGruppe")]
	[JsonProperty(PropertyName = "ParameterGroup")]
	public Struct_ParameterGroup[] m_sttParameterGroup;
}
