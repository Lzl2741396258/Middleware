using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_ProgrammAuswaehlen_Anfo_LE")]
public class Request_SelectProgram
{
	[XmlElement("Header")]
	[JsonProperty(PropertyName = "Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Loetprogramm")]
	[JsonProperty(PropertyName = "SolderingProgram")]
	public Struct_SolderingProgram m_sttSolderingProgram { get; set; }
}
