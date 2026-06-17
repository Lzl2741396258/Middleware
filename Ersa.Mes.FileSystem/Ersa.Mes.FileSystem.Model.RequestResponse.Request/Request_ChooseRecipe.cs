using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_RezeptAuswaehlen_Anfo_EL")]
public class Request_ChooseRecipe
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("Identifier")]
	public Edc_Identifier m_sttIdentifier { get; set; }
}
