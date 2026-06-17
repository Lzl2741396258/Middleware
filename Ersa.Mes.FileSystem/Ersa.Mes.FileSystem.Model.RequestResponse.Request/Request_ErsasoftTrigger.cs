using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

[Serializable]
[XmlRoot("STRUCT_ErsasoftTriggern_Anfo_LE")]
public class Request_ErsasoftTrigger
{
	[XmlElement("Header")]
	public Struct_HeaderWithTrack m_sttHeader;

	[XmlElement("AnfoNachricht")]
	public Enum_MessageRequest m_enmMessageRequest;
}
