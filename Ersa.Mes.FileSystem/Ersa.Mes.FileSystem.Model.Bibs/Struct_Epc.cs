using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_Epc
{
	[XmlElement("sttEpc.bytActive")]
	public int? FastActive { get; set; } = 0;


	[XmlElement("sttEpc.stfFilename")]
	public int? EpcFilename { get; set; } = 0;


	[XmlElement("sttEpc.stfZeitstempel")]
	public int? EpcTimestamp { get; set; } = 0;

}
