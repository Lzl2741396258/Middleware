using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.Message;

public enum Enum_MessageState
{
	[DescriptionChinese("没有定义")]
	[DescriptionEnglish("NotDefined")]
	[DescriptionGerman("NichtDefiniert")]
	NotDefined,
	[DescriptionChinese("正在发生")]
	[DescriptionEnglish("Occurred")]
	[DescriptionGerman("Aufgetreten")]
	Aufgetreten,
	[DescriptionChinese("正在确认")]
	[DescriptionEnglish("Acknowledged")]
	[DescriptionGerman("Quittiert")]
	Quittiert,
	[DescriptionChinese("退后一步")]
	[DescriptionEnglish("Set Back")]
	[DescriptionGerman("Zurueckgesetzt")]
	Zurueckgesetzt,
	[DescriptionChinese("备份")]
	[DescriptionEnglish("Backed Up")]
	[DescriptionGerman("Zurueckgestellt")]
	Zurueckgestellt
}
