using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BR.AN.PviServices;
using Ersa.Mes.Common;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.PLC.Model;
using Ersa.Mes.PLC.Modules;

namespace Ersa.Mes.PLC;

public class Edc_BrPlc : Inf_PLC, IDisposable
{
	private readonly SemaphoreSlim m_fdcGroupSemaphore = new SemaphoreSlim(1);

	public List<VariableCollection> m_lstGroup = new List<VariableCollection>();

	private readonly Dictionary<string, TaskCompletionSource<List<Edc_PLCElement>>> m_dicSpsElement = new Dictionary<string, TaskCompletionSource<List<Edc_PLCElement>>>();

	private List<string> m_lstVariableError;

	public DeviceType m_DeviceType = DeviceType.AR000;

	public string m_strIp = "127.0.0.1";

	public int m_i32Port = 20000;

	public Service m_fdcService;

	public string Pro_strServiceName = "service";

	public Cpu m_fdcCpu;

	public string Pro_strCpuName = "CPU";

	private VariableCollection m_fdcGroup;

	private VariableCollection m_fdcGroupEvent;

	private VariableCollection m_fdcGroupTemp;

	private TaskCompletionSource<bool> m_fdcCpuConnectionCompletionSource;

	private TaskCompletionSource<bool> m_fdcTaskUploadCompletionSource;

	private TaskCompletionSource<bool> m_fdcTaskConnectCompletionSource;

	private TaskCompletionSource<bool> m_fdcVariableUploadCompletionSource;

	private TaskCompletionSource<bool> m_fdcVariableConnectCompletionSource;

	private TaskCompletionSource<bool> m_fdcVariableReadCompletionSource;

	private TaskCompletionSource<bool> m_fdcGroupConnectedCompletionSource;

	public List<string> m_lstTaskDistinct = new List<string>();

	private List<Edc_PLCElement> m_lstCacheSpsElement = new List<Edc_PLCElement>();

	public Inf_Logger m_edcLogger { get; set; }

	public bool Pro_blnCpuConnected
	{
		get
		{
			if (m_fdcCpu != null)
			{
				return m_fdcCpu.IsConnected;
			}
			return false;
		}
	}

	public Edc_BrPlc(Inf_Logger i_edcLogger)
	{
		m_edcLogger = i_edcLogger;
	}

	public void Dispose()
	{
	}

	public System.Threading.Tasks.Task Fun_ConnectAsync(bool i_blnOnline, string i_strAddress)
	{
		m_fdcCpuConnectionCompletionSource = new TaskCompletionSource<bool>();
		try
		{
			Sub_CreatePlcService();
			m_strIp = (i_blnOnline ? i_strAddress : "127.0.0.1");
			if (!m_fdcService.IsConnected)
			{
				m_fdcService.Connect();
			}
		}
		catch (System.Exception ex)
		{
			m_edcLogger.Error("Error connecting to the PLC...Detail:'" + ex.Message + "'", null, "Fun_ConnectAsync", 170);
		}
		return m_fdcCpuConnectionCompletionSource.Task.Fun_fdcWithTimeout(3000, delegate
		{
			throw new System.Exception("Cpu connect Timeout");
		});
	}

	public void Sub_DisConnect()
	{
		m_fdcCpu?.Disconnect();
		m_fdcCpu?.Dispose();
		Cpu fdcCpu = m_fdcCpu;
		if (fdcCpu != null && fdcCpu.IsConnected)
		{
		}
	}

	private void Sub_CreatePlcService()
	{
		if (m_fdcService == null)
		{
			m_fdcService = new Service(Pro_strServiceName);
			m_fdcService.Connected += Sub_ServiceConnected;
			m_fdcService.ConnectionChanged += Sub_ConnectionChanged;
			m_fdcService.Error += Sub_PviError;
		}
		m_fdcService.Connect();
	}

	public void Sub_ServiceConnected(object i_objSender, PviEventArgs i_fdcPviEventArgs)
	{
		Sub_CreateCpu();
		m_fdcCpu.Connect();
	}

	private void Sub_ConnectionChanged(object i_objSender, PviEventArgs i_fdcPviEventArgs)
	{
		m_edcLogger.Debug("Sub_ConnectionChanged", null, "Sub_ConnectionChanged", 225);
	}

	public void Sub_PviError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
	{
		m_edcLogger.Error($"PLC Error...ErrorCode:'{i_fdcPviEventArgs.ErrorCode}'  ErrorText:'{i_fdcPviEventArgs.ErrorText}'", null, "Sub_PviError", 235);
	}

	private void Sub_CreateCpu()
	{
		m_fdcCpu = new Cpu(m_fdcService, Pro_strCpuName);
		m_fdcCpu.Connection.DeviceType = m_DeviceType;
		m_fdcCpu.Connected += Sub_CpuConnected;
	}

	public void Sub_CpuConnected(object i_objSender, PviEventArgs i_edcPviEventArgs)
	{
		if (i_edcPviEventArgs.ErrorCode == 0)
		{
			m_fdcCpuConnectionCompletionSource.TrySetResult(result: true);
		}
		else
		{
			m_edcLogger.Error($"Sub_CpuConnected Error...ErrorCode :'{i_edcPviEventArgs.ErrorCode}'  ErrorText:'{i_edcPviEventArgs.ErrorText}'", null, "Sub_CpuConnected", 266);
		}
	}

	private void Sub_CpuDateTimeRead(object sender, CpuEventArgs e)
	{
		string test1 = e.DateTime.ToString();
	}

	private void Tasks_Uploaded(object sender, PviEventArgs e)
	{
		m_fdcTaskUploadCompletionSource.TrySetResult(result: true);
	}

