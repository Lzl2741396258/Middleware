using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_AuslaufProtokollErstellen_Anfo_EL")]
public class Request_OutfeedCreateProtocol
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

	[XmlElement("Loetergebnis")]
	[JsonProperty(PropertyName = "State")]
	public Enum_Status m_enmState;

	[XmlElement("Code")]
	[JsonProperty(PropertyName = "Code")]
	public string[] ma_strCode;

	[XmlElement("LoetprotokollPfad")]
	[JsonProperty(PropertyName = "SolderingProtocolPath")]
	public string m_strSolderingProtocolPath;

	[XmlElement("LoetprotokollDateiname")]
	[JsonProperty(PropertyName = "SolderingProtocolFilename")]
	public string m_strSolderingProtocolFilename;

	[XmlElement("ParameterGruppe")]
	[JsonProperty(PropertyName = "ParameterGroup")]
	public Struct_ParameterGroup[] m_sttParameterGroup;

	[XmlElement("LoetprotokollElemente")]
	[JsonProperty(PropertyName = "ProtocolElement")]
	public Struct_ProtocolElement[] ma_sttProtocolElement;
}
