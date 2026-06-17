using System.ComponentModel;

namespace Ersa.Mes.Database.Model;

public enum Enum_MachineManagement
{
	[Description("Zeit")]
	Betriebszeit,
	[Description("DatenEditInt64")]
	Loegutzaehler,
	[Description("DatenInt64")]
	Wegstreckenzaehler,
	[Description("DatenReal")]
	Energiezaehler,
	[Description("DatenReal")]
	Stickstoffmenge,
	[Description("DatenReal")]
	Flussmittel,
	[Description("DatenInt64")]
	TiegelWerte
}
