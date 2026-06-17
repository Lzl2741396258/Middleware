using System;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Message;

[Serializable]
public class Edc_ErrorMessage
{
	[XmlElement("ErrorID")]
	public string m_strErrorID { get; set; }

	[XmlElement("Number")]
	public string m_strNumber { get; set; }

	[XmlElement("Kind")]
	public string m_strKind { get; set; }

	[XmlElement("Place1")]
	public string m_strPlace1 { get; set; }

	[XmlElement("Place2")]
	public string m_strPlace2 { get; set; }

	[XmlElement("Place3")]
	public string m_strPlace3 { get; set; }

	[XmlElement("Text")]
	public string m_strText { get; set; }

	[XmlElement("Info")]
	public string m_strInfo { get; set; }

	[XmlElement("Reason")]
	public string m_strReason { get; set; }

	[XmlElement("Remedy")]
	public string m_strRemedy { get; set; }
}
