using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public struct Struct_Identifier
{
	[XmlAttribute("Wert")]
	[JsonProperty(PropertyName = "Value")]
	public string m_strValue;

	[XmlAttribute("Typ")]
	[JsonProperty(PropertyName = "CodeMeaning;")]
	public Enum_CodeMeaning m_enmCodeMeaning;

	[XmlAttribute("Variante")]
	[JsonProperty(PropertyName = "Variante")]
	public string m_strVariant;

	[XmlAttribute("LsgNr")]
	[JsonProperty(PropertyName = "CodereaderNumber")]
	public int m_i32CodereaderNumber;

	public void SUB_Init()
	{
		m_strValue = string.Empty;
		m_enmCodeMeaning = Enum_CodeMeaning.NichtDefiniert;
		m_strVariant = string.Empty;
		m_i32CodereaderNumber = 0;
	}

	public static Struct_Identifier[] Fun_sttFieldInitialized(int i_i32ElementsNumber)
	{
		if (i_i32ElementsNumber < 1)
		{
			return null;
		}
		checked
		{
			Struct_Identifier[] array = new Struct_Identifier[i_i32ElementsNumber - 1 + 1];
			int lowerBound = array.GetLowerBound(0);
			for (int upperBound = array.GetUpperBound(0); lowerBound <= upperBound; lowerBound++)
			{
				array[lowerBound] = default(Struct_Identifier);
				array[lowerBound].SUB_Init();
			}
			return array;
		}
	}

	public static bool Fun_blnCheckBox(Edc_Identifier[] ia_sttIdentifier)
	{
		if ((ia_sttIdentifier == null || ia_sttIdentifier.GetLength(0) <= 0) ? true : false)
		{
			return false;
		}
		return true;
	}
}
