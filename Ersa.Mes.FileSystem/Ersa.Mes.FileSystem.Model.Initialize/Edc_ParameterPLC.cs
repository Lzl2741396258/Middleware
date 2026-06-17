using System.ComponentModel;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Initialize;

public class Edc_ParameterPLC
{
	[XmlAttribute("Task")]
	[Description("Task")]
	public string m_strTask { get; set; }

	[XmlAttribute("PVariable")]
	[Description("PVariable")]
	public string m_strPVariable { get; set; }

	[XmlAttribute("StructMember")]
	[Description("StructMember")]
	public string m_strStructMember { get; set; }

	[XmlAttribute("Value")]
	[Description("Value")]
	public string m_strValue { get; set; }
}
