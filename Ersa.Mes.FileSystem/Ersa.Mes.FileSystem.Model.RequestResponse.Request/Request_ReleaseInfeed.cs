using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_EinlaufFreigeben_Anfo_EL")]
public class Request_ReleaseInfeed
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("Identifier")]
	[JsonProperty(PropertyName = "Identifier")]
	public Struct_Identifier[] ma_edcIdentifier { get; set; }

	[XmlElement("Code")]
	[JsonProperty(PropertyName = "Code")]
	public string[] ma_strCode { get; set; }

	[XmlElement("Produktname")]
	[JsonProperty(PropertyName = "ProductName")]
	public string[] ma_strProductName { get; set; }

	[XmlElement("Loetprogramm")]
	[JsonProperty(PropertyName = "SolderingProgram")]
	public Struct_SolderingProgram m_sttSolderingProgram { get; set; }
}
