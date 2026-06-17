using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse.Request;

public struct STRUCT_Aktiv
{
	[XmlAttribute("Aktiv")]
	[JsonProperty(PropertyName = "Active")]
	public byte m_bytActive;
}
