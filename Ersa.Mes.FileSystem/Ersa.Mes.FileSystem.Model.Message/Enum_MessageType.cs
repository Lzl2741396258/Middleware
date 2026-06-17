using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.Message;

public enum Enum_MessageType
{
	[DescriptionChinese("没有定义")]
	[DescriptionEnglish("NotDefined")]
	[DescriptionGerman("NichtDefiniert")]
	NichtDefiniert,
	[DescriptionChinese("危险")]
	[DescriptionEnglish("Danger")]
	[DescriptionGerman("Gefahr")]
	Gefahr,
	[DescriptionChinese("麻烦")]
	[DescriptionEnglish("Trouble")]
	[DescriptionGerman("Stoerung")]
	Stoerung,
	[DescriptionChinese("警报")]
	[DescriptionEnglish("Warning")]
	[DescriptionGerman("Warnung")]
	Warnung,
	[DescriptionChinese("服务的")]
	[DescriptionEnglish("Service")]
	[DescriptionGerman("Service")]
	Service,
	[DescriptionChinese("循环的")]
	[DescriptionEnglish("Cyclical")]
	[DescriptionGerman("Zyklisch")]
	Zyklisch,
	[DescriptionChinese("等待的")]
	[DescriptionEnglish("Waiting")]
	[DescriptionGerman("Warten")]
	Warten,
	[DescriptionChinese("记录的")]
	[DescriptionEnglish("Note")]
	[DescriptionGerman("Hinweis")]
	Hinweis,
	[DescriptionChinese("外部的")]
	[DescriptionEnglish("External")]
	[DescriptionGerman("Extern")]
	Extern
}
