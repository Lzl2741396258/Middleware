using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BR.AN.PviServices;
using Ersa.Mes.Common.Extensions;
using Ersa.Mes.PLC;
using Ersa.Mes.PLC.Model;
using MesXPT.Model;

namespace MesXPT;

public class FrmTestPLC : Form, IDisposable
{
	public Service m_fdcService;

	public string Pro_strServiceName = "service";

	public Cpu m_fdcCpu;

	public string Pro_strCpuName = "CPU";

	public Dictionary<string, VariableCollection> m_dicGroup = new Dictionary<string, VariableCollection>();

	private readonly Dictionary<string, List<Edc_PLCElement>> m_dicSpsElement = new Dictionary<string, List<Edc_PLCElement>>();

	public VariableCollection m_fdcGroup;

	public VariableCollection m_fdcGroupTemp;

	public VariableCollection m_fdcGroupEvent;

	public Variable m_fdcVariable;

	private string _strConfigInitialize;

	private TaskCompletionSource<bool> m_fdcCpuCompletionSource;

	private TaskCompletionSource<bool> m_fdcTaskUploadCompletionSource;

	private TaskCompletionSource<bool> m_fdcVariableUploadCompletionSource;

	private TaskCompletionSource<bool> m_fdcVariableConnectCompletionSource;

	private TaskCompletionSource<bool> m_fdcVariableReadCompletionSource;

	public List<string> m_lstTaskDistinct = new List<string>();

	private TaskCompletionSource<bool> tcs;

	public bool m_blnBegin = false;

	public string m_strMessage = string.Empty;

	private string m_strReuslt = string.Empty;

	private IContainer components = null;

	private Button m_btnRead;

	private Button m_btnWrite;

	private TextBox m_txtReadTagName;

	private TextBox m_txtPlcIp;

	private TextBox m_i32PlcSlot;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label5;

	private TextBox m_txtValue;

	private Label label4;

	private TextBox m_txtWriteTagName;

	private Button m_btnConnect;

	private TextBox m_txtServiceName;

	private TextBox m_txtCpuName;

	private Label label6;

	private Label label7;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private RichTextBox m_txtInfo;

	private Label label8;

	private TextBox m_txtTask;

	private Label label9;

	private TextBox m_txtVariable;

	private Button m_btnGroupCreate;

	private Label label10;

	private TextBox m_txtGroupName;

	private Button m_btnGroupGet;

	private Button m_btnGroupValueRead;

	private Button m_btnGroupConnect;

	private Button m_btnRegister;

	private Button m_btnAddGroup;

	private Button m_btnDicConnect;

	private Button m_btnAddEventChange;

	private Label label11;

	private TextBox m_txtMember;

	private Button m_btnTaskUpload;

	private Button m_btnGroupValueChange;

	private Button m_btnEnd;

	private Button m_btnBegin;

	private Button m_btnShow;

	private Button m_btnClear;

	private XPT_Config m_Config { get; set; }

	private Inf_PLC Pro_edcSps { get; set; }

	public string m_strConfigInitialize
	{
		get
		{
			if (string.IsNullOrEmpty(_strConfigInitialize))
			{
				_strConfigInitialize = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Initialize_PLC.xml";
			}
			return _strConfigInitialize;
		}
		set
		{
			_strConfigInitialize = value;
		}
	}

	public FrmTestPLC()
	{
	}

	public FrmTestPLC(XPT_Config i_Config)
	{
		InitializeComponent();
		m_Config = i_Config;
	}

	private void m_btnConnect_Click(object sender, EventArgs e)
	{
		try
		{
			Pro_strServiceName = m_txtServiceName.Text;
			Pro_strCpuName = m_txtCpuName.Text;
			if (m_fdcService == null)
			{
				m_fdcService = new Service(Pro_strServiceName);
				m_fdcService.Error += Sub_PviError;
				m_fdcService.Connected += Sub_ServiceConnected;
			}
			m_fdcService.Connect();
		}
		catch (System.Exception)
		{
		}
	}

	private void m_btnDicConnect_Click(object sender, EventArgs e)
	{
		m_fdcCpu?.Disconnect();
		m_fdcService?.Disconnect();
		Sub_Show("Successed to disconnect to the PLC....");
	}

	public void Sub_ServiceConnected(object sender, PviEventArgs e)
	{
		Sub_CreateCpu();
		m_fdcCpu.Connect();
	}

	private void Sub_CreateCpu()
	{
		m_fdcCpu = new Cpu(m_fdcService, Pro_strCpuName);
		m_fdcCpu.Connection.DeviceType = DeviceType.AR000;
		m_fdcCpu.Connected += Sub_CpuConnected;
	}

	public void Sub_CpuConnected(object sender, PviEventArgs e)
	{
		if (e.ErrorCode == 0)
		{
			m_fdcCpuCompletionSource?.TrySetResult(result: true);
			Sub_Show("Successed to connect to the PLC....");
		}
		else
		{
			Sub_Show($"ErrorCode :'{e.ErrorCode}'  ErrorText:'{e.ErrorText}'");
		}
	}

