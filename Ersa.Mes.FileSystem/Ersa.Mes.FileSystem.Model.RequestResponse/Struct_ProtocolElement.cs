using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

[XmlRoot("LoetProtokoll")]
public class Struct_ProtocolElement
{
	[JsonIgnore]
	[XmlIgnore]
	public bool m_blnConfig;

	[JsonIgnore]
	[XmlIgnore]
	public string m_strNameAdditionalProtocolLanguage;

	[JsonProperty(PropertyName = "Type")]
	[XmlAttribute("LoetProtElementTyp")]
	public string m_strType;

	[JsonProperty(PropertyName = "Name")]
	[XmlAttribute("LoetProtElementName")]
	public string m_strName;

	[JsonProperty(PropertyName = "Unit")]
	[XmlAttribute("LoetProtElementEinheit")]
	public string m_strUnit;

	[JsonProperty(PropertyName = "ActualValue")]
	[XmlAttribute("LoetProtElementIstwert")]
	public string m_strActualValue;

	[JsonProperty(PropertyName = "SetValue")]
	[XmlAttribute("LoetProtElementSollwert")]
	public string m_strSetValue;

	[JsonProperty(PropertyName = "OffsetPlus")]
	[XmlAttribute("LoetProtElementToleranzPlus")]
	public string m_strOffsetPlus;

	[JsonProperty(PropertyName = "OffsetMinus")]
	[XmlAttribute("LoetProtElementToleranzMinus")]
	public string m_strOffsetMinus;
}