	private void Task_Connected(object sender, PviEventArgs e)
	{
		BR.AN.PviServices.Task a_Task = (BR.AN.PviServices.Task)sender;
		a_Task.Variables.Uploaded += Variables_Uploaded;
		a_Task.Variables.Upload();
	}

	private async Task<bool> Fun_blnTaskAlreadyConnect(string i_strVariableName)
	{
		if (string.IsNullOrWhiteSpace(i_strVariableName))
		{
			return false;
		}
		string a_strTaskName = string.Empty;
		if (i_strVariableName.Split('.').Length != 0)
		{
			a_strTaskName = i_strVariableName.Split('.')[0];
		}
		if (m_fdcCpu.Tasks.Count == 0)
		{
			m_fdcTaskUploadCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks.Uploaded += Tasks_Uploaded;
			m_fdcCpu.Tasks.Upload();
			await m_fdcTaskUploadCompletionSource.Task.Fun_fdcWithTimeout(1000, delegate
			{
			});
		}
		foreach (object item in m_fdcCpu.Tasks.Values)
		{
			BR.AN.PviServices.Task a_Task = (BR.AN.PviServices.Task)item;
			if (a_Task.Name.Equals(a_strTaskName, StringComparison.OrdinalIgnoreCase) && !a_Task.IsConnected)
			{
				m_fdcTaskConnectCompletionSource = new TaskCompletionSource<bool>();
				a_Task.Connected += Task_Connected;
				a_Task.Connect();
			}
		}
		await m_fdcTaskConnectCompletionSource.Task.Fun_fdcWithTimeout(2000, delegate
		{
		});
		return m_fdcCpu.Tasks[a_strTaskName].IsConnected;
	}

	public async Task<Variable> Sub_GetVariableFromTask(string i_strVariableName)
	{
		if (m_fdcCpu == null || !m_fdcCpu.IsConnected)
		{
			return null;
		}
		if (string.IsNullOrWhiteSpace(i_strVariableName) || i_strVariableName.Split('.').Length < 1)
		{
			return null;
		}
		string a_strTaskName = i_strVariableName.Split('.')[0];
		string a_strVariableName = i_strVariableName.Split('.')[1];
		if (m_fdcCpu.Tasks.Count == 0)
		{
			m_fdcTaskUploadCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks.Uploaded += Tasks_Uploaded;
			m_fdcCpu.Tasks.Upload();
			await m_fdcTaskUploadCompletionSource.Task.Fun_fdcWithTimeout(2000, delegate
			{
			});
		}
		if (!m_fdcCpu.Tasks[a_strTaskName].IsConnected)
		{
			m_fdcTaskConnectCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks[a_strTaskName].Connected += Task_Connected;
			m_fdcCpu.Tasks[a_strTaskName].Connect();
			await m_fdcTaskConnectCompletionSource.Task.Fun_fdcWithTimeout(2000, delegate
			{
			});
		}
		if (m_fdcCpu.Tasks[a_strTaskName].Variables == null || m_fdcCpu.Tasks[a_strTaskName].Variables.Count == 0)
		{
			m_fdcVariableUploadCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks[a_strTaskName].Variables.Uploaded += Variables_Uploaded;
			m_fdcCpu.Tasks[a_strTaskName].Variables.Upload();
			await m_fdcVariableUploadCompletionSource.Task.Fun_fdcWithTimeout(2000, delegate
			{
			});
		}
		Variable variable = m_fdcCpu.Tasks[a_strTaskName].Variables[a_strVariableName];
		if (variable == null)
		{
			return null;
		}
		if (!variable.IsConnected)
		{
			m_fdcVariableConnectCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks[a_strTaskName].Variables[a_strVariableName].Connected += Variable_Connected;
			m_fdcCpu.Tasks[a_strTaskName].Variables[a_strVariableName].Connect();
			await m_fdcVariableConnectCompletionSource.Task.Fun_fdcWithTimeout(2000, delegate
			{
			});
		}
		return variable;
	}

	public Variable Sub_GetVariableFromCpu(string i_strVariableName)
	{
		if (!Pro_blnCpuConnected)
		{
			return null;
		}
		return m_fdcCpu.Variables[i_strVariableName];
	}

