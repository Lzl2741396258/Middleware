using System;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.Model;

namespace MesXPT.XPT_MesFunction;

public class XPT_MachineCondition : MachineCondition
{
	private XPT_Config m_Config { get; set; }

	public XPT_MachineCondition(XPT_Config i_Config, Inf_Logger i_edcLogger)
		: base(i_Config.m_lstMesFunction, i_edcLogger)
	{
		m_Config = i_Config;
	}

	protected override Task Fun_blnConnectMesPlatform()
	{
		try
		{
			Request_MachineCondition request = base.m_Request;
			Enum_OEECode a_enuOeeCode = base.m_Request.m_sttOeeCode.m_enuMDECode;
			XPT_Data.m_enuOeeState = a_enuOeeCode;
		}
		catch (Exception ex)
		{
			base.m_edcLogger.Error(ex.Message + "  " + MethodBase.GetCurrentMethod().Name, null, "Fun_blnConnectMesPlatform", 46);
		}
		return Task.CompletedTask;
	}
}
