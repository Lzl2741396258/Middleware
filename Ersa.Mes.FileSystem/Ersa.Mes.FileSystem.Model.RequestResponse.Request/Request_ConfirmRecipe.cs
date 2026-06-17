using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_RezeptBestaetigen_Anfo_EL")]
public class Request_ConfirmRecipe
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	[JsonProperty(PropertyName = "MachineType")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("Identifier")]
	[JsonProperty(PropertyName = "Identifier")]
	public Edc_Identifier[] ma_sttIdentifier { get; set; }

	[XmlElement("LoetprogrammName")]
	[JsonProperty(PropertyName = "ProgramName")]
	public string m_strProgramName { get; set; }

	[XmlElement("BibliothekName")]
	[JsonProperty(PropertyName = "LibraryName")]
	public string m_strLibraryName { get; set; }

	[XmlElement("StatusLoetprogrammStart")]
	[JsonProperty(PropertyName = "StatusProgramStart")]
	public Enum_Status m_enmStatusProgramStart { get; set; }
}
