using System.ComponentModel;
using Ersa.Mes.Common.Description;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public enum Enum_ProtocolElementReflow
{
	[DescriptionEnglish("Time")]
	[DescriptionChinese("时间")]
	[Description("stfDate")]
	Time,
	[DescriptionEnglish("timestamp Soldering program")]
	[DescriptionChinese("时间戳 焊接程 式")]
	[Description("stfZeitstempel")]
	TimeStamp,
	[DescriptionEnglish("User name")]
	[DescriptionChinese("用户名")]
	[Description("UserId")]
	UserName,
	[DescriptionEnglish("Track")]
	[DescriptionChinese("追踪")]
	[Description("sntConveyor")]
	Track,
	[DescriptionEnglish("Board number")]
	[DescriptionChinese("板子号码")]
	[Description("Panel No.")]
	BoardNumber,
	[DescriptionEnglish("Soldering program")]
	[DescriptionChinese("焊接程 式")]
	[Description("stfProg")]
	SolderingProgram,
	[DescriptionEnglish("Library")]
	[DescriptionChinese("程序库")]
	[Description("stfBib")]
	Library,
	[DescriptionEnglish("Productname")]
	[DescriptionChinese("产品名称")]
	[Description("stfProdukt")]
	ProductName,
	[DescriptionEnglish("Code")]
	[DescriptionChinese("代码")]
	[Description("stfBarcode")]
	Code,
	[DescriptionEnglish("Mode (0- MANUAL, 1- AUTO)")]
	[DescriptionChinese("模式")]
	[Description("sntMode")]
	Mode,
	[DescriptionEnglish("Alarms")]
	[DescriptionChinese("报警")]
	[Description("sntError")]
	Alarms,
	[DescriptionEnglish("nitrogen switch")]
	[DescriptionChinese("氮气开关")]
	[Description("sntN2")]
	N2Switch,
	[DescriptionEnglish("Process time")]
	[DescriptionChinese("处理时间")]
	[Description("dwdProcessTime")]
	CycleTime
}
