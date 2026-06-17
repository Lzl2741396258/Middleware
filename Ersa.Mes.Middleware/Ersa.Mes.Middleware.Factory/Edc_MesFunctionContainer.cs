using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Interface;
using Ersa.Mes.Middleware.MesFunctionFolder;

namespace Ersa.Mes.Middleware.Factory;

public abstract class Edc_MesFunctionContainer : Edc_MesTask, Inf_MesFunctionContainer
{
	public TcpClient m_TcpClient = new TcpClient();

	private bool m_blnTaskStopFlag = false;

	private TaskCompletionSource<bool> m_tcsCompletionConnected;

	private int m_i32ConnectError = -1;

	private Edc_ConfigBase m_edcConfigBase { get; set; }

	private IDictionary<Type, object> m_dicMesFunction { get; set; }

	public Edc_MesFunctionContainer(Edc_ConfigBase i_ConfigBase, Inf_Logger i_edcLogger, Inf_MesTaskAttributes i_edcMesTask)
		: base(i_edcMesTask, i_edcLogger)
	{
		m_edcConfigBase = i_ConfigBase;
		m_dicMesFunction = new Dictionary<Type, object>();
	}

	~Edc_MesFunctionContainer()
	{
		Sub_End();
	}

	public override void Sub_Load()
	{
		Sub_AddMesFunction();
		switch (m_edcConfigBase.m_clsBasicSettings.m_enuMachineType)
		{
		case Enum_MachineType.Selektiv:
			Sub_AddMesFunctionSelective();
			break;
		case Enum_MachineType.Reflow:
			Sub_AddMesFunctionReflow();
			break;
		case Enum_MachineType.Welle:
			Sub_AddMesFunctionWave();
			break;
		case Enum_MachineType.EcoSelect2:
			Sub_AddMesFunctionEcoSelect2();
			break;
		default:
			throw new Exception("Unrecognized machine types");
		}
		Sub_AddEvent();
	}

	protected virtual void Sub_AddMesFunction()
	{
	}

	protected virtual void Sub_AddMesFunctionReflow()
	{
	}

	protected virtual void Sub_AddMesFunctionSelective()
	{
	}

	protected virtual void Sub_AddMesFunctionWave()
	{
	}

	protected virtual void Sub_AddMesFunctionEcoSelect2()
	{
	}

	private void Sub_AddEvent()
	{
		foreach (KeyValuePair<Type, object> item in m_dicMesFunction)
		{
			Edc_MesFunction function = item.Value as Edc_MesFunction;
			function.Evt_ShowMessage += OnShowMessage;
			function.Evt_SendRequest += Sub_SendResponse;
			if (item.Key == typeof(TransferProcessParameter))
			{
				function.Evt_ShowErsasoftCount += OnShowErsasoftCount;
			}
			if (item.Key == typeof(MachineCondition))
			{
				function.Evt_ShowOee += OnShowOEE;
			}
		}
	}

	public override async Task Sub_Act()
	{
		try
		{
			if (!(await Fun_blnCheckConnectAsync(m_edcConfigBase.m_edcInterfaceAddress.Pro_strClientIP, m_edcConfigBase.m_edcInterfaceAddress.Pro_i32ClientPort)))
			{
				return;
			}
			NetworkStream a_Stream = Sub_GetStream(m_TcpClient.GetStream());
			if (!(a_Stream?.CanRead ?? false))
			{
				return;
			}
			Dictionary<int, string> a_dicRequest = new Dictionary<int, string>();
			
			int iResult = Fun_i32GetRequest(a_Stream, out a_dicRequest);
			if (iResult <= 0)
			{
				return;
			}
			_ = string.Empty;
			foreach (string request in a_dicRequest.Values)
			{
				// Get Function Name
				Edc_MesFunction function = Fun_edcFind(request);
				if (function == null)
				{
					continue;
				}
				base.m_edcLogger.Debug("Start " + function?.Fun_strGetFunctionName() + "  Request\r\n" + request, null, "Sub_Act", 195);
				Task<string> task = function.Fun_strExecute();
				string a_strResponse = task.Result;
				if (function.m_blnResponse)
				{
					if (string.IsNullOrEmpty(a_strResponse))
					{
						base.m_edcLogger.Debug("End " + function?.Fun_strGetFunctionName() + "  Response is empty", null, "Sub_Act", 206);
					}
					else
					{
						Sub_SendResponse(a_strResponse);
					}
					base.m_edcLogger.Debug("End " + function?.Fun_strGetFunctionName() + "  Response\r\n" + a_strResponse, null, "Sub_Act", 212);
				}
				else
				{
					base.m_edcLogger.Debug("End " + function?.Fun_strGetFunctionName() + " m_blnResponse is false", null, "Sub_Act", 216);
				}
			}
		}
		catch (ObjectDisposedException)
		{
			OnShowMessage(Enum_LogType.Info, "MES disconnected successfully...");
		}
		catch (Exception ex3)
		{
			Exception ex = ex3;
			string info = MethodBase.GetCurrentMethod().DeclaringType.Name + " Error...Details:'" + ex.Message + "'";
			base.m_edcLogger.Error(info, null, "Sub_Act", 229);
			OnShowMessage(Enum_LogType.Error, info);
		}
	}

