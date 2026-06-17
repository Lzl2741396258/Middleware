using System.Xml.Serialization;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_ProtocolLoet
{
	[XmlElement("LoetProtElement")]
	public Struct_ProtocolElement[] ma_sttProtocolElement { get; set; }
}
