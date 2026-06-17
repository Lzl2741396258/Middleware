using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_Maschinenzustand_Anfo_EL")]
public class Request_MachineCondition
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithoutTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("MDECode")]
	[JsonProperty(PropertyName = "OeeCode")]
	public Struct_OEECoding m_sttOeeCode { get; set; }

	[XmlElement("MDECodeText")]
	[JsonProperty(PropertyName = "OeeText")]
	public string m_strOeeText { get; set; }

	[XmlElement("Produktname")]
	[JsonProperty(PropertyName = "ProductName")]
	public string[] ma_strProductName { get; set; }

	[XmlElement("Charge")]
	[JsonProperty(PropertyName = "Charge")]
	public string m_strCharge { get; set; }

	[XmlElement("ParameterGruppe")]
	[JsonProperty(PropertyName = "ParameterGroup")]
	public Struct_ParameterGroup[] ma_sttParameterGroup { get; set; }

	[XmlElement("LoetgutInMaschine")]
	[JsonProperty(PropertyName = "SolderInMachine")]
	public Struct_SolderingInMachine m_sttSolderingInMachine { get; set; }

	[XmlElement("Loetprogramm")]
	[JsonProperty(PropertyName = "SolderingProgram")]
	public Struct_SolderingProgram[] ma_sttSolderingProgram { get; set; }
}