	// Check Function Name with Message from TCP Client. (String.IndexOf)
	private Edc_MesFunction Fun_edcFind(string i_strRequest)
	{
		foreach (KeyValuePair<Type, object> item in m_dicMesFunction)
		{
			Edc_MesFunction function = item.Value as Edc_MesFunction;
			if (function != null)
			{
				function.m_strRequest = i_strRequest;
			}
			if (function.Fun_blnContains())
			{
				return function;
			}
		}
		return null;
	}

	private int Fun_i32GetRequest(NetworkStream i_Stream, out Dictionary<int, string> o_dicRequest)
	{
		o_dicRequest = new Dictionary<int, string>();
		try
		{
			if (i_Stream.DataAvailable)
			{
				int a_i32CurrentSize = 0;
				int i_32MessageLength = 0;
				byte[] a_bytBuffer = new byte[m_TcpClient.ReceiveBufferSize];
				byte[] a_bytTotal = new byte[0];
				Thread.Sleep(100);
				int a_i32ReadBytesNumber = i_Stream.Read(a_bytBuffer, 0, m_TcpClient.ReceiveBufferSize);
				if (a_i32ReadBytesNumber > 0)
				{
					byte[] temp1 = new byte[a_i32ReadBytesNumber];
					Array.Copy(a_bytBuffer, 0, temp1, 0, a_i32ReadBytesNumber);
					a_bytTotal = a_bytTotal.Concat(temp1).ToArray();
					a_i32CurrentSize += a_i32ReadBytesNumber;
					i_32MessageLength = RequestHelper.Fun_i32GetSize(a_bytTotal, m_edcConfigBase.m_clsBasicSettings.m_blnCRCAuthentication);
					while (i_Stream.DataAvailable || a_i32CurrentSize < i_32MessageLength)
					{
						a_i32ReadBytesNumber = i_Stream.Read(a_bytBuffer, 0, m_TcpClient.ReceiveBufferSize);
						if (a_i32ReadBytesNumber > 0)
						{
							byte[] temp2 = new byte[a_i32ReadBytesNumber];
							Array.Copy(a_bytBuffer, 0, temp2, 0, a_i32ReadBytesNumber);
							a_i32CurrentSize += a_i32ReadBytesNumber;
							a_bytTotal = a_bytTotal.Concat(temp2).ToArray();
						}
					}
					byte[] a_bytData = new byte[a_i32CurrentSize];
					Array.Copy(a_bytTotal, a_bytData, a_i32CurrentSize);
					string a_strRequest = (m_edcConfigBase.m_clsBasicSettings.m_blnCRCAuthentication ? RequestHelper.Fun_strExtractRequestMessage(a_bytData) : Encoding.UTF8.GetString(a_bytData));
					o_dicRequest = Fun_dicGetRequest(a_strRequest);
					a_strRequest = a_strRequest.TrimEnd(default(char));
					return o_dicRequest.Count;
				}
			}
		}
		catch (Exception ex)
		{
			OnShowMessage(Enum_LogType.Error, "Steam Read or CRC Convert Error...Message:'" + ex.Message + "'");
		}
		return 0;
	}

	private Dictionary<int, string> Fun_dicGetRequest(string i_strRequest)
	{
		Dictionary<int, string> dic = new Dictionary<int, string>();
		i_strRequest = i_strRequest.Replace("\r", "").Replace("\n", "");
		foreach (Enum_MesFunction item in Enum.GetValues(typeof(Enum_MesFunction)))
		{
			string i_strStart = $"<STRUCT_{item}";
			string i_strEnd = $"</STRUCT_{item}>";
			string pattern = i_strStart + ".*?" + i_strEnd;
			MatchCollection matches = Regex.Matches(i_strRequest, pattern, RegexOptions.IgnoreCase);
			foreach (Match match in matches)
			{
				dic.Add(dic.Count + 1, match.Value);
			}
		}
		return dic;
	}

