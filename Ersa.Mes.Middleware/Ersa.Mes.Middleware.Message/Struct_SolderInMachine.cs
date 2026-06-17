using System.Xml.Serialization;

namespace Ersa.Mes.Middleware.Message;

public struct Struct_SolderInMachine
{
	[XmlAttribute("Anzahl")]
	public int Number { get; set; }

	[XmlAttribute("AenderungsStatus")]
	public string ChangeStatus { get; set; }
}
