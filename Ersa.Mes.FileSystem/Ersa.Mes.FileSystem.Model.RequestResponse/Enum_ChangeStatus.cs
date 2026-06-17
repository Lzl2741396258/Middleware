using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public enum Enum_ChangeStatus : byte
{
	[DescriptionChinese("没有定义")]
	[DescriptionEnglish("Not defined")]
	[DescriptionGerman("NichtDefiniert")]
	NichtDefiniert,
	[DescriptionChinese("有改变")]
	[DescriptionEnglish("Changed")]
	[DescriptionGerman("Geaendert")]
	Geaendert,
	[DescriptionChinese("没有改变")]
	[DescriptionEnglish("Not Changed")]
	[DescriptionGerman("NichtGeaendert")]
	NichtGeaendert,
	[DescriptionChinese("外部信号")]
	[DescriptionEnglish("Extend Request")]
	[DescriptionGerman("AnfoExtern")]
	AnfoExtern,
	[DescriptionChinese("改装清空")]
	[DescriptionEnglish("Retrofitting Emptied")]
	[DescriptionGerman("UmruestenLeergefahren")]
	UmruestenLeergefahren,
	[DescriptionChinese("下一台机器转换信息")]
	[DescriptionEnglish("Conversion Info Next machine")]
	[DescriptionGerman("UmruestenInfoNaechsteMaschine")]
	UmruestenInfoNaechsteMaschine
}
