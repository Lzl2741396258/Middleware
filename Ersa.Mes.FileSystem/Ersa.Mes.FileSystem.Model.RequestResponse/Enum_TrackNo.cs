using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public enum Enum_TrackNo
{
	[DescriptionGerman("Spur_ohneZuordnung")]
	[DescriptionEnglish("Track_withoutAssignment")]
	[DescriptionChinese("没有分配轨道")]
	Spur_ohneZuordnung = 0,
	[DescriptionGerman("Spur_1")]
	[DescriptionEnglish("Track_1")]
	[DescriptionChinese("轨道_1")]
	Spur_1 = 1,
	[DescriptionGerman("Spur_2")]
	[DescriptionEnglish("Track_2")]
	[DescriptionChinese("轨道_2")]
	Spur_2 = 2,
	[DescriptionGerman("Spur_3")]
	[DescriptionEnglish("Track_3")]
	[DescriptionChinese("轨道_3")]
	Spur_3 = 3,
	[DescriptionGerman("Spur_4")]
	[DescriptionEnglish("Track_4")]
	[DescriptionChinese("轨道_4")]
	Spur_4 = 4,
	[DescriptionGerman("NichtDefiniert")]
	[DescriptionEnglish("Not Defined")]
	[DescriptionChinese("没有定义")]
	NichtDefiniert = 255
}
