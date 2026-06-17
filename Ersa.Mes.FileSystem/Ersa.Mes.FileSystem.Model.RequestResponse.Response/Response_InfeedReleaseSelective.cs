using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Response;

[Serializable]
[XmlRoot("STRUCT_EinlaufFreigeben_Stat_LE")]
public class Response_InfeedReleaseSelective
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Maschinentyp")]
	public Enum_MachineType m_enuMachineType { get; set; }

	[XmlElement("Resultat")]
	public Edc_Result m_sttResult { get; set; }

	[XmlElement("EinlaufFreigabe")]
	public Enum_ComingRelease m_enuComingRelease { get; set; }
}
