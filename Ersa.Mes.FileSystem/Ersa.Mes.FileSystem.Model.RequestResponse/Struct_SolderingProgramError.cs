using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Struct_SolderingProgramError
{
	[XmlAttribute("Code")]
	public Enum_SolderingProgramErrorCode m_enuCode;

	[XmlAttribute("Text")]
	public string m_strText;
}
