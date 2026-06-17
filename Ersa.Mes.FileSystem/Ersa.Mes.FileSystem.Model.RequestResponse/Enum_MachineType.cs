using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public enum Enum_MachineType
{
	[DescriptionChinese("选择焊")]
	[DescriptionEnglish("Selective")]
	[DescriptionGerman("Selektiv")]
	Selektiv,
	[DescriptionChinese("回流炉")]
	[DescriptionEnglish("Reflow")]
	[DescriptionGerman("Reflow")]
	Reflow,
	[DescriptionChinese("波峰焊")]
	[DescriptionEnglish("Wave")]
	[DescriptionGerman("Welle")]
	Welle,
	[DescriptionChinese("EcoSelect")]
	[DescriptionEnglish("EcoSelect")]
	[DescriptionGerman("EcoSelect")]
	EcoSelect2
}
