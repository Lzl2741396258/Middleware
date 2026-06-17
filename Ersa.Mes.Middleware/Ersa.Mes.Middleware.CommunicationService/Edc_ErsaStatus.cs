using System;

namespace Ersa.Mes.Middleware.CommunicationService;

public class Edc_ErsaStatus
{
	public static Enum_ErsaStatus Fun_enuGetCurrentStatus(Enum_ErsaStatus i_enuPreviouStatus, int i_i32PreviouMode, int i_i32Mode, int i_i32Engineer = 0, int i_i32PlanedDown = 0, int i_i32PcbCount = 0)
	{
		switch (i_enuPreviouStatus)
		{
		case Enum_ErsaStatus.Default:
			return Enum_ErsaStatus.Idle;
		case Enum_ErsaStatus.Running:
			if (i_i32PcbCount == 0)
			{
				return Enum_ErsaStatus.Idle;
			}
			return Enum_ErsaStatus.Running;
		case Enum_ErsaStatus.Idle:
			if (i_i32Engineer == 1)
			{
				return Enum_ErsaStatus.Engineer;
			}
			if (i_i32PlanedDown == 1)
			{
				return Enum_ErsaStatus.PlanedDown;
			}
			if (i_i32PcbCount >= 1)
			{
				return Enum_ErsaStatus.Running;
			}
			return Enum_ErsaStatus.Idle;
		case Enum_ErsaStatus.Engineer:
			if (i_i32Engineer == 0)
			{
				return Enum_ErsaStatus.Idle;
			}
			if (i_i32PcbCount >= 1)
			{
				return Enum_ErsaStatus.Idle;
			}
			return Enum_ErsaStatus.Engineer;
		case Enum_ErsaStatus.PlanedDown:
			if (i_i32PlanedDown == 0)
			{
				return Enum_ErsaStatus.Idle;
			}
			if (i_i32PcbCount >= 1)
			{
				return Enum_ErsaStatus.Idle;
			}
			return Enum_ErsaStatus.PlanedDown;
		case Enum_ErsaStatus.Error:
			return Enum_ErsaStatus.Error;
		default:
			throw new Exception("状态机异常");
		}
	}
}
