using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public class Edc_ParameterMeasuring
{
	[XmlAttribute("equipment")]
	public string m_strEquipment { get; set; }

	[XmlElement("channel")]
	public Edc_MeasuringChannel m_strChannel { get; set; }
}