	private async void m_btnTaskUpload_Click(object i_objSender, EventArgs i_fdcPviEventArgs)
	{
		await Sub_TaskUpload(i_objSender, i_fdcPviEventArgs);
	}

	private async System.Threading.Tasks.Task Sub_TaskUpload(object i_objSender, EventArgs i_fdcPviEventArgs)
	{
		m_fdcTaskUploadCompletionSource = new TaskCompletionSource<bool>();
		m_fdcCpu.Tasks.Uploaded -= Tasks_Uploaded;
		m_fdcCpu.Tasks.Uploaded += Tasks_Uploaded;
		m_fdcCpu.Tasks.Upload();
		await m_fdcTaskUploadCompletionSource.Task.Fun_fdcWithTimeout(2000, delegate
		{
			Sub_Show("Task upload timeout...");
		});
	}

	private void Tasks_Uploaded(object sender, PviEventArgs e)
	{
		string a_strTaskName = m_txtTask.Text;
		if (!string.IsNullOrEmpty(a_strTaskName))
		{
			m_lstTaskDistinct.Clear();
			m_lstTaskDistinct.Add(a_strTaskName);
			foreach (string MyTask in m_lstTaskDistinct)
			{
				if (m_fdcCpu.Tasks.ContainsKey(MyTask))
				{
					BR.AN.PviServices.Task a_Task = m_fdcCpu.Tasks[MyTask];
					if (a_Task != null)
					{
						a_Task.Connected += Task_Connected;
						a_Task.Connect();
					}
				}
			}
			Sub_Show("Task " + a_strTaskName + " Uploaded");
			return;
		}
		string a_strMessage = "Tasks_Uploaded\r\n";
		foreach (object item in m_fdcCpu.Tasks.Keys)
		{
			BR.AN.PviServices.Task task = m_fdcCpu.Tasks[item] as BR.AN.PviServices.Task;
			a_strMessage = a_strMessage + task.Name + "  ";
			task.Connected += Task_Connected;
			task.Connect();
		}
		Sub_Show(a_strMessage);
	}

	private void Task_Connected(object sender, PviEventArgs e)
	{
		BR.AN.PviServices.Task a_Task = (BR.AN.PviServices.Task)sender;
		a_Task.Variables.Uploaded += Sub_VariablesUploaded;
		a_Task.Variables.Upload();
	}

	private void Sub_VariablesUploaded(object sender, PviEventArgs e)
	{
		VariableCollection a_VariableCollection = sender as VariableCollection;
		BR.AN.PviServices.Task a_Task = (BR.AN.PviServices.Task)a_VariableCollection.Parent;
		string a_strTaskName = a_Task.Name;
		if (string.IsNullOrEmpty(m_txtVariable.Text))
		{
			string a_strMessage = string.Empty;
			foreach (object item in a_Task.Variables)
			{
				a_strMessage += $"{item}|";
			}
		}
		m_fdcTaskUploadCompletionSource.TrySetResult(result: true);
	}

	private void Variable_Connected(object sender, PviEventArgs e)
	{
		if (sender is Variable a_PVariable)
		{
			m_fdcVariable = a_PVariable;
		}
		m_fdcVariableConnectCompletionSource.TrySetResult(result: true);
	}

	private void VariableCollection_Connected(object sender, PviEventArgs e)
	{
	}

	private void m_btnGroupValueChange_Click(object sender, EventArgs e)
	{
		foreach (KeyValuePair<string, VariableCollection> item in m_dicGroup)
		{
			VariableCollection collection = item.Value;
			collection.RefreshTime = 30000;
			collection.Active = true;
			collection.Connect();
			collection.CollectionPropertyChanged += Value_CollectionPropertyChanged;
			collection.PropertyChanged += Value_PropertyChanged;
			collection.ValueChanged += Variable_ValueChanged;
		}
	}

	private void Value_PropertyChanged(object sender, PviEventArgs e)
	{
	}

	private void Value_CollectionPropertyChanged(object sender, CollectionEventArgs e)
	{
	}

	private async void m_btnRead_Click(object i_objSender, EventArgs i_fdcPviEventArgs)
	{
		Sub_SplitPlcAddress();
		string a_strTaskName = m_txtTask.Text;
		string a_strVariableName = m_txtVariable.Text;
		m_fdcVariable = await Fun_edcVariableGet(a_strTaskName, a_strVariableName);
		if (m_fdcVariable != null && m_fdcVariable.ErrorCode == 0)
		{
			m_fdcVariable.ValueRead -= Variable_ValueRead;
			m_fdcVariable.ValueRead += Variable_ValueRead;
			m_fdcVariable.ReadValue();
		}
	}

