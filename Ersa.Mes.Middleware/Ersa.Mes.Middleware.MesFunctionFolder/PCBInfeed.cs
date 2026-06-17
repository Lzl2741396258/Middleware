using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Factory;

namespace Ersa.Mes.Middleware.MesFunctionFolder;

public class PCBInfeed : Edc_MesFunction
{
	public Request_PCBInfeed m_Request;

	protected override string m_strFunctionName => "PCBInfeed";

	public override bool m_blnResponse => false;

	public byte m_bytTrack { get; set; } = 1;


	public string m_strCodes { get; set; } = string.Empty;


	public DateTime m_dtmInfeedTime { get; set; } = DateTime.Now;


	public PCBInfeed(List<Edc_ConfigMesFunction> i_lstMesFunction, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		Edc_ConfigMesFunction function = i_lstMesFunction.Where((Edc_ConfigMesFunction s) => s.Pro_strName.Equals(m_strFunctionName)).FirstOrDefault();
		if (function != null)
		{
			base.m_blnActivePlatform = function.Pro_blnActivePlatform;
			base.m_blnActiveDatabase = function.Pro_blnActiveDatabase;
			base.m_blnActiveLocalFile = function.Pro_blnActiveLocalFile;
		}
		i_edcLogger.Debug($"Assmebly MES Function {m_strFunctionName}   Platform:{base.m_blnActivePlatform}  Database:{base.m_blnActiveDatabase}  LocalFile:{base.m_blnActiveLocalFile}", null, ".ctor", 62);
	}

	public override bool Fun_blnContains()
	{
		return base.m_strRequest.IndexOf(Enum_MesFunction.PcbEingelaufen_Anfo_EL.Fun_strGetDescription(), StringComparison.OrdinalIgnoreCase) > 0;
	}

	public override void Sub_GetRequest()
	{
		m_Request = base.m_strRequest.Fun_DeserializeContent<Request_PCBInfeed>();
		m_dtmInfeedTime = m_Request.m_sttHeader.m_dtmTimestamp.ToLocalTime();
		m_bytTrack = m_Request.m_sttHeader.m_bytTrackNumber;
		m_strCodes = Fun_strGetCode();
	}

	public override async Task<string> Fun_strExecute()
	{
		try
		{
			Sub_GetRequest();
			if (base.m_blnActivePlatform)
			{
				base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + ".Fun_blnConnectMesPlatform()...", null, "Fun_strExecute", 101);
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
			Sub_ShowMessage();
			base.m_i32ActCount++;
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			OnShowMessage(Enum_LogType.Error, "Ersasoft -> 'PCB Infeed' Error...Details:'" + ex.Message + "'");
		}
		return string.Empty;
	}

	protected virtual void Sub_ShowMessage()
	{
		OnShowMessage(Enum_LogType.Info, $"Ersasoft -> 'PCB Infeed'...Time:'{m_dtmInfeedTime}'  Code:'{m_strCodes}'");
	}

	private string Fun_strGetCode()
	{
		string a_strResult = string.Empty;
		Edc_Identifier[] a_sttIdentifier = m_Request.ma_sttIdentifier;
		if (a_sttIdentifier == null || a_sttIdentifier.Length == 0)
		{
			return a_strResult;
		}
		Edc_Identifier[] array = a_sttIdentifier;
		foreach (Edc_Identifier item in array)
		{
			a_strResult += item.m_strValue;
		}
		return a_strResult.TrimEnd(',');
	}
}
