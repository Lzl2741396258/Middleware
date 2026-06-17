using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public struct Struct_OEECoding
{
	[XmlText]
	[JsonProperty(PropertyName = "MDECode")]
	public Enum_OEECode m_enuMDECode;

	[XmlAttribute("AenderungsStatus")]
	[JsonProperty(PropertyName = "ChangeStatus")]
	public Enum_ChangeStatus m_enuChangeStatus;
}