	private async Task<Variable> Fun_edcVariableGet(string i_strTaskName, string i_strVariableName)
	{
		if (m_fdcCpu == null || !m_fdcCpu.IsConnected)
		{
			return null;
		}
		if (m_fdcCpu.Tasks.Count == 0)
		{
			await Sub_TaskUpload(null, null);
		}
		Sub_Show($"Task {i_strTaskName} upload...Task Count {m_fdcCpu.Tasks.Count}");
		string a_strVariableName = m_txtVariable.Text;
		if (m_fdcCpu.Tasks[i_strTaskName].Variables == null || m_fdcCpu.Tasks[i_strTaskName].Variables.Count == 0)
		{
			m_fdcVariableUploadCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks[i_strTaskName].Variables.Uploaded += delegate
			{
				m_fdcVariableUploadCompletionSource.TrySetResult(result: true);
			};
			m_fdcCpu.Tasks[i_strTaskName].Variables.Upload();
			await m_fdcVariableUploadCompletionSource.Task.Fun_fdcWithTimeout(5000, delegate
			{
				Sub_Show("Variables upload timeout...");
			});
		}
		Variable variable = m_fdcCpu.Tasks[i_strTaskName].Variables[a_strVariableName];
		if (variable == null)
		{
			return null;
		}
		if (!variable.IsConnected)
		{
			m_fdcVariableConnectCompletionSource = new TaskCompletionSource<bool>();
			m_fdcCpu.Tasks[i_strTaskName].Variables[a_strVariableName].Connected += Variable_Connected;
			m_fdcCpu.Tasks[i_strTaskName].Variables[a_strVariableName].Connect();
			await m_fdcVariableConnectCompletionSource.Task.Fun_fdcWithTimeout(5000, delegate
			{
				Sub_Show("Variable " + i_strTaskName + "." + a_strVariableName + " connect timeout...");
			});
		}
		Sub_Show("Variable " + i_strTaskName + "." + a_strVariableName + " connected...");
		return variable;
	}

	private void m_btnWrite_Click(object sender, EventArgs e)
	{
		TaskCompletionSource<IEnumerable<Edc_PLCElement>> a_tcsTestCompletionSource = new TaskCompletionSource<IEnumerable<Edc_PLCElement>>();
		Variable variable = m_fdcVariable;
		variable.ValueWritten -= Variable_ValueWritten;
		variable.WriteValueAutomatic = false;
		variable.Value["sttTraceabilitySoll.bytAktivieren"] = 1;
		variable.WriteValue();
		variable.ValueWritten += Variable_ValueWritten;
		a_tcsTestCompletionSource.Task.Fun_fdcTimeoutAfterAsync(5000).ConfigureAwait(continueOnCapturedContext: true);
	}

	private void Variable_ValueWritten(object sender, PviEventArgs e)
	{
		Variable variable = (Variable)sender;
	}

	private void m_btnAddEventChange_Click(object sender, EventArgs e)
	{
		m_fdcVariable.ValueChanged -= Variable_ValueChanged;
		m_fdcVariable.ValueChanged += Variable_ValueChanged;
		m_fdcVariable.Active = true;
	}

	private void Variable_ValueChanged(object sender, VariableEventArgs e)
	{
		Variable variable = sender as Variable;
		if (!m_blnBegin)
		{
			return;
		}
		if (variable.ChangedStructMembers.Length == 0)
		{
			m_strMessage += $"{variable.FullName}  {variable.Value}\r\n";
			return;
		}
		string[] changedStructMembers = variable.ChangedStructMembers;
		foreach (string item in changedStructMembers)
		{
			m_strMessage += string.Format("{0}  {1}\r\n", variable.FullName + "." + item, variable.Value[item]);
		}
	}

	private void m_btnBegin_Click(object sender, EventArgs e)
	{
		m_strMessage = string.Empty;
		System.Threading.Tasks.Task.Run(delegate
		{
			m_blnBegin = true;
			Thread.Sleep(2000);
			m_blnBegin = false;
		});
	}

	private void m_btnEnd_Click(object sender, EventArgs e)
	{
		m_blnBegin = false;
	}

	private void m_btnShow_Click(object sender, EventArgs e)
	{
		Sub_Show(m_strMessage);
	}

