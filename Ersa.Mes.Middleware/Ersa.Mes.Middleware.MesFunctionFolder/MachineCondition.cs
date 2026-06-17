using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.Database.Model.EF_Ersasoft4;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class MachineCondition : Edc_MesFunction
{
	public Dictionary<uint, string> m_dicOeeDecription = new Dictionary<uint, string>();

	protected override string m_strFunctionName => "MachineCondition";

	public override bool m_blnResponse => false;

	public Request_MachineCondition m_Request { get; set; }

	public uint m_i32OeeCode { get; set; }

	public Enum_OEECode m_enuOeeCode { get; set; }

	public string m_strOeeText { get; set; }

	public MachineCondition(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		Sub_InitializeDecription();
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 70);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.Maschinenzustand_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_MachineCondition>();
		Sub_GetOEECode();
	}

	public override Task<string> Fun_strExecute()
	{
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 96);
				Task.Run((Func<Task>)Fun_blnConnectMesPlatform);
			}
			if (base.m_blnActiveDatabase)
			{
				Task.Run((Action)Sub_AddToDatabase);
			}
			if (base.m_blnActiveLocalFile)
			{
				Task.Run(delegate
				{
					Sub_WriteToLocalFile(MethodBase.GetCurrentMethod().DeclaringType.Name);
				});
			}
			OnShowOee(m_i32OeeCode.ToString(), m_strOeeText, m_Request.m_sttSolderingInMachine.m_i32Number);
			base.m_i32ActCount++;
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "MES Function 'Machine Condition' Error...Details:'" + ex.Message + "'");
		}
		return Task.FromResult(string.Empty);
	}

	protected override void Sub_AddToDatabase()
	{
		try
		{
			using (new Ersasoft4())
			{
			}
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, ex.Message);
		}
	}

	public void Sub_GetOEECode()
	{
		try
		{
			m_i32OeeCode = (uint)m_Request.m_sttOeeCode.m_enuMDECode;
			Enum.TryParse<Enum_OEECode>(m_i32OeeCode.ToString(), out var value);
			m_enuOeeCode = value;
			m_strOeeText = m_Request.m_strOeeText.ToString();
		}
		catch
		{
		}
	}

	public string Fun_strGetOeeDecription()
	{
		try
		{
			return m_dicOeeDecription[m_i32OeeCode].ToString();
		}
		catch
		{
			return "Please check the machine condition list...";
		}
	}

	private void Sub_InitializeDecription()
	{
		if (m_dicOeeDecription.Count == 0)
		{
			m_dicOeeDecription.Add(1100u, "Productive_Time : Machine is in‘automatic’ mode and no of the other states is active.");
			m_dicOeeDecription.Add(1200u, "Engineering_Time : Machine is in automatic mode with board simulation");
			m_dicOeeDecription.Add(1300u, "SB Standby Time : Machine is waiting for parts from downstream machine(or operator by manual feeding) or congestion at upstream machine / periphery.");
			m_dicOeeDecription.Add(2100u, "Scheduled_Downtime : At least one ‘cyclic’ message which is blocking machine infeed, Manual stop of infeed by operator or machine isswitched off by weekly timer function.");
			m_dicOeeDecription.Add(2210u, "Wait_Time : Machine is in ‘maintenance mode’  or at least one message of class ‘warning’, ‘service’ or ‘waiting’. ");
			m_dicOeeDecription.Add(2220u, "Repair_Time : At least one message of class ‘error’ or ‘danger’.");
		}
	}
}
