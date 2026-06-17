using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;

public class Edc_InterfaceAddress
{
	[XmlElement("ClientIP")]
	public string Pro_strClientIP { get; set; }

	[XmlElement("ClientPort")]
	public int Pro_i32ClientPort { get; set; }
}