	private void Variable_ValueRead(object sender, PviEventArgs e)
	{
		Variable a_PVariable = (Variable)sender;
		BR.AN.PviServices.Task a_Task = null;
		if (a_PVariable.Parent.Address.Equals("CPU"))
		{
			Variable test1 = a_PVariable;
			a_Task = m_fdcCpu.Tasks[a_PVariable.Name];
		}
		else
		{
			a_Task = (BR.AN.PviServices.Task)a_PVariable.Parent;
		}
		string a_strKey = a_Task.Name + "." + a_PVariable.Name;
		string a_strVallue = ((Variable)sender).Value.ToString();
		string a_strMemberName = m_txtMember.Text;
		if (!a_PVariable.ExpandMembers)
		{
			Sub_Show($"{DateTime.Now} -- {a_PVariable.FullName} -- {a_PVariable.Value} -- Error:{a_PVariable.ErrorCode} -- ErrorText:{a_PVariable.ErrorText}");
			return;
		}
		m_strReuslt = string.Empty;
		m_strReuslt = Fun_strGetAllValue(a_strKey, a_Task.Name, a_PVariable.Name, a_PVariable);
		m_strReuslt = m_strReuslt.TrimEnd('\n');
		m_strReuslt = m_strReuslt.TrimEnd('\r');
		Sub_Show("===================== Begin ===================== ");
		Sub_Show(m_strReuslt);
		Sub_Show("===================== End ===================== ");
		if (!string.IsNullOrEmpty(a_strMemberName))
		{
			Sub_Show(string.Format("{0} -- {1} -- {2} -- Error:{3} -- ErrorText:{4}", DateTime.Now, a_PVariable.FullName + "." + a_strMemberName, a_PVariable.Value[a_strMemberName], a_PVariable.ErrorCode, a_PVariable.ErrorText));
		}
	}

	private string Fun_strGetAllValue(string i_strKey, string i_strTaskName, string i_strPVariable, Variable variable)
	{
		if (variable == null)
		{
			return string.Empty;
		}
		if (variable.Members == null)
		{
			m_strReuslt += $"{i_strKey}.{variable.StructMemberName}||{variable.Value}\r\n";
		}
		else
		{
			foreach (object item in variable.Members)
			{
				Fun_strGetAllValue(i_strKey, i_strTaskName, i_strPVariable, (Variable)item);
			}
		}
		return m_strReuslt;
	}

	private void m_btnGroupGet_Click(object sender, EventArgs e)
	{
		string a_strGroupName = m_txtGroupName.Text;
		VariableCollection variableCollection = m_dicGroup.Where((KeyValuePair<string, VariableCollection> s) => s.Key.Equals(a_strGroupName + ".Variables")).FirstOrDefault().Value;
		Sub_Show($"GroupGet {variableCollection.Name}");
	}

