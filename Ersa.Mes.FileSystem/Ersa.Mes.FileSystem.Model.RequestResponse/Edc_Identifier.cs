using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public class Edc_Identifier
{
	[XmlAttribute("Wert")]
	[JsonProperty(PropertyName = "Value")]
	public string m_strValue;

	[XmlAttribute("Typ")]
	[JsonProperty(PropertyName = "CodeMeaning")]
	public Enum_CodeMeaning m_enmCodeMeaning;

	[XmlAttribute("Variante")]
	[JsonProperty(PropertyName = "Variant")]
	public string m_strVariant;

	[XmlAttribute("LsgNr")]
	[JsonProperty(PropertyName = "CodereaderNumber")]
	public int m_i32CodereaderNumber;

	[XmlAttribute("Loetstatus")]
	[JsonProperty(PropertyName = "SolderStatus")]
	public Enum_SolderStatus m_enuSolderStatus;

	[XmlAttribute("PositionInVerbund")]
	[JsonProperty(PropertyName = "PositionCombine")]
	public int m_i32PositionCombine;

	[XmlAttribute("SpurNr")]
	[JsonProperty(PropertyName = "TrackNumber")]
	public int m_i32TrackNumber;

	[XmlAttribute("ErfolgreichOhneCode")]
	[JsonProperty(PropertyName = "SuccessfulWithoutCode")]
	public bool m_blnSuccessfulWithoutCode;
}
