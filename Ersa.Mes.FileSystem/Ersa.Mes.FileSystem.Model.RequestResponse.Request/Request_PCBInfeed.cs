using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_PcbEingelaufen_Anfo_EL")]
public class Request_PCBInfeed
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enmMachineType;

	[XmlElement("Charge")]
	[JsonProperty(PropertyName = "Charge")]
	public string m_strCharge;

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
