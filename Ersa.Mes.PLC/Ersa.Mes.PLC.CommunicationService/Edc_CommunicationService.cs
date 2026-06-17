using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Logging;
using Ersa.Mes.PLC.Interfaces;
using Ersa.Mes.PLC.Model;

namespace Ersa.Mes.PLC.CommunicationService;

public class Edc_CommunicationService : Inf_CommunicationService, IDisposable
{
	private Inf_PLCProvider m_edcSpsProvider;

	public Inf_Logger m_Log;

	private Inf_PLC m_edcSpsService;

	private readonly object m_objLockConnection = new object();

	private readonly object m_objLockReading = new object();

	private readonly object m_objLockWrite = new object();

	private readonly Dictionary<Edc_PLCElement, Action<Edc_PLCElement>> m_dicReadOperation;

	private readonly Dictionary<Edc_PLCElement, Action<Edc_PLCElement>> m_dicWriteOperation;

	private readonly Dictionary<Edc_PLCElement, IDisposable> m_dicEventHandle;

	public readonly Dictionary<string, IEnumerable<Edc_PLCElement>> m_dicGroupParameter = new Dictionary<string, IEnumerable<Edc_PLCElement>>();

	private Inf_PLC Pro_edcSpsService => m_edcSpsService ?? (m_edcSpsService = m_edcSpsProvider.Fun_edcActiveSps());

	public Edc_CommunicationService(Inf_PLCProvider i_edcSpsProvider)
	{
		m_edcSpsProvider = i_edcSpsProvider;
		m_dicReadOperation = new Dictionary<Edc_PLCElement, Action<Edc_PLCElement>>();
		m_Log = new Edc_Logger(Enum_LogLevels.All);
	}

	public async Task Fun_fdcConnect(bool i_blnOnline)
	{
		try
		{
			m_dicGroupParameter.Clear();
			await Pro_edcSpsService.Fun_ConnectAsync(i_blnOnline, "127.0.0.1").ConfigureAwait(continueOnCapturedContext: true);
			lock (m_objLockConnection)
			{
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			m_Log.Error("Error connecting to the PLC...Fun_fdcConnect()  " + ex.Message, null, "Fun_fdcConnect", 91);
		}
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public Task Fun_fdcCreateVariableToGroup(IEnumerable<Edc_PLCElement> i_lstElement, string i_strGroupName, int i_i32CycleTime = 100)
	{
		List<Edc_PLCElement> a_lstSpsElement = i_lstElement.ToList();
		List<string> list2 = new List<string>();
		foreach (Edc_PLCElement item2 in a_lstSpsElement)
		{
			if (string.IsNullOrEmpty(item2.Pro_strAddress))
			{
			}
			list2.Add(item2.Pro_strAddress);
		}
		if (!m_dicGroupParameter.ContainsKey(i_strGroupName))
		{
			m_dicGroupParameter.Add(i_strGroupName, a_lstSpsElement);
		}
		else
		{
			List<Edc_PLCElement> list3 = m_dicGroupParameter[i_strGroupName].ToList();
			foreach (Edc_PLCElement item in list3)
			{
				if (!list3.Contains(item))
				{
					list3.Add(item);
				}
			}
		}
		return Pro_edcSpsService.Fun_fdcGroupCreateVariableAsync(list2, i_strGroupName, i_i32CycleTime);
	}

	private bool Fun_blnEstablishedConnection()
	{
		return true;
	}

	public void Sub_ReadValue(Edc_PLCElement i_edcSpsElement)
	{
		if (!Fun_blnEstablishedConnection())
		{
			return;
		}
		try
		{
			Action<Edc_PLCElement> value;
			lock (m_objLockReading)
			{
				if (!m_dicReadOperation.TryGetValue(i_edcSpsElement, out value))
				{
					Action<Edc_PLCElement> action = delegate(Edc_PLCElement s)
					{
						s.Pro_objValue = s.Pro_objValue;
					};
					m_dicReadOperation.Add(i_edcSpsElement, action);
					value = action;
				}
			}
			value(i_edcSpsElement);
		}
		catch (Exception ex)
		{
			string a_strMessage = "Error reading value from the PLC...Variable:'" + i_edcSpsElement?.Pro_strAddress + "'  Value:'" + i_edcSpsElement?.Pro_objValue + "'  Detail:'" + ex.Message + "'";
			m_Log.Error(a_strMessage, null, "Sub_ReadValue", 175);
		}
	}

	public Task Fun_fdcReadValueAsync(IEnumerable<Edc_PLCElement> i_lstSpsElement, CancellationToken i_fdcCancellationToken)
	{
		return Task.Run(delegate
		{
			foreach (Edc_PLCElement current in i_lstSpsElement)
			{
				i_fdcCancellationToken.ThrowIfCancellationRequested();
				Sub_ReadValue(current);
			}
		}, i_fdcCancellationToken);
	}

	public Task<string> Fun_fdcReadValueAsync(string i_strTaskName, string i_strPVariable, string i_strVariableName)
	{
		throw new NotImplementedException();
	}
}
