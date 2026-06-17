using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.Initialize;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Factory;
using Ersa.Mes.PLC;
using Ersa.Mes.PLC.Model;
using MesXPT.Model;

namespace MesXPT.XPT_MesPLC;

public class XPT_PLC : Edc_MesTask, IDisposable
{
	private Inf_PLC m_edcPLCService;

	private CancellationTokenSource cts = new CancellationTokenSource();

	private readonly Dictionary<string, TaskCompletionSource<List<Edc_PLCElement>>> m_dicPLCElement = new Dictionary<string, TaskCompletionSource<List<Edc_PLCElement>>>();

	private const string mC_strDirectoryConfig = "Configuration";

	private const string mC_strConfigPLCFileName = "Initialize_PLC.xml";

	private XPT_Config m_Config { get; }

	private int m_i32ActiveTimes { get; set; } = 0;


	public XPT_PLC(XPT_Config i_Config, Edc_MesTaskAttributes a_MesAttributes2, Inf_Logger i_edcLogger)
		: base(i_Config.ma_MesTask.Where((Edc_MesTaskAttributes s) => s.Pro_strName.Equals(Enum_TaskName.PLC.ToString())).FirstOrDefault(), i_edcLogger)
	{
		base.m_edcLogger = i_edcLogger;
		m_Config = i_Config;
		Sub_LoadPLCParameter();
		Fun_edcConnectPvi();
		Task.Run(delegate
		{
			Sub_GetPLCValue();
		});
	}

	public void Dispose()
	{
	}

	public override Task Sub_Act()
	{
		return Task.CompletedTask;
	}

	private async Task Fun_edcConnectPvi()
	{
		m_edcPLCService = new Edc_BrPlc(base.m_edcLogger);
		await m_edcPLCService.Fun_ConnectAsync(i_blnOnline: true, m_Config.m_sttPLC.m_strIP).ConfigureAwait(continueOnCapturedContext: true);
		OnShowMessage(Enum_LogType.Info, "Successfully connected to PLC....");
	}

	private async void Sub_GetPLCValue()
	{
		while (true)
		{
			try
			{
				if (cts.IsCancellationRequested)
				{
					break;
				}
				Thread.Sleep(base.Pro_i32Interval);
				foreach (KeyValuePair<string, TaskCompletionSource<List<Edc_PLCElement>>> item in m_dicPLCElement)
				{
					List<Edc_PLCElement> a_dicResult = await m_edcPLCService.Sub_lstReadValue(item.Key);
					XPT_Data.m_dicPLCData[item.Key] = a_dicResult;
				}
				OnShowPlcCount(++m_i32ActiveTimes);
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				OnShowMessage(Enum_LogType.Error, "Error connecting to the PLC...Details:'" + ex.Message + "'");
			}
		}
	}

	private void Sub_LoadPLCParameter()
	{
		try
		{
			string a_strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Initialize_PLC.xml");
			Edc_InitializePLC initializePLC = a_strPath.Fun_edcDeserializeByFilePath<Edc_InitializePLC>();
			foreach (Edc_ParameterPLC para in initializePLC.m_lstParameterPLC.ToList())
			{
				string key = para.m_strTask + "." + para.m_strPVariable;
				Edc_PLCElement edc_PLCElement = new Edc_PLCElement();
				edc_PLCElement.Pro_strGroupName = key;
				edc_PLCElement.Pro_strTask = para.m_strTask;
				edc_PLCElement.Pro_strPVariable = para.m_strPVariable;
				edc_PLCElement.Pro_strStructMember = para.m_strStructMember;
				edc_PLCElement.Pro_strAddress = para.m_strTask + "." + para.m_strPVariable + "." + para.m_strStructMember;
				edc_PLCElement.Pro_objValue = para.m_strValue;
				Edc_PLCElement spsElement = edc_PLCElement;
				m_dicPLCElement.TryGetValue(key, out var a_dicPVariable);
				if (a_dicPVariable == null)
				{
					TaskCompletionSource<List<Edc_PLCElement>> tcs = new TaskCompletionSource<List<Edc_PLCElement>>();
					tcs.SetResult(new List<Edc_PLCElement> { spsElement });
					m_dicPLCElement.Add(key, tcs);
				}
			}
		}
		catch (Exception ex)
		{
			string a_strMessage = "Damaged file Initialize_PLC.xml";
			base.m_edcLogger.Error(a_strMessage + "  Details:" + ex.Message, null, "Sub_LoadPLCParameter", 167);
			throw;
		}
	}
}
