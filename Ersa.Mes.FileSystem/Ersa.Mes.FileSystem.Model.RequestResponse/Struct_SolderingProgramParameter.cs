using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Struct_SolderingProgramParameter
{
	[XmlAttribute("Name")]
	[JsonProperty(PropertyName = "Name")]
	public string m_strName;

	[XmlAttribute("Einheit")]
	[JsonProperty(PropertyName = "Unit")]
	public string m_strUnit;

	[XmlAttribute("Wert")]
	[JsonProperty(PropertyName = "Value")]
	public double m_dblValue;
}
