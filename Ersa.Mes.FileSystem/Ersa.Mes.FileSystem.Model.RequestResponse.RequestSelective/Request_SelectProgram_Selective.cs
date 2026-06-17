using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.RequestSelective;

[Serializable]
[XmlRoot("STRUCT_ProgrammAuswaehlen_Anfo_LE")]
public class Request_SelectProgram_Selective
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Loetprogramm")]
	public Struct_SolderingProgram m_sttSolderingProgram { get; set; }
}
