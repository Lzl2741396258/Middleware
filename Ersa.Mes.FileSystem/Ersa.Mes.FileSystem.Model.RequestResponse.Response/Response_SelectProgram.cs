using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Response;

[Serializable]
[XmlRoot("STRUCT_ProgrammAuswaehlen_Stat_EL")]
public class Response_SelectProgram
{
	[XmlElement("Resultat")]
	public Struct_SolderingProgramError m_sttResult;

	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader { get; set; }

	[XmlElement("Loetprogramm")]
	public Struct_SolderingProgram m_sttSolderingProgram { get; set; }
}
