using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Response;

[Serializable]
[XmlRoot("STRUCT_Initialisierung_Stat_LE")]
public class Response_Initialize
{
	[XmlElement("ParameterGruppe")]
	public Struct_ParameterGroupWithoutValue[] m_sttParameterGroupWithoutValue;

	[XmlElement("Header")]
	public Struct_HeaderWithTrack Header { get; set; }
}
