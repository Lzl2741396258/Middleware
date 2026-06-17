using System.ComponentModel;

namespace Ersa.Mes.Middleware.ConfigForm;

public enum Enum_SignalType
{
	[Description("默认")]
	Default,
	[Description("串口")]
	SerialPort,
	[Description("网口")]
	NetworkPort
}
