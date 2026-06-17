using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_ParameterProcess
{
	[XmlAttribute("name")]
	public string m_strName { get; set; }

	[XmlAttribute("value")]
	public string m_strValue { get; set; }

	[XmlAttribute("UnitOfMeasure")]
	public string m_MeasureUnit { get; set; }
}
