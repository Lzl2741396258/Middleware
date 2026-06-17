using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.Message;

public enum Enum_OperatingMode
{
	[DescriptionChinese("电气关闭")]
	[DescriptionEnglish("Electornice Off")]
	[DescriptionGerman("ElektronikAus")]
	ElektronikAus = 0,
	[DescriptionChinese("电气初始化")]
	[DescriptionEnglish("Electornice Init")]
	[DescriptionGerman("ElektronikInit")]
	ElektronikInit = 1,
	[DescriptionChinese("电源关闭")]
	[DescriptionEnglish("Power off")]
	[DescriptionGerman("LeistungAus")]
	LeistungAus = 2,
	[DescriptionChinese("阶段OK")]
	[DescriptionEnglish("Phase ok")]
	[DescriptionGerman("LeistungAus")]
	PhaseOk = 3,
	[DescriptionChinese("检查紧急停止")]
	[DescriptionEnglish("Check Emergency Stop")]
	[DescriptionGerman("PruefeNotAus")]
	PruefeNotAus = 4,
	[DescriptionChinese("释放开")]
	[DescriptionEnglish("Release On")]
	[DescriptionGerman("FreigabeAus")]
	FreigabeAus = 5,
	[DescriptionChinese("释放关")]
	[DescriptionEnglish("Release Off")]
	[DescriptionGerman("FreigabeEin")]
	FreigabeEin = 6,
	[DescriptionChinese("设备初始化")]
	[DescriptionEnglish("Machine Init")]
	[DescriptionGerman("MaschinenInit")]
	MaschinenInit = 7,
	[DescriptionChinese("建立")]
	[DescriptionEnglish("Set up")]
	[DescriptionGerman("Einrichten")]
	Einrichten = 8,
	[DescriptionChinese("自动")]
	[DescriptionEnglish("automatic")]
	[DescriptionGerman("Automatik")]
	Automatik = 9,
	[DescriptionChinese("离线")]
	[DescriptionEnglish("Offline")]
	[DescriptionGerman("Offline")]
	Offline = 10,
	[DescriptionChinese("没有定义")]
	[DescriptionEnglish("Not Defined")]
	[DescriptionGerman("NotDefined")]
	NotDefined = 255
}