	public override void Sub_End()
	{
		foreach (KeyValuePair<Type, object> item in m_dicMesFunction)
		{
			if (item.Value is Edc_MesFunction)
			{
				((Edc_MesFunction)item.Value).m_cts.Cancel();
			}
		}
		if (m_TcpClient != null)
		{
			try
			{
				m_blnTaskStopFlag = true;
				m_TcpClient.GetStream().Close();
				m_TcpClient.Close();
			}
			catch
			{
			}
		}
		base.Sub_End();
	}

	private async Task<bool> Fun_blnCheckConnectAsync(string i_strIP = "127.0.0.1", int i_i32Port = 12121)
	{
		if (m_blnTaskStopFlag)
		{
			return false;
		}
		if (m_TcpClient.Client == null || !m_TcpClient.Client.Connected)
		{
			m_tcsCompletionConnected = new TaskCompletionSource<bool>();
			Sub_Connect(i_strIP, i_i32Port);
			await m_tcsCompletionConnected.Task.Fun_fdcWithTimeout(5000, delegate
			{
			});
			OnShowMessage(Enum_LogType.Info, "MES connection established successfully...");
			return true;
		}
		if (m_TcpClient.Client.Poll(-1, SelectMode.SelectRead))
		{
			byte[] buffer = new byte[1];
			if (m_TcpClient.Client.Receive(buffer, SocketFlags.Peek) == 0)
			{
				m_tcsCompletionConnected = new TaskCompletionSource<bool>();
				Sub_Connect(i_strIP, i_i32Port);
				await m_tcsCompletionConnected.Task.Fun_fdcWithTimeout(5000, delegate
				{
				});
				OnShowMessage(Enum_LogType.Info, "TCP Connection established successfully...");
			}
		}
		return true;
	}

	private Task Sub_Connect(string i_strIP, int i_i32Port)
	{
		try
		{
			m_TcpClient = new TcpClient();
			m_TcpClient.ReceiveBufferSize = 20000;
			m_TcpClient.Connect(i_strIP, i_i32Port);
			m_tcsCompletionConnected.TrySetResult(result: true);
		}
		catch (SocketException fdcSocketException)
		{
			m_i32ConnectError++;
			if (m_i32ConnectError % 80 == 0)
			{
				m_tcsCompletionConnected.TrySetException(new SocketException((int)fdcSocketException.SocketErrorCode));
				throw new SocketException((int)fdcSocketException.SocketErrorCode);
			}
		}
		catch (Exception ex)
		{
			m_i32ConnectError++;
			m_tcsCompletionConnected.TrySetException(ex);
		}
		return Task.CompletedTask;
	}

	private NetworkStream Sub_GetStream(NetworkStream i_Stream)
	{
		i_Stream = m_TcpClient.GetStream();
		if (i_Stream.CanRead && i_Stream.DataAvailable)
		{
			return i_Stream;
		}
		return null;
	}

	public async void Sub_SendResponse(string i_strResponse)
	{
		try
		{
			byte[] a_bytsResponse = ResponseHelper.CreateResponse(i_strResponse);
			await m_TcpClient.GetStream().WriteAsync(a_bytsResponse, 0, a_bytsResponse.Length);
		}
		catch (IOException ex)
		{
			IOException fdcIoException = ex;
			base.m_edcLogger.Error("Communication failed: \r\n" + fdcIoException.Message, null, "Sub_SendResponse", 494);
		}
		catch (InvalidOperationException ex2)
		{
			InvalidOperationException fdcInvalidOperationException = ex2;
			base.m_edcLogger.Error("Communication failed: \r\n" + fdcInvalidOperationException.Message, null, "Sub_SendResponse", 498);
		}
	}

	public void Sub_AddObject<T>(object i_objObject)
	{
		if (i_objObject != null && i_objObject is Edc_MesFunction)
		{
			m_dicMesFunction.Add(typeof(T), i_objObject);
		}
	}

	public T Fun_edcGetObject<T>() where T : class
	{
		return (T)m_dicMesFunction.Where((KeyValuePair<Type, object> s) => s.Key == typeof(T)).First().Value;
	}

	public void Sub_RemoveObject<T>()
	{
		m_dicMesFunction?.Remove(typeof(T));
	}

	public bool Fun_Execute()
	{
		throw new NotImplementedException();
	}


	//≤‚ ‘
	public void Func_Test()
    {
        foreach (KeyValuePair<Type, object> item in m_dicMesFunction)
		{
            if (item.Value is Edc_MesFunction)
            {
                ((Edc_MesFunction)item.Value).Fun_Test();
            }
        }
    }
}
