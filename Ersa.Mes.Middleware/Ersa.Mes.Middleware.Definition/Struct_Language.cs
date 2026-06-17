using System.Xml.Serialization;

namespace Ersa.Mes.Middleware.Definition;

public struct Struct_Language
{
	[XmlElement("Type")]
	public int i32Type;

	[XmlElement("Index")]
	public int i32Index;

	[XmlElement("Deutsch")]
	public string strDeutsch;

	[XmlElement("Englisch")]
	public string strEnglisch;

	[XmlElement("Finnisch")]
	public string strFinnisch;

	[XmlElement("Franzoesisch")]
	public string strFranzoesisch;

	[XmlElement("Japanisch")]
	public string strJapanisch;

	[XmlElement("Polnisch")]
	public string strPolnisch;

	[XmlElement("Rumaenisch")]
	public string strRumaenisch;

	[XmlElement("Tschechisch")]
	public string strTschechisch;

	[XmlElement("Ungarisch")]
	public string strUngarisch;

	[XmlElement("Spanisch")]
	public string strSpanisch;

	[XmlElement("Chinesisch")]
	public string strChinesich;

	[XmlElement("Schwedisch")]
	public string strSchwedisch;

	[XmlElement("Portugiesisch")]
	public string strPortugiesisch;

	[XmlElement("Italienisch")]
	public string strItalienisch;

	[XmlElement("Russisch")]
	public string strRussisch;

	[XmlElement("Koreanisch")]
	public string strKoreanisch;

	[XmlElement("Serbisch")]
	public string strSerbisch;
}
