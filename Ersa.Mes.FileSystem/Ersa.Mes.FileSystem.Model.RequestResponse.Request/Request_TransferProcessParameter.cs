using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_ParameterUebertragen_Anfo_EL")]
public class Request_TransferProcessParameter
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("ParameterGruppe")]
	public Struct_ParameterGroup m_sttParameterGroup { get; set; }
}
