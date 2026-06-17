using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Edc_Result
{
	[XmlAttribute("Code")]
	public Enum_ResponseCode m_enuResultCode { get; set; } = Enum_ResponseCode.NichtDefiniert;


	[XmlIgnore]
	public int m_i32Number { get; set; } = 0;


	[XmlAttribute("Text")]
	public string m_strText { get; set; } = string.Empty;

}
