using Ersa.Mes.Common.Extensions;

namespace Ersa.Mes.FileSystem.Model.Protocol;

public enum Enum_ProtocolSelectiveXmlDefault
{
	[Edc_MesFunctionEnglish("Serial board number")]
	i32SerialBoardNumber,
	[Edc_MesFunctionEnglish("Code")]
	strCode,
	[Edc_MesFunctionEnglish("Library")]
	strLibrary,
	[Edc_MesFunctionEnglish("Program")]
	strProgram,
	[Edc_MesFunctionEnglish("Error")]
	i32Error,
	[Edc_MesFunctionEnglish("Track-Number")]
	i32TrackNumber,
	[Edc_MesFunctionEnglish("Running in moment")]
	dtmRunningInMoment,
	[Edc_MesFunctionEnglish("Running out moment")]
	dtmRunningOutMoment,
	[Edc_MesFunctionEnglish("代码含义")]
	代码
}
