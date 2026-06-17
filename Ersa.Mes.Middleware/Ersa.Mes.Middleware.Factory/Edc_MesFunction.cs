using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Interface;

namespace Ersa.Mes.Middleware.Factory;

public abstract class Edc_MesFunction
{
	public CancellationTokenSource m_cts = new CancellationTokenSource();

	protected abstract string m_strFunctionName { get; }

	public bool m_blnActive { get; set; }

	protected string m_strCurrentClassName { get; set; }

	public bool m_blnInitiative { get; set; } = false;


	public abstract bool m_blnResponse { get; }

	protected Inf_Logger m_edcLogger { get; }

	protected Inf_MesFunctionContainer m_edcMesFunctionContainer { get; private set; }

	public string m_strRequest { get; set; }

	public int m_i32ActCount { get; set; }

	protected bool m_blnActivePlatform { get; set; } = true;


	protected bool m_blnActiveDatabase { get; set; } = false;


	protected bool m_blnActiveLocalFile { get; set; } = false;


	public event Evt_ShowMessageEventHandler Evt_ShowMessage;

	public event Evt_ShowOeeEventHandler Evt_ShowOee;

	public event Evt_ShowErsasoftEventHandler Evt_ShowErsasoftCount;

	private event Evt_SendRequestEventHandle m_evtSendRequest;

	public event Evt_SendRequestEventHandle Evt_SendRequest
	{
		add
		{
			m_evtSendRequest += value;
		}
		remove
		{
			m_evtSendRequest -= value;
		}
	}

	private event Action<string> m_evtUserChanged;

	public event Action<string> Evt_UserChanged
	{
		add
		{
			m_evtUserChanged += value;
		}
		remove
		{
			m_evtUserChanged -= value;
		}
	}

	protected virtual void OnShowMessage(Enum_LogType i_enuLogType, string i_strMessage)
	{
		this.Evt_ShowMessage?.Invoke(i_enuLogType, i_strMessage);
	}

	protected virtual void OnShowOee(string i_strOeeCode, string i_strOeeText, int i_i32PcbInMachine = 0)
	{
		this.Evt_ShowOee?.Invoke(i_strOeeCode, i_strOeeText, i_i32PcbInMachine);
	}

	protected virtual void OnShowProcess(int i_strCount)
	{
		this.Evt_ShowErsasoftCount?.Invoke(i_strCount);
	}

	protected virtual void OnSendRequest(string i_strMessage)
	{
		this.m_evtSendRequest?.Invoke(i_strMessage);
		m_edcLogger.Debug("Send Request  " + i_strMessage, null, "OnSendRequest", 94);
	}

	protected virtual void OnUserChanged(string i_strUserName)
	{
		this.m_evtUserChanged?.Invoke(i_strUserName);
		OnShowMessage(Enum_LogType.Info, "Change to User '" + i_strUserName + "'");
	}

	public Edc_MesFunction(Inf_Logger i_edcLogger)
	{
		m_edcLogger = i_edcLogger;
	}

	public virtual bool Fun_blnContains()
	{
		return false;
	}

	public virtual void Sub_GetRequest()
	{
	}

	public virtual Task<string> Fun_strExecute()
	{
		return Task.FromResult(string.Empty);
	}

	protected virtual Task Fun_blnConnectMesPlatform()
	{
		return Task.CompletedTask;
	}

	protected virtual Task Fun_blnConnectMesPlatformWait()
	{
		return Task.FromResult(result: true);
	}

	protected virtual void Sub_AddToDatabase()
	{
	}

	protected virtual void Sub_WriteToLocalFile(string i_strMesFunctionName)
	{
		try
		{
			string a_strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", i_strMesFunctionName);
			string a_strFilename = i_strMesFunctionName + "_" + Guid.NewGuid().ToString();
			DirectoryFilesHelper.Sub_WriteToTxt(a_strPath, a_strFilename, m_strRequest);
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, MethodBase.GetCurrentMethod().Name + " error...Details:'" + ex.Message + "'");
		}
	}

	public string Fun_strGetFunctionName()
	{
		return m_strFunctionName ?? string.Empty;
	}
	//≤‚ ‘
    public virtual void Fun_Test()
    {
        
    }
}
