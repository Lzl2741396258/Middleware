using System;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware;

[Obsolete("This class is Obsolete, use class HW_PLC instead")]
public class MT_GetPlcBr : Edc_MesTask
{
	public enum Enum_ConnectType
	{
		Default,
		ARsim,
		PLC
	}

	private Enum_ConnectType m_ConnectType = Enum_ConnectType.Default;

	public MT_GetPlcBr(Inf_MesTaskAttributes i_edcMesTask, Inf_Logger i_edcLogger)
		: base(i_edcMesTask, i_edcLogger)
	{
		try
		{
			m_ConnectType = Enum_ConnectType.PLC;
		}
		catch (Exception)
		{
		}
	}

	public override Task Sub_Act()
	{
		return null;
	}
}
