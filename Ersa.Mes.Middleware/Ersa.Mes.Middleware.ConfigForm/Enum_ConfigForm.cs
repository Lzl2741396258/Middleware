using System.ComponentModel;

namespace Ersa.Mes.Middleware.ConfigForm;

public enum Enum_ConfigForm
{
	[Description("基础设置")]
	Basic,
	[Description("Mes接口")]
	Interface,
	[Description("Ersasoft路径")]
	Path,
	[Description("Mes Function")]
	MesFunction,
	[Description("Mes Task")]
	MesTask,
	[Description("Device")]
	Device,
	[Description("Process")]
	Process,
	[Description("Initialize")]
	Initialize,
	[Description("Database")]
	Database,
	[Description("PLC")]
	PLC,
	[Description("MES")]
	MES
}