	public async System.Threading.Tasks.Task Sub_VariablesRegisterAsync(IEnumerable<string> i_lstVariable, CancellationToken i_fdcToken)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Not connected...");
		}
		m_fdcVariableConnectCompletionSource = new TaskCompletionSource<bool>();
		IList<string> source = (i_lstVariable as IList<string>) ?? i_lstVariable.ToList();
		List<string> a_lstNewVariable = source.Where((string i_strVar) => !Fun_blnVariableAlreadyCreated(i_strVar)).ToList();
		if (a_lstNewVariable.Any())
		{
			Sub_VariableMoveToNewGroup(m_fdcGroup, m_fdcGroupTemp);
			m_fdcGroupTemp.Disconnect();
			Sub_GroupFill(a_lstNewVariable, m_fdcGroupTemp, i_fdcToken);
			m_fdcGroupTemp.Connect();
			m_edcLogger.Debug($"Sub_VariablesRegisterAsync() Count={a_lstNewVariable.Count}", null, "Sub_VariablesRegisterAsync", 476);
			await m_fdcVariableConnectCompletionSource.Task.Fun_fdcWithTimeout(100000, delegate
			{
				m_edcLogger.Error("Timeout registering variables...Incorrect Variables " + string.Join(",", m_lstVariableError) + "  - New Variables " + string.Join(",", a_lstNewVariable), null, "Sub_VariablesRegisterAsync", 480);
				m_lstVariableError = null;
				throw new System.Exception("Timeout registering variables...");
			});
		}
	}

	public System.Threading.Tasks.Task Sub_VariablesUnregister(IEnumerable<string> i_lstVariable, CancellationToken i_fdcToken)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Not connected...");
		}
		if (i_lstVariable == null)
		{
			throw new System.Exception("List empty...");
		}
		int num = 0;
		foreach (string item in i_lstVariable)
		{
			i_fdcToken.ThrowIfCancellationRequested();
			string text = Fun_strGetCurrectName(item);
			Sub_VariableRemoveFromGroup(m_fdcGroupEvent, item);
			Sub_VariableRemoveFromGroup(m_fdcGroup, item);
			Sub_VariableRemoveFromGroup(m_fdcGroupTemp, item);
			foreach (VariableCollection item2 in m_lstGroup)
			{
				Sub_VariableRemoveFromGroup(item2, text);
			}
			m_fdcCpu.Variables.Remove(text);
			num++;
		}
		m_edcLogger.Debug($"Sub_VariablesUnregister() Count : '{num}'", null, "Sub_VariablesUnregister", 522);
		return System.Threading.Tasks.Task.FromResult(result: true);
	}

	public IDisposable Sub_RegisterEventHandler(string i_strVariableName, System.Action i_delHandler)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Not Connected...");
		}
		Variable a_Variable = Sub_GetVariableFromCpu(i_strVariableName);
		a_Variable.ValueChanged += delegate(object i_objSender, VariableEventArgs i_fdcEventArgs)
		{
			if (i_fdcEventArgs.Action == BR.AN.PviServices.Action.VariableValueChangedEvent)
			{
				i_delHandler();
			}
		};
		return null;
	}

	private void Variables_Uploaded(object sender, PviEventArgs e)
	{
		VariableCollection a_VariableCollection = sender as VariableCollection;
		BR.AN.PviServices.Task a_Task = (BR.AN.PviServices.Task)a_VariableCollection.Parent;
		m_fdcVariableUploadCompletionSource?.TrySetResult(result: true);
		m_fdcTaskConnectCompletionSource?.TrySetResult(result: true);
	}

	private void Variable_Connected(object sender, PviEventArgs e)
	{
		m_fdcVariableConnectCompletionSource.TrySetResult(result: true);
	}

	public Variable Fun_fdcVariableCreate(string i_strVariable, int i_i32CycleTime)
	{
		Variable variable = new Variable(m_fdcCpu, i_strVariable)
		{
			Polling = false
		};
		variable.Access |= Access.ReadAndWrite | Access.FASTECHO;
		variable.RefreshTime = i_i32CycleTime;
		variable.RuntimeObjectIndex = Variable.ROIoptions.NonZeroBasedArrayIndex;
		return variable;
	}

	private Variable Fun_edcVariableGet(string i_strVariable)
	{
		if (string.IsNullOrWhiteSpace(i_strVariable) || i_strVariable.Split('.').Length < 1)
		{
			return null;
		}
		string a_strTaskName = i_strVariable.Split('.')[0];
		string a_strVariableName = i_strVariable.Split('.')[1];
		return m_fdcCpu.Tasks[a_strTaskName].Variables[a_strVariableName];
	}

	private void Sub_VariableMoveToNewGroup(VariableCollection i_GroupNew, VariableCollection i_GroupOld)
	{
		foreach (object item in i_GroupOld)
		{
			i_GroupNew.Add(item as Variable);
		}
		i_GroupOld.Clear();
	}

	private void Sub_VariableRemoveFromGroup(VariableCollection i_fdcGroup, string i_strVariable)
	{
		Variable variable = (i_fdcGroup.Contains(i_strVariable) ? m_fdcGroup[i_strVariable] : null);
		if (variable != null)
		{
			i_fdcGroup.Remove(variable);
		}
	}

	private bool Fun_blnVariableAlreadyCreated(string i_strVariableName)
	{
		if (string.IsNullOrWhiteSpace(i_strVariableName))
		{
			return false;
		}
		if (Sub_GetVariableFromCpu(i_strVariableName) == null)
		{
			return false;
		}
		return true;
	}

	private void Sub_GroupInitialize()
	{
		m_lstGroup.Clear();
		m_fdcGroup = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString())
		{
			RefreshTime = 400
		};
		m_fdcGroupTemp = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
		Sub_GroupEventRegister(m_fdcGroupTemp);
		m_fdcGroupEvent = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
		m_fdcGroupEvent.Error += Sub_GroupEventError;
		m_fdcGroupEvent.CollectionPropertyChanged += Sub_GroupEventPropertyChanged;
		m_fdcGroupEvent.RefreshTime = 100;
	}

	private void Sub_GroupEventPropertyChanged(object i_objSender, CollectionEventArgs i_fdcEventArgse)
	{
		m_edcLogger.Debug($"e.Objects.Count = {i_fdcEventArgse.Objects.Count}  SUB_EventGruppePropertyChanged", null, "Sub_GroupEventPropertyChanged", 678);
	}

	public async System.Threading.Tasks.Task Fun_fdcGroupCreateVariableAsync(IEnumerable<string> i_enuVariable, string i_strGroupName, int i_i32CycleTime = 100)
	{
		List<string> a_lstVariable = i_enuVariable.ToList();
		if (string.IsNullOrEmpty(i_strGroupName) || !a_lstVariable.Any())
		{
			throw new System.Exception("i_strGroupName");
		}
		if (!Pro_blnCpuConnected)
		{
			return;
		}
		try
		{
			await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: true);
			VariableCollection a_VariableCollection = Fun_fdcGroupGet(i_strGroupName);
			if (a_VariableCollection == null)
			{
				a_VariableCollection = Fun_fdcGroupCreate(i_strGroupName, i_i32CycleTime);
				a_VariableCollection.CollectionValuesRead += Sub_GroupValuesRead;
				a_VariableCollection.CollectionValuesWritten += Sub_GroupValuesWrite;
				a_VariableCollection.CollectionConnected += Sub_CollectionChanged;
				a_VariableCollection.CollectionDisconnected += Sub_CollectionChanged;
			}
			a_VariableCollection.Disconnect();
			foreach (string item in a_lstVariable)
			{
				Fun_strGetCurrectName(item);
				a_VariableCollection.Add(a_VariableCollection["item"]);
			}
			m_fdcGroupConnectedCompletionSource = new TaskCompletionSource<bool>();
			a_VariableCollection.Active = true;
			a_VariableCollection.Connect();
			await m_fdcGroupConnectedCompletionSource.Task.Fun_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
		}
		finally
		{
			m_fdcGroupSemaphore.Release();
		}
	}

	private void Sub_GroupValuesRead(object sender, CollectionEventArgs arg)
	{
		if (!(sender is VariableCollection))
		{
			return;
		}
		List<Edc_PLCElement> list = new List<Edc_PLCElement>();
		foreach (Variable a_Variable in arg.Objects.Values)
		{
			Edc_PLCElement item = new Edc_PLCElement();
			list.Add(item);
		}
	}

	private void Sub_GroupValuesWrite(object sender, CollectionEventArgs e)
	{
		throw new NotImplementedException();
	}

	private VariableCollection Fun_fdcGroupCreate(string i_strGroupName, int i_i32CycleTime)
	{
		VariableCollection variableCollection = new VariableCollection(m_fdcCpu, i_strGroupName);
		variableCollection.CollectionError += Sub_PviCollectionError;
		variableCollection.RefreshTime = i_i32CycleTime;
		m_lstGroup.Add(variableCollection);
		return variableCollection;
	}

	private VariableCollection Fun_fdcGroupGet(string i_strGroupName)
	{
		return m_lstGroup.FirstOrDefault((VariableCollection s) => s.Name.Equals(i_strGroupName + ".Variables"));
	}

	private void Sub_GroupEventRegister(VariableCollection i_fdcGroup)
	{
		i_fdcGroup.Error += Sub_PviCollectionError;
		i_fdcGroup.CollectionConnected += Sub_CollectionConnected;
		i_fdcGroup.ValueRead += Sub_GroupValuesRead;
	}

	private void Sub_GroupEventUnregister(VariableCollection i_fdcGroup)
	{
		i_fdcGroup.Error -= Sub_PviCollectionError;
		i_fdcGroup.CollectionConnected -= Sub_CollectionConnected;
		i_fdcGroup.ValueRead -= Sub_GroupValuesRead;
	}

	public void Sub_GroupEventActive()
	{
		if (m_fdcGroupEvent != null && !m_fdcGroupEvent.Active && m_fdcGroupEvent.Count > 0)
		{
			m_fdcGroupEvent.Active = true;
		}
	}

	public void Sub_GroupEventDisactivate()
	{
		VariableCollection a_fdcGroupEvent = m_fdcGroupEvent;
	}

	private void Sub_GroupFill(IEnumerable<string> i_lstVariable, VariableCollection i_fdcVariableCollection, CancellationToken i_fdcToken)
	{
		foreach (string item in i_lstVariable)
		{
			i_fdcToken.ThrowIfCancellationRequested();
			Variable variable = Fun_edcVariableGet(item);
			if (!variable.IsConnected)
			{
				variable.Connect();
			}
			if (!i_fdcVariableCollection.Contains(variable))
			{
				i_fdcVariableCollection.Add(variable);
			}
		}
		i_fdcVariableCollection.Connect();
	}

	private void Sub_GroupValuesRead(object i_objSender, PviEventArgs e)
	{
		if (i_objSender is VariableCollection variableCollection)
		{
			m_edcLogger.Debug($"{variableCollection.Name} read: {variableCollection.Count} variables  SUB_CollectionValuesRead", null, "Sub_GroupValuesRead", 873);
		}
		m_fdcVariableConnectCompletionSource.SetResult(result: true);
	}

	private void Sub_CollectionConnected(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
	{
		if (i_objSender is VariableCollection variableCollection)
		{
			variableCollection.ReadValues();
			variableCollection.Active = true;
		}
	}

	private void Sub_CollectionChanged(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
	{
		if (i_objSender is VariableCollection variableCollection)
		{
			string message = (variableCollection.Active ? "Activated" : "Deactivated");
			m_edcLogger.Debug($"PLC -> Sub_CollectionChanged()  CollectionName:'{variableCollection.Name}'  CollectionCount:'{variableCollection.Count}'", null, "Sub_CollectionChanged", 898);
			m_fdcGroupConnectedCompletionSource?.TrySetResult(result: true);
		}
	}

	private void Sub_PviCollectionError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
	{
		string i_strMessage = "PLC -> Collection Error: " + i_fdcPviEventArgs.Address + " --- " + i_fdcPviEventArgs.ErrorText;
		m_edcLogger.Error(i_strMessage, null, "Sub_PviCollectionError", 911);
		if (i_fdcPviEventArgs.Action != BR.AN.PviServices.Action.VariableDisconnect)
		{
			Sub_AddErrorVariable(i_fdcPviEventArgs.Name);
		}
	}

	private void Sub_GroupEventError(object i_objSender, PviEventArgs i_fdcEventArgs)
	{
		string text = i_fdcEventArgs.ErrorText;
		if (i_objSender is Variable variable)
		{
			text = variable.FullName + "message = " + text;
		}
		throw new System.Exception("Sub_GroupEventError()...ErrorText = '" + text + "'");
	}

	public async Task<List<Edc_PLCElement>> Sub_lstReadValue(string i_strPVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Can not read if cpu not connect...");
		}
		Variable variable = await Sub_GetVariableFromTask(i_strPVariable);
		variable.Value.ToString();
		m_dicSpsElement.TryGetValue(i_strPVariable, out var _);
		try
		{
			TaskCompletionSource<List<Edc_PLCElement>> tcs = new TaskCompletionSource<List<Edc_PLCElement>>();
			m_dicSpsElement[i_strPVariable] = tcs;
			variable.ValueRead -= Variable_ValueRead;
			variable.ValueRead += Variable_ValueRead;
			variable.ReadValue();
			await tcs.Task.Fun_fdcWithTimeout(2000, delegate
			{
				string text = "111";
			});
			return m_dicSpsElement[i_strPVariable].Task.Result;
		}
		catch (System.Exception)
		{
			return null;
		}
	}

	public async void Sub_WriteValue(string i_strVariable, string i_strValue)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Can not write if cpu not connect...");
		}
		(await Sub_GetVariableFromTask(i_strVariable)).WriteValue();
	}

	public async void Sub_WriteValue(string i_strVariable, string i_strValue, string i_strMembers)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Can not write if cpu not connect...");
		}
		Variable variable = await Sub_GetVariableFromTask(i_strVariable);
		if (variable != null)
		{
			m_fdcVariableReadCompletionSource = new TaskCompletionSource<bool>();
			variable.ValueRead += Variable_ValueRead;
			variable.ReadValue();
			await m_fdcVariableReadCompletionSource.Task.ConfigureAwait(continueOnCapturedContext: false);
			variable.ValueWritten -= Variable_ValueWritten;
			variable.WriteValueAutomatic = false;
			variable.Value[i_strMembers] = i_strValue;
			variable.ValueWritten += Variable_ValueWritten;
			variable.WriteValue();
		}
	}

	public async System.Threading.Tasks.Task Sub_WriteValueAsync(string i_strTaskDotVariable, IEnumerable<KeyValuePair<string, string>> i_listMembers)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Can not write if cpu not connect...");
		}
		Variable variable = await Sub_GetVariableFromTask(i_strTaskDotVariable);
		if (variable == null)
		{
			return;
		}
		m_fdcVariableReadCompletionSource = new TaskCompletionSource<bool>();
		variable.ValueRead += Variable_ValueRead;
		variable.ReadValue();
		await m_fdcVariableReadCompletionSource.Task.ConfigureAwait(continueOnCapturedContext: false);
		variable.ValueWritten -= Variable_ValueWritten;
		variable.WriteValueAutomatic = false;
		foreach (KeyValuePair<string, string> item in i_listMembers.ToList())
		{
			variable.Value[item.Key] = item.Value;
		}
		variable.ValueWritten += Variable_ValueWritten;
		variable.WriteValue();
	}

	public async System.Threading.Tasks.Task Sub_WriteMesAddress(string i_strTaskDotVariable)
	{
		string a_strFullname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "Pvi.json");
		try
		{
			Dictionary<string, string> directory = new Dictionary<string, string>();
			Edc_PlcAddresses address = SerializerHelper.Fun_DeserializeJson<Edc_PlcAddresses>(a_strFullname);
			foreach (Edc_PlcTag item in address.Tags)
			{
				string i_strCurrentName2 = Fun_strGetCurrectName(item.TagName);
				i_strCurrentName2 = Fun_strGetMemberName(i_strCurrentName2, i_strTaskDotVariable);
				directory.Add(i_strCurrentName2, item.Value);
			}
			await Sub_WriteValueAsync(i_strTaskDotVariable, directory);
		}
		catch (System.Exception)
		{
			throw new System.Exception("Damaged file " + a_strFullname);
		}
	}

	private bool Fun_blnConvertValueType(Variable variable, ref string i_strValue, string i_strMembers = "")
	{
		return (string.IsNullOrEmpty(i_strMembers) ? variable.Value.DataType : variable.Value[i_strMembers].DataType) switch
		{
			DataType.UInt8 => true, 
			_ => false, 
		};
	}

	private void Variable_ValueRead(object sender, PviEventArgs e)
	{
		Variable v1 = (Variable)sender;
		BR.AN.PviServices.Task t1 = (BR.AN.PviServices.Task)v1.Parent;
		string key = t1.Name + "." + v1.Name;
		m_lstCacheSpsElement = new List<Edc_PLCElement>();
		m_lstCacheSpsElement = Fun_strGetAllValue(key, t1.Name, v1.Name, v1);
		m_dicSpsElement.TryGetValue(key, out var value);
		value?.TrySetResult(m_lstCacheSpsElement);
		m_fdcVariableReadCompletionSource?.TrySetResult(result: true);
	}

	private List<Edc_PLCElement> Fun_strGetAllValue(string i_strKey, string i_strTaskName, string i_strPVariable, Variable variable)
	{
		if (variable.Members == null)
		{
			Edc_PLCElement edc_PLCElement = new Edc_PLCElement();
			edc_PLCElement.Pro_strGroupName = i_strKey;
			edc_PLCElement.Pro_strTask = i_strTaskName;
			edc_PLCElement.Pro_strPVariable = i_strPVariable;
			edc_PLCElement.Pro_objValue = variable.Value[variable.Name];
			edc_PLCElement.Pro_strAddress = i_strTaskName + "." + i_strPVariable + "." + variable.StructMemberName;
			edc_PLCElement.Pro_strStructMember = variable.StructMemberName;
			Edc_PLCElement model2 = edc_PLCElement;
			m_lstCacheSpsElement.Add(model2);
		}
		else
		{
			Edc_PLCElement edc_PLCElement = new Edc_PLCElement();
			edc_PLCElement.Pro_strGroupName = i_strKey;
			edc_PLCElement.Pro_strTask = i_strTaskName;
			edc_PLCElement.Pro_strPVariable = i_strPVariable;
			edc_PLCElement.Pro_objValue = variable.Value.ToString();
			edc_PLCElement.Pro_strAddress = i_strTaskName + "." + i_strPVariable + "." + variable.StructMemberName;
			edc_PLCElement.Pro_strStructMember = variable.StructMemberName;
			Edc_PLCElement model = edc_PLCElement;
			m_lstCacheSpsElement.Add(model);
			foreach (object item in variable.Members)
			{
				Fun_strGetAllValue(i_strKey, i_strTaskName, i_strPVariable, (Variable)item);
			}
		}
		return m_lstCacheSpsElement;
	}

	private void Variable_ValueWritten(object sender, PviEventArgs e)
	{
		Variable a_Variable = sender as Variable;
		m_edcLogger.Debug("Middleware -> " + e.Action.ToString() + "  " + a_Variable.FullName + "," + a_Variable.Value.ToString(), null, "Variable_ValueWritten", 1198);
	}

	private string Fun_strConvertWriteValueType(string i_strValue, Variable i_Variable)
	{
		TypeCode typeCode = i_Variable.Value.GetTypeCode();
		string result = string.Empty;
		switch (typeCode)
		{
		case TypeCode.Boolean:
			i_Variable.Value.Assign(bool.Parse(i_strValue));
			break;
		case TypeCode.Int32:
			i_Variable.Value.Assign(int.Parse(i_strValue));
			break;
		case TypeCode.UInt32:
			i_Variable.Value.Assign(uint.Parse(i_strValue));
			break;
		case TypeCode.Int64:
			i_Variable.Value.Assign(long.Parse(i_strValue));
			break;
		case TypeCode.UInt64:
			i_Variable.Value.Assign((float)(double)ulong.Parse(i_strValue));
			break;
		case TypeCode.Single:
			i_Variable.Value.Assign(float.Parse(i_strValue));
			break;
		case TypeCode.Double:
			i_Variable.Value.Assign(double.Parse(i_strValue));
			break;
		case TypeCode.String:
			i_Variable.Value = i_strValue;
			break;
		case TypeCode.SByte:
			i_Variable.Value.Assign(sbyte.Parse(i_strValue));
			break;
		case TypeCode.Byte:
			i_Variable.Value.Assign(byte.Parse(i_strValue));
			break;
		default:
			result = "Missing dataType : " + typeCode;
			break;
		}
		return result;
	}

	public IDisposable Fun_fdcEventHandlerRegister(string i_strVariable, System.Action i_delHandler)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Not Connected...");
		}
		Variable a_Variable = Sub_GetVariableFromCpu(i_strVariable);
		VariableEventHandler a_delHandler = delegate(object i_objSender, VariableEventArgs e)
		{
			if (e.Action == BR.AN.PviServices.Action.VariableValueChangedEvent)
			{
				i_delHandler();
			}
		};
		a_Variable.ValueChanged += a_delHandler;
		if (!m_fdcGroupEvent.Contains(a_Variable))
		{
			a_Variable.RefreshTime = 100;
			m_fdcGroupEvent.Add(a_Variable);
			a_Variable.Active = true;
		}
		return Edc_Disposable.Fun_fdcCreate(delegate
		{
			a_Variable.ValueChanged -= a_delHandler;
		});
	}

	public string Fun_strReadValue(string i_strAddress)
	{
		if (!Pro_blnCpuConnected)
		{
			return string.Empty;
		}
		Variable variable = Sub_GetVariableFromCpu(i_strAddress);
		variable.ReadValue(synchronous: true);
		return variable.Value.ToString(CultureInfo.InvariantCulture);
	}

	public float Fun_sngReadValue(string i_strVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			return 0f;
		}
		return Sub_GetVariableFromCpu(i_strVariable).Value.ToSingle(CultureInfo.InvariantCulture.NumberFormat);
	}

	public uint Fun_u32ReadValue(string i_strVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			return 0u;
		}
		return Sub_GetVariableFromCpu(i_strVariable).Value.ToUInt32(CultureInfo.InvariantCulture.NumberFormat);
	}

	public int Fun_i32ReadValue(string i_strVariable)
	{
		throw new NotImplementedException();
	}

	public short Fun_i16ReadValue(string i_strVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			return 0;
		}
		return Sub_GetVariableFromCpu(i_strVariable).Value.ToInt16(CultureInfo.InvariantCulture.NumberFormat);
	}

	public ushort Fun_u16ReadValue(string i_strVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			return 0;
		}
		return Sub_GetVariableFromCpu(i_strVariable).Value.ToUInt16(CultureInfo.InvariantCulture.NumberFormat);
	}

	public byte Fun_bytReadValue(string i_strVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			return 0;
		}
		return Sub_GetVariableFromCpu(i_strVariable).Value.ToByte(CultureInfo.InvariantCulture.NumberFormat);
	}

	public bool Fun_blnReadValue(string i_strVariable)
	{
		if (!Pro_blnCpuConnected)
		{
			return false;
		}
		return Sub_GetVariableFromCpu(i_strVariable).Value.ToBoolean(CultureInfo.InvariantCulture.NumberFormat);
	}

	private string Fun_strGetMemberName(string i_strVariable, string i_strTaskPVariable)
	{
		if (i_strVariable.StartsWith(i_strTaskPVariable + "."))
		{
			i_strVariable = i_strVariable.Replace(i_strTaskPVariable + ".", "");
		}
		return i_strVariable;
	}

	private void Sub_AddErrorVariable(string i_strVariableName)
	{
		if (m_lstVariableError == null)
		{
			m_lstVariableError = new List<string>();
		}
		if (!m_lstVariableError.Contains(i_strVariableName))
		{
			m_lstVariableError.Add(i_strVariableName);
		}
	}

	public Task<IEnumerable<Edc_PLCElement>> Fun_fdcGroupReadAsync(string i_strGruppenName)
	{
		throw new NotImplementedException();
	}

	public async System.Threading.Tasks.Task Fun_fdcGroupActiveAsync(string i_strGroupName)
	{
		if (string.IsNullOrEmpty(i_strGroupName))
		{
			throw new System.Exception("Fun_fdcGroupActiveAsync(i_strGroupName)...");
		}
		if (!Pro_blnCpuConnected)
		{
			return;
		}
		try
		{
			await m_fdcGroupSemaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
			VariableCollection variableCollection = Fun_fdcGroupGet(i_strGroupName);
			if (variableCollection == null)
			{
				throw new System.Exception("B&R sps group " + i_strGroupName + " does not exist");
			}
			m_fdcGroupConnectedCompletionSource = new TaskCompletionSource<bool>();
			variableCollection.Active = true;
			variableCollection.Connect();
			await m_fdcGroupConnectedCompletionSource.Task.Fun_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
		}
		finally
		{
			m_fdcGroupSemaphore.Release();
		}
	}

	public Variable Fun_fdcCreateNewCpuVariable(string i_strVariable, int i_i32CycleTime)
	{
		Variable variable = new Variable(m_fdcCpu, i_strVariable)
		{
			Polling = false
		};
		variable.Access |= Access.ReadAndWrite | Access.FASTECHO;
		variable.RefreshTime = i_i32CycleTime;
		variable.RuntimeObjectIndex = Variable.ROIoptions.NonZeroBasedArrayIndex;
		return variable;
	}

	public System.Threading.Tasks.Task FUN_fdcGroupDisableAsync(string i_strGroupName)
	{
		throw new NotImplementedException();
	}

	public IDisposable Fun_fdcRegisterEventHandler(string i_strVariableName, System.Action i_delHandler)
	{
		if (!Pro_blnCpuConnected)
		{
			throw new System.Exception("Not Connected...");
		}
		Variable a_Variable = Fun_edcGetVariableFromCpu(i_strVariableName);
		a_Variable.ValueChanged += delegate(object i_objSender, VariableEventArgs i_fdcEventArgs)
		{
			if (i_fdcEventArgs.Action == BR.AN.PviServices.Action.VariableValueChangedEvent)
			{
				i_delHandler();
			}
		};
		if (!m_fdcGroupEvent.Contains(a_Variable))
		{
			a_Variable.RefreshTime = 400;
			m_fdcGroupEvent.Add(a_Variable);
			a_Variable.Active = true;
		}
		return null;
	}

	private Variable Fun_edcGetVariableFromCpu(string i_strVariableName)
	{
		if (!Pro_blnCpuConnected)
		{
			return null;
		}
		return m_fdcCpu.Variables[i_strVariableName];
	}

	public static string Fun_strGetValueFromPlc(string i_strAddress, Dictionary<string, List<Edc_PLCElement>> a_dic)
	{
		Sub_SplitPlcAddress(i_strAddress, out var i_strTaskName, out var i_strPVariableName, out var i_strMemberName);
		string a_strKey = i_strTaskName + "." + i_strPVariableName;
		a_dic.TryGetValue(a_strKey, out var value);
		if (value != null)
		{
			Edc_PLCElement item = value.Where((Edc_PLCElement s) => s.Pro_strStructMember.Equals(i_strMemberName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
			if (item != null)
			{
				return item.Pro_objValue.Replace(',', '.');
			}
		}
		return "";
	}

	public static int Fun_i32GetValueFromPlc(string i_strAddress, Dictionary<string, List<Edc_PLCElement>> a_dic)
	{
		int a_i32Default = -1;
		Sub_SplitPlcAddress(i_strAddress, out var i_strTaskName, out var i_strPVariableName, out var i_strMemberName);
		string a_strKey = i_strTaskName + "." + i_strPVariableName;
		a_dic.TryGetValue(a_strKey, out var value);
		if (value != null)
		{
			Edc_PLCElement item = value.Where((Edc_PLCElement s) => s.Pro_strStructMember.Equals(i_strMemberName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
			if (item != null)
			{
				if (item.Pro_objValue.IndexOf(",") >= 0)
				{
					int.TryParse(item.Pro_objValue.Split(',')[0], out var result3);
					return result3;
				}
				if (item.Pro_objValue.IndexOf(".") >= 0)
				{
					int.TryParse(item.Pro_objValue.Split('.')[0], out var result2);
					return result2;
				}
				int.TryParse(item.Pro_objValue, out var result4);
				return result4;
			}
		}
		return a_i32Default;
	}

	private static void Sub_SplitPlcAddress(string i_strAddress, out string i_strTaskName, out string i_strPVariableName, out string i_strMemberName)
	{
		string a_strAddress = Fun_strGetCurrectName(i_strAddress);
		i_strTaskName = string.Empty;
		i_strPVariableName = string.Empty;
		i_strMemberName = string.Empty;
		if (a_strAddress.Split('.').Length >= 1)
		{
			i_strTaskName = a_strAddress.Split('.')[0];
		}
		if (a_strAddress.Split('.').Length >= 2)
		{
			i_strPVariableName = a_strAddress.Split('.')[1];
		}
		if (a_strAddress.Split('.').Length >= 3)
		{
			for (int i = 2; i < a_strAddress.Split('.').Length; i++)
			{
				i_strMemberName = i_strMemberName + a_strAddress.Split('.')[i] + ".";
			}
			i_strMemberName = i_strMemberName.TrimEnd('.');
		}
	}

	private static string Fun_strGetCurrectName(string i_strVariable)
	{
		if (i_strVariable.StartsWith("Visu."))
		{
			i_strVariable = i_strVariable.Substring(5);
		}
		return i_strVariable;
	}

	private void Sub_InitializeGroup()
	{
		m_lstGroup.Clear();
		m_fdcGroup = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString())
		{
			RefreshTime = 400
		};
		m_fdcGroupTemp = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
		Sub_GroupEventRegister(m_fdcGroupTemp);
		m_fdcGroupEvent = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
		m_fdcGroupEvent.Error += Sub_GroupEventError;
		m_fdcGroupEvent.CollectionPropertyChanged += Sub_GroupPropertyChanged;
		m_fdcGroupEvent.RefreshTime = 100;
	}

	private void Sub_GroupPropertyChanged(object i_objSender, CollectionEventArgs i_fdcEventArgse)
	{
		m_edcLogger.Debug($"e.Objects.Count = {i_fdcEventArgse.Objects.Count}  CollectionPropertyChanged", null, "Sub_GroupPropertyChanged", 1623);
	}

	private VariableCollection Fun_fdcCreateVariableCollection(string i_strTaskName, int i_i32RefreshTime)
	{
		VariableCollection a_fdcCollection = new VariableCollection(m_fdcCpu, i_strTaskName + "_Test");
		a_fdcCollection.RefreshTime = i_i32RefreshTime;
		a_fdcCollection.ValueRead += Variable_ValueRead;
		a_fdcCollection.ValueChanged += VariableCollection_ValueChanged;
		a_fdcCollection.CollectionValuesRead += VariableCollection_ValuesRead;
		return a_fdcCollection;
	}

	private void VariableCollection_ValuesRead(object sender, CollectionEventArgs e)
	{
		VariableCollection variableCollection = sender as VariableCollection;
		string a_strTaskName = variableCollection.Name.ToString();
		m_edcLogger.Debug($"Sub_VariableCollection_ValuesRead Task:'{a_strTaskName}' VariableCollection Count:'{variableCollection.Count}'", null, "VariableCollection_ValuesRead", 1664);
		try
		{
			if (variableCollection == null)
			{
				return;
			}
			List<Edc_PLCElement> list = new List<Edc_PLCElement>();
			foreach (object item in e.Objects.Values)
			{
				Variable variable = (Variable)item;
				Edc_PLCElement spsElement = new Edc_PLCElement
				{
					Pro_strGroupName = a_strTaskName,
					Pro_strTask = a_strTaskName,
					Pro_strPVariable = variable.Name,
					Pro_strAddress = variable.FullName,
					Pro_objValue = variable.Value
				};
				list.Add(spsElement);
			}
		}
		catch (System.Exception ex)
		{
			m_edcLogger.Error("PLC -> " + ex.Message, null, "VariableCollection_ValuesRead", 1745);
		}
	}

	private VariableCollection Fun_fdcGetVariableCollection(string i_strGroupName)
	{
		VariableCollection a_fdcCollection = m_fdcCpu.Tasks[i_strGroupName].Variables;
		a_fdcCollection.CollectionError += Sub_PviCollectionError;
		a_fdcCollection.CollectionValuesRead += VariableCollection_ValuesRead;
		a_fdcCollection.CollectionConnected += VariableCollection_Connected;
		a_fdcCollection.CollectionDisconnected += VariableCollection_Disconnected;
		return a_fdcCollection;
	}

	private void Sub_GroupEventsRegister(VariableCollection i_fdcGroup)
	{
		i_fdcGroup.Error += Sub_PviCollectionError;
		i_fdcGroup.CollectionConnected += VariableCollection_Connected;
		i_fdcGroup.CollectionValuesRead += VariableCollection_ValuesRead;
	}

	private void Sub_GroupEventsDeRegister(VariableCollection i_fdcGroup)
	{
		i_fdcGroup.Error -= Sub_PviCollectionError;
		i_fdcGroup.CollectionConnected -= VariableCollection_Connected;
		i_fdcGroup.CollectionValuesRead -= VariableCollection_ValuesRead;
	}

	private void VariableCollection_ValueChanged(object sender, VariableEventArgs e)
	{
		Variable a_Variable = (Variable)sender;
		if (a_Variable.Value.DataType != DataType.Structure && a_Variable.Value.ArrayLength <= 1)
		{
		}
	}

	private void VariableCollection_Connected(object sender, CollectionEventArgs e)
	{
	}

	private void VariableCollection_Disconnected(object sender, CollectionEventArgs e)
	{
	}

	private void VariableCollection_Activated(object sender, CollectionEventArgs e)
	{
	}

	private void VariableCollection_Deactivated(object sender, CollectionEventArgs e)
	{
	}
}
