using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_Fast
{
	[XmlElement("sttSchnell.bytActive")]
	public int? FastActive { get; set; } = 0;

}
