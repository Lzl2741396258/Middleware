using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Response;

[Serializable]
[XmlRoot("STRUCT_AuslaufProtokollErstellen_Stat_LE")]
public class Response_OutfeedCreateProtocol
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Resultat")]
	public Edc_Result m_sttResult;
}
