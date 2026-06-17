using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Bibs;

public class Struct_Pyrolyse
{
	[XmlElement("a_sttPyrolyse_0_.bytActive")]
	public int? bytActive0 { get; set; } = -1;


	[XmlElement("a_sttPyrolyse_1_.bytActive")]
	public int? bytActive1 { get; set; } = 0;

}
