using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Response;

[Serializable]
[XmlRoot("STRUCT_VerbindungBeenden_Stat_LE")]
public class Response_CloseConnection
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack Header { get; set; }

	[XmlElement("Resultat")]
	public Edc_Result Result { get; set; }
}
