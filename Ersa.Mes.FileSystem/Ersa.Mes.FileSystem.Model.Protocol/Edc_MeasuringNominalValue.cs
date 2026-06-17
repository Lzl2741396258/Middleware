using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_MeasuringNominalValue
{
	[XmlAttribute("value")]
	public string m_strValue { get; set; }
}
