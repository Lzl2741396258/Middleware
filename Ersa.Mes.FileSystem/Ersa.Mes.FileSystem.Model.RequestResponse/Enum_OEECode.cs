using System.ComponentModel;
using System.Xml.Serialization;

namespace Ersa.Mes.FileSystem.Model.RequestResponse;

public enum Enum_OEECode
{
	[Description("没有定义")]
	[XmlEnum(Name = "0")]
	NichtDefiniert = 0,
	[XmlEnum(Name = "1000")]
	UP_Uptime = 1000,
	[XmlEnum(Name = "1100")]
	PR_Productive_Time_L3 = 1100,
	[XmlEnum(Name = "1110")]
	PR_Productive_Time_L4 = 1110,
	[XmlEnum(Name = "1111")]
	QA_Sampling = 1111,
	[XmlEnum(Name = "1112")]
	Rework_Retest = 1112,
	[XmlEnum(Name = "1113")]
	Notmal_Production = 1113,
	[XmlEnum(Name = "1200")]
	EN_Engineering_Time_L3 = 1200,
	[XmlEnum(Name = "1210")]
	EN_Engineering_Time_L4 = 1210,
	[XmlEnum(Name = "1211")]
	Process_Engineering = 1211,
	[XmlEnum(Name = "1300")]
	SB_Standby_Time_L3 = 1300,
	[XmlEnum(Name = "1310")]
	SB_Standby_Time_L4 = 1310,
	[XmlEnum(Name = "1311")]
	No_Operator = 1311,
	[XmlEnum(Name = "1312")]
	No_Material = 1312,
	[XmlEnum(Name = "2000")]
	DN_Downtime = 2000,
	[XmlEnum(Name = "2100")]
	SD_Scheduled_Downtime = 2100,
	[XmlEnum(Name = "2110")]
	SDM_Setup_Time = 2110,
	[XmlEnum(Name = "2111")]
	Waiting_for_Setup = 2111,
	[XmlEnum(Name = "2120")]
	SDT_Productive_Test_Time = 2120,
	[XmlEnum(Name = "2121")]
	Maintenance = 2121,
	[XmlEnum(Name = "2122")]
	Planned_Offline = 2122,
	[XmlEnum(Name = "2130")]
	SDS_Preventive_Maintenance_Time = 2130,
	[XmlEnum(Name = "2131")]
	EQ_Cleaning = 2131,
	[XmlEnum(Name = "2132")]
	Change_Lot_Setup = 2132,
	[XmlEnum(Name = "2200")]
	UD_Unscheduled_Downtime = 2200,
	[XmlEnum(Name = "2210")]
	DUW_Wait_Time = 2210,
	[XmlEnum(Name = "2211")]
	Waiting_for_Maintenance = 2211,
	[XmlEnum(Name = "2212")]
	Waiting_for_Operator = 2212,
	[XmlEnum(Name = "2213")]
	Waiting_for_Spares = 2213,
	[XmlEnum(Name = "2214")]
	Waiting_for_Tech = 2214,
	[XmlEnum(Name = "2220")]
	UDR_Repair_Time = 2220,
	[XmlEnum(Name = "2221")]
	Operator_Error = 2221,
	[XmlEnum(Name = "2222")]
	Utility_Problem = 2222,
	[XmlEnum(Name = "2223")]
	Process_Problem = 2223,
	[XmlEnum(Name = "2224")]
	Machine_Failure = 2224
}