	private void m_btnGroupCreate_Click(object sender, EventArgs e)
	{
		m_dicGroup.Clear();
		m_fdcGroup = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString())
		{
			RefreshTime = 5000
		};
		m_fdcGroupTemp = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString())
		{
			RefreshTime = 5000
		};
		m_fdcGroupEvent = new VariableCollection(m_fdcCpu, Guid.NewGuid().ToString());
		m_fdcGroupEvent.Error += Sub_PviCollectionError;
		m_fdcGroupEvent.CollectionConnected += Sub_CollectionConnected;
		m_fdcGroupEvent.ValueRead += Sub_GroupValuesRead;
		Sub_Show("GroupCreate ok...");
	}

	private async void m_btnGroupVariableAdd_Click(object sender, EventArgs e)
	{
		string a_strTaskName = m_txtTask.Text;
		string a_strVariableName = m_txtVariable.Text;
		Variable variable = await Fun_edcVariableGet(a_strTaskName, a_strVariableName);
		if (variable != null)
		{
			variable.Active = true;
			m_fdcGroupTemp?.Add(variable);
		}
	}

	private async void m_btnGroupValueRead_Click(object sender, EventArgs e)
	{
		m_fdcVariableReadCompletionSource = new TaskCompletionSource<bool>();
		m_fdcGroupTemp.ValueRead -= Sub_GroupValuesRead;
		m_fdcGroupTemp.ReadValues();
		m_fdcGroupTemp.ValueRead += Sub_GroupValuesRead;
		await m_fdcVariableReadCompletionSource.Task.Fun_fdcWithTimeout(5000, delegate
		{
			Sub_Show("m_fdcGroupTemp read timeout");
		});
	}

	private void Sub_PviCollectionError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
	{
		string i_strMessage = "PLC -> Collection Error: " + i_fdcPviEventArgs.Address + " --- " + i_fdcPviEventArgs.ErrorText;
		Sub_Show(i_strMessage);
		if (i_fdcPviEventArgs.Action == BR.AN.PviServices.Action.VariableDisconnect)
		{
		}
	}

	private void Sub_CollectionConnected(object i_objSender, CollectionEventArgs i_fdcCollectionEventArgs)
	{
		if (i_objSender is VariableCollection variableCollection)
		{
			variableCollection.ReadValues();
			variableCollection.Active = true;
		}
	}

	private void Sub_GroupValuesRead(object i_objSender, PviEventArgs e)
	{
		if (i_objSender is VariableCollection variableCollection)
		{
			if (variableCollection.Name.Equals("convey01"))
			{
				Sub_Show($"{variableCollection.Name} read: {variableCollection.Count} variables");
			}
			if (variableCollection.Name.Equals("bde_data"))
			{
				Sub_Show($"{variableCollection.Name} read: {variableCollection.Count} variables");
			}
		}
	}

	private void m_btnRegister_Click(object sender, EventArgs e)
	{
		m_fdcVariableConnectCompletionSource = new TaskCompletionSource<bool>();
	}

	public void Sub_PviError(object i_objSender, PviEventArgs i_fdcPviEventArgs)
	{
		string a_strMessage = $"PLC Error...ErrorCode:'{i_fdcPviEventArgs.ErrorCode}'  ErrorText:'{i_fdcPviEventArgs.ErrorText}'";
	}

	private void Sub_Show(string i_strMessage)
	{
		Thread.Sleep(100);
		Invoke((System.Action)delegate
		{
			RichTextBox txtInfo = m_txtInfo;
			txtInfo.Text = txtInfo.Text + i_strMessage + "\r\n";
			m_txtInfo.SelectionStart = m_txtInfo.Text.Length;
			m_txtInfo.ScrollToCaret();
		});
	}

	private void m_txtReadTagName_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r')
		{
			Sub_SplitPlcAddress();
		}
	}

	private void Sub_SplitPlcAddress()
	{
		Sub_SplitPlcAddress(m_txtReadTagName.Text, out var i_strTaskName, out var i_strPVariableName, out var i_strMemberName);
		m_txtTask.Text = i_strTaskName;
		m_txtVariable.Text = i_strPVariableName;
		m_txtMember.Text = i_strMemberName;
	}

	private void Sub_SplitPlcAddress(string i_strAddress, out string i_strTaskName, out string i_strPVariableName, out string i_strMemberName)
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

	private string Fun_strGetCurrectName(string i_strVariable)
	{
		if (i_strVariable.StartsWith("Visu."))
		{
			i_strVariable = i_strVariable.Substring(5);
		}
		return i_strVariable;
	}

	private void m_btnClear_Click(object sender, EventArgs e)
	{
		m_txtInfo.Text = string.Empty;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.m_btnRead = new System.Windows.Forms.Button();
		this.m_btnWrite = new System.Windows.Forms.Button();
		this.m_txtReadTagName = new System.Windows.Forms.TextBox();
		this.m_txtPlcIp = new System.Windows.Forms.TextBox();
		this.m_i32PlcSlot = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.m_txtValue = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.m_txtWriteTagName = new System.Windows.Forms.TextBox();
		this.m_btnConnect = new System.Windows.Forms.Button();
		this.m_txtServiceName = new System.Windows.Forms.TextBox();
		this.m_txtCpuName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_btnShow = new System.Windows.Forms.Button();
		this.m_btnEnd = new System.Windows.Forms.Button();
		this.m_btnBegin = new System.Windows.Forms.Button();
		this.m_btnGroupValueChange = new System.Windows.Forms.Button();
		this.m_btnTaskUpload = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.m_txtMember = new System.Windows.Forms.TextBox();
		this.m_btnAddEventChange = new System.Windows.Forms.Button();
		this.m_btnDicConnect = new System.Windows.Forms.Button();
		this.m_btnAddGroup = new System.Windows.Forms.Button();
		this.m_btnRegister = new System.Windows.Forms.Button();
		this.m_btnGroupGet = new System.Windows.Forms.Button();
		this.m_btnGroupValueRead = new System.Windows.Forms.Button();
		this.m_btnGroupConnect = new System.Windows.Forms.Button();
		this.m_btnGroupCreate = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.m_txtGroupName = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.m_txtVariable = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.m_txtTask = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.m_txtInfo = new System.Windows.Forms.RichTextBox();
		this.m_btnClear = new System.Windows.Forms.Button();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		base.SuspendLayout();
		this.m_btnRead.Location = new System.Drawing.Point(937, 29);
		this.m_btnRead.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnRead.Name = "m_btnRead";
		this.m_btnRead.Size = new System.Drawing.Size(107, 30);
		this.m_btnRead.TabIndex = 0;
		this.m_btnRead.Text = "Read";
		this.m_btnRead.UseVisualStyleBackColor = true;
		this.m_btnRead.Click += new System.EventHandler(m_btnRead_Click);
		this.m_btnWrite.Location = new System.Drawing.Point(937, 69);
		this.m_btnWrite.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnWrite.Name = "m_btnWrite";
		this.m_btnWrite.Size = new System.Drawing.Size(107, 30);
		this.m_btnWrite.TabIndex = 1;
		this.m_btnWrite.Text = "Write";
		this.m_btnWrite.UseVisualStyleBackColor = true;
		this.m_btnWrite.Click += new System.EventHandler(m_btnWrite_Click);
		this.m_txtReadTagName.Location = new System.Drawing.Point(110, 29);
		this.m_txtReadTagName.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtReadTagName.Name = "m_txtReadTagName";
		this.m_txtReadTagName.Size = new System.Drawing.Size(514, 26);
		this.m_txtReadTagName.TabIndex = 2;
		this.m_txtReadTagName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(m_txtReadTagName_KeyPress);
		this.m_txtPlcIp.Location = new System.Drawing.Point(110, 30);
		this.m_txtPlcIp.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtPlcIp.Name = "m_txtPlcIp";
		this.m_txtPlcIp.Size = new System.Drawing.Size(225, 26);
		this.m_txtPlcIp.TabIndex = 4;
		this.m_txtPlcIp.Text = "127.0.0.1";
		this.m_i32PlcSlot.Location = new System.Drawing.Point(389, 28);
		this.m_i32PlcSlot.Margin = new System.Windows.Forms.Padding(4);
		this.m_i32PlcSlot.Name = "m_i32PlcSlot";
		this.m_i32PlcSlot.Size = new System.Drawing.Size(225, 26);
		this.m_i32PlcSlot.TabIndex = 5;
		this.m_i32PlcSlot.Text = "0";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(44, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 16);
		this.label1.TabIndex = 7;
		this.label1.Text = "PlcIp";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(342, 33);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(40, 16);
		this.label2.TabIndex = 8;
		this.label2.Text = "Port";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(39, 36);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 16);
		this.label3.TabIndex = 9;
		this.label3.Text = "TagName";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(638, 72);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 16);
		this.label5.TabIndex = 12;
		this.label5.Text = "Value";
		this.m_txtValue.Location = new System.Drawing.Point(704, 69);
		this.m_txtValue.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtValue.Name = "m_txtValue";
		this.m_txtValue.Size = new System.Drawing.Size(225, 26);
		this.m_txtValue.TabIndex = 11;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(39, 72);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 16);
		this.label4.TabIndex = 14;
		this.label4.Text = "TagName";
		this.m_txtWriteTagName.Location = new System.Drawing.Point(110, 69);
		this.m_txtWriteTagName.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtWriteTagName.Name = "m_txtWriteTagName";
		this.m_txtWriteTagName.Size = new System.Drawing.Size(514, 26);
		this.m_txtWriteTagName.TabIndex = 13;
		this.m_btnConnect.Location = new System.Drawing.Point(622, 28);
		this.m_btnConnect.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnConnect.Name = "m_btnConnect";
		this.m_btnConnect.Size = new System.Drawing.Size(98, 60);
		this.m_btnConnect.TabIndex = 17;
		this.m_btnConnect.Text = "Connect";
		this.m_btnConnect.UseVisualStyleBackColor = true;
		this.m_btnConnect.Click += new System.EventHandler(m_btnConnect_Click);
		this.m_txtServiceName.Location = new System.Drawing.Point(110, 64);
		this.m_txtServiceName.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtServiceName.Name = "m_txtServiceName";
		this.m_txtServiceName.Size = new System.Drawing.Size(225, 26);
		this.m_txtServiceName.TabIndex = 18;
		this.m_txtServiceName.Text = "service";
		this.m_txtCpuName.Location = new System.Drawing.Point(389, 62);
		this.m_txtCpuName.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtCpuName.Name = "m_txtCpuName";
		this.m_txtCpuName.Size = new System.Drawing.Size(225, 26);
		this.m_txtCpuName.TabIndex = 19;
		this.m_txtCpuName.Text = "CPU";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(44, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(64, 16);
		this.label6.TabIndex = 20;
		this.label6.Text = "Service";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(350, 67);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(32, 16);
		this.label7.TabIndex = 21;
		this.label7.Text = "Cpu";
		this.groupBox1.Controls.Add(this.m_btnClear);
		this.groupBox1.Controls.Add(this.m_btnShow);
		this.groupBox1.Controls.Add(this.m_btnEnd);
		this.groupBox1.Controls.Add(this.m_btnBegin);
		this.groupBox1.Controls.Add(this.m_btnGroupValueChange);
		this.groupBox1.Controls.Add(this.m_btnTaskUpload);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.m_txtMember);
		this.groupBox1.Controls.Add(this.m_btnAddEventChange);
		this.groupBox1.Controls.Add(this.m_btnDicConnect);
		this.groupBox1.Controls.Add(this.m_btnAddGroup);
		this.groupBox1.Controls.Add(this.m_btnRegister);
		this.groupBox1.Controls.Add(this.m_btnGroupGet);
		this.groupBox1.Controls.Add(this.m_btnGroupValueRead);
		this.groupBox1.Controls.Add(this.m_btnGroupConnect);
		this.groupBox1.Controls.Add(this.m_btnGroupCreate);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.m_txtGroupName);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.m_txtVariable);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.m_txtTask);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.m_txtPlcIp);
		this.groupBox1.Controls.Add(this.m_txtCpuName);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.m_btnConnect);
		this.groupBox1.Controls.Add(this.m_txtServiceName);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.m_i32PlcSlot);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1072, 294);
		this.groupBox1.TabIndex = 22;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Connect";
		this.m_btnShow.Location = new System.Drawing.Point(577, 170);
		this.m_btnShow.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnShow.Name = "m_btnShow";
		this.m_btnShow.Size = new System.Drawing.Size(109, 30);
		this.m_btnShow.TabIndex = 41;
		this.m_btnShow.Text = "Show";
		this.m_btnShow.UseVisualStyleBackColor = true;
		this.m_btnShow.Click += new System.EventHandler(m_btnShow_Click);
		this.m_btnEnd.Location = new System.Drawing.Point(460, 170);
		this.m_btnEnd.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnEnd.Name = "m_btnEnd";
		this.m_btnEnd.Size = new System.Drawing.Size(109, 30);
		this.m_btnEnd.TabIndex = 40;
		this.m_btnEnd.Text = "End";
		this.m_btnEnd.UseVisualStyleBackColor = true;
		this.m_btnEnd.Click += new System.EventHandler(m_btnEnd_Click);
		this.m_btnBegin.Location = new System.Drawing.Point(343, 170);
		this.m_btnBegin.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnBegin.Name = "m_btnBegin";
		this.m_btnBegin.Size = new System.Drawing.Size(109, 30);
		this.m_btnBegin.TabIndex = 39;
		this.m_btnBegin.Text = "Begin";
		this.m_btnBegin.UseVisualStyleBackColor = true;
		this.m_btnBegin.Click += new System.EventHandler(m_btnBegin_Click);
		this.m_btnGroupValueChange.Location = new System.Drawing.Point(811, 132);
		this.m_btnGroupValueChange.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnGroupValueChange.Name = "m_btnGroupValueChange";
		this.m_btnGroupValueChange.Size = new System.Drawing.Size(123, 30);
		this.m_btnGroupValueChange.TabIndex = 38;
		this.m_btnGroupValueChange.Text = "G_ValueChanged";
		this.m_btnGroupValueChange.UseVisualStyleBackColor = true;
		this.m_btnGroupValueChange.Click += new System.EventHandler(m_btnGroupValueChange_Click);
		this.m_btnTaskUpload.Location = new System.Drawing.Point(343, 96);
		this.m_btnTaskUpload.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnTaskUpload.Name = "m_btnTaskUpload";
		this.m_btnTaskUpload.Size = new System.Drawing.Size(343, 30);
		this.m_btnTaskUpload.TabIndex = 37;
		this.m_btnTaskUpload.Text = "T_Upload|Connect|VariablesUploaded";
		this.m_btnTaskUpload.UseVisualStyleBackColor = true;
		this.m_btnTaskUpload.Click += new System.EventHandler(m_btnTaskUpload_Click);
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(47, 249);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(56, 16);
		this.label11.TabIndex = 36;
		this.label11.Text = "Member";
		this.m_txtMember.Location = new System.Drawing.Point(110, 246);
		this.m_txtMember.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtMember.Name = "m_txtMember";
		this.m_txtMember.Size = new System.Drawing.Size(225, 26);
		this.m_txtMember.TabIndex = 35;
		this.m_btnAddEventChange.Location = new System.Drawing.Point(577, 208);
		this.m_btnAddEventChange.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnAddEventChange.Name = "m_btnAddEventChange";
		this.m_btnAddEventChange.Size = new System.Drawing.Size(109, 30);
		this.m_btnAddEventChange.TabIndex = 34;
		this.m_btnAddEventChange.Text = "E_Changed";
		this.m_btnAddEventChange.UseVisualStyleBackColor = true;
		this.m_btnAddEventChange.Click += new System.EventHandler(m_btnAddEventChange_Click);
		this.m_btnDicConnect.Location = new System.Drawing.Point(728, 28);
		this.m_btnDicConnect.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnDicConnect.Name = "m_btnDicConnect";
		this.m_btnDicConnect.Size = new System.Drawing.Size(98, 60);
		this.m_btnDicConnect.TabIndex = 33;
		this.m_btnDicConnect.Text = "DisConnect";
		this.m_btnDicConnect.UseVisualStyleBackColor = true;
		this.m_btnDicConnect.Click += new System.EventHandler(m_btnDicConnect_Click);
		this.m_btnAddGroup.Location = new System.Drawing.Point(343, 208);
		this.m_btnAddGroup.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnAddGroup.Name = "m_btnAddGroup";
		this.m_btnAddGroup.Size = new System.Drawing.Size(109, 30);
		this.m_btnAddGroup.TabIndex = 32;
		this.m_btnAddGroup.Text = "FillGroup";
		this.m_btnAddGroup.UseVisualStyleBackColor = true;
		this.m_btnAddGroup.Click += new System.EventHandler(m_btnGroupVariableAdd_Click);
		this.m_btnRegister.Location = new System.Drawing.Point(460, 208);
		this.m_btnRegister.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnRegister.Name = "m_btnRegister";
		this.m_btnRegister.Size = new System.Drawing.Size(109, 30);
		this.m_btnRegister.TabIndex = 31;
		this.m_btnRegister.Text = "V_Register";
		this.m_btnRegister.UseVisualStyleBackColor = true;
		this.m_btnRegister.Click += new System.EventHandler(m_btnRegister_Click);
		this.m_btnGroupGet.Location = new System.Drawing.Point(460, 132);
		this.m_btnGroupGet.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnGroupGet.Name = "m_btnGroupGet";
		this.m_btnGroupGet.Size = new System.Drawing.Size(109, 30);
		this.m_btnGroupGet.TabIndex = 30;
		this.m_btnGroupGet.Text = "G_Get";
		this.m_btnGroupGet.UseVisualStyleBackColor = true;
		this.m_btnGroupGet.Click += new System.EventHandler(m_btnGroupGet_Click);
		this.m_btnGroupValueRead.Location = new System.Drawing.Point(694, 132);
		this.m_btnGroupValueRead.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnGroupValueRead.Name = "m_btnGroupValueRead";
		this.m_btnGroupValueRead.Size = new System.Drawing.Size(109, 30);
		this.m_btnGroupValueRead.TabIndex = 29;
		this.m_btnGroupValueRead.Text = "G_ValueRead";
		this.m_btnGroupValueRead.UseVisualStyleBackColor = true;
		this.m_btnGroupValueRead.Click += new System.EventHandler(m_btnGroupValueRead_Click);
		this.m_btnGroupConnect.Location = new System.Drawing.Point(577, 132);
		this.m_btnGroupConnect.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnGroupConnect.Name = "m_btnGroupConnect";
		this.m_btnGroupConnect.Size = new System.Drawing.Size(109, 30);
		this.m_btnGroupConnect.TabIndex = 28;
		this.m_btnGroupConnect.Text = "G_Connect";
		this.m_btnGroupConnect.UseVisualStyleBackColor = true;
		this.m_btnGroupCreate.Location = new System.Drawing.Point(343, 132);
		this.m_btnGroupCreate.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnGroupCreate.Name = "m_btnGroupCreate";
		this.m_btnGroupCreate.Size = new System.Drawing.Size(109, 30);
		this.m_btnGroupCreate.TabIndex = 15;
		this.m_btnGroupCreate.Text = "G_Create";
		this.m_btnGroupCreate.UseVisualStyleBackColor = true;
		this.m_btnGroupCreate.Click += new System.EventHandler(m_btnGroupCreate_Click);
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(44, 135);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(48, 16);
		this.label10.TabIndex = 27;
		this.label10.Text = "Group";
		this.m_txtGroupName.Location = new System.Drawing.Point(110, 132);
		this.m_txtGroupName.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtGroupName.Name = "m_txtGroupName";
		this.m_txtGroupName.Size = new System.Drawing.Size(225, 26);
		this.m_txtGroupName.TabIndex = 26;
		this.m_txtGroupName.Text = "Test";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(31, 217);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(72, 16);
		this.label9.TabIndex = 25;
		this.label9.Text = "Variable";
		this.m_txtVariable.Location = new System.Drawing.Point(110, 212);
		this.m_txtVariable.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtVariable.Name = "m_txtVariable";
		this.m_txtVariable.Size = new System.Drawing.Size(225, 26);
		this.m_txtVariable.TabIndex = 24;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(44, 101);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(40, 16);
		this.label8.TabIndex = 23;
		this.label8.Text = "Task";
		this.m_txtTask.Location = new System.Drawing.Point(110, 98);
		this.m_txtTask.Margin = new System.Windows.Forms.Padding(4);
		this.m_txtTask.Name = "m_txtTask";
		this.m_txtTask.Size = new System.Drawing.Size(225, 26);
		this.m_txtTask.TabIndex = 22;
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.m_btnRead);
		this.groupBox2.Controls.Add(this.m_btnWrite);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.m_txtReadTagName);
		this.groupBox2.Controls.Add(this.m_txtWriteTagName);
		this.groupBox2.Controls.Add(this.m_txtValue);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(0, 294);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1072, 119);
		this.groupBox2.TabIndex = 23;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Function";
		this.groupBox3.Controls.Add(this.m_txtInfo);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox3.Location = new System.Drawing.Point(0, 413);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1072, 554);
		this.groupBox3.TabIndex = 24;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Info";
		this.m_txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_txtInfo.Font = new System.Drawing.Font("宋体", 9f);
		this.m_txtInfo.Location = new System.Drawing.Point(3, 22);
		this.m_txtInfo.Name = "m_txtInfo";
		this.m_txtInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
		this.m_txtInfo.Size = new System.Drawing.Size(1066, 529);
		this.m_txtInfo.TabIndex = 17;
		this.m_txtInfo.Text = "";
		this.m_btnClear.Location = new System.Drawing.Point(937, 246);
		this.m_btnClear.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnClear.Name = "m_btnClear";
		this.m_btnClear.Size = new System.Drawing.Size(107, 30);
		this.m_btnClear.TabIndex = 15;
		this.m_btnClear.Text = "Clear";
		this.m_btnClear.UseVisualStyleBackColor = true;
		this.m_btnClear.Click += new System.EventHandler(m_btnClear_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1072, 967);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "FrmTestPLC";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "PLC Test";
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
