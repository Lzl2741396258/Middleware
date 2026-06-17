using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.Middleware.Definition;

public struct Struct_ResponseUserLogin
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("Resultat")]
	public Edc_Result m_sttResponse;

	[XmlElement("BenutzerFreigabe")]
	public Enum_UserRelease m_enmBenutzerFreigabe;
}
