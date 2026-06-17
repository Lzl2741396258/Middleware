using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.FileSystem.Language;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Helper;
using Ersa.Mes.Middleware.Interface;
using Ersa.Mes.Middleware.Model;
using Ersa.Mes.Middleware.Properties;

namespace Ersa.Mes.Middleware;

public class P_frmMain : Form
{
	private Edc_ConfigBase m_ConfigBase;

	protected Inf_MesTaskContainer m_edcTaskContainer;

	private bool _blnFrmLock = false;

	public Edc_Account _edcAccount;

	private bool _blnLogin = false;

	private bool m_blnLampErsa = false;

	private bool m_blnLampPLC = false;

	private bool m_blnInitialize = true;

	private IContainer components = null;

	private ToolStrip toolStrip1;

	public ToolStripButton m_tsbtnSettingCustom;

	private ToolStripSeparator toolStripSeparator1;

	public ToolStripButton m_tsbtnStart;

	public ToolStripButton m_tsbtnClear;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private StatusStrip statusStrip1;

	public GroupBox groupBox1;

	private ObjectListView m_olvInfo;

	private ToolStripSeparator toolStripSeparator6;

	private System.Windows.Forms.Timer timer1;

	private ToolStripStatusLabel m_tsslbErsasoft;

	private ToolStripStatusLabel m_tsslbEmpty1;

	private ToolStripStatusLabel m_tsslbOEE;

	private ToolStripStatusLabel m_tsslbEmpty2;

	public ToolStripButton m_tsbtnReport;

	public ToolStripButton m_tsbtnPLC;

	private ToolStripSeparator toolStripSeparator4;

	public ToolStripButton m_tsbtnSettingErsa;

	public ToolStripButton m_tsbtnLock;

	private NotifyIcon notifyIcon1;

	public ToolStripButton m_tsbtnRecipe;

	public ToolStripButton m_tsbtnErsaData;

	public ToolStripButton m_tsbtnPLCData;

	private ToolStripStatusLabel m_tsslbPLC;

	private ToolStripStatusLabel m_tsslbEmpty3;

	public ToolStripButton m_tsbtnUser;

	public ToolStripButton m_tsbtnEngineer;

	public ToolStripButton m_tsbtnPlanDown;

	public ToolStripButton m_tsbtnCPK;

	public ToolStripButton m_tsbtnComfirm;

	private ToolStripStatusLabel m_tsslbLampErsa;

	private ToolStripStatusLabel m_tsslbLampPLC;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripStatusLabel m_tsslbAccount;

	private ToolStripStatusLabel toolStripStatusLabel2;

	public Inf_Logger m_edcLogger { get; set; }

	private bool m_blnFrmLock
	{
		get
		{
			return _blnFrmLock;
		}
		set
		{
			if (_blnFrmLock)
			{
				m_tsbtnLock.Image = Resources._lock;
			}
			else
			{
				m_tsbtnLock.Image = Resources._unlock;
			}
			_blnFrmLock = value;
		}
	}

	public Edc_Account m_edcAccount
	{
		get
		{
			return _edcAccount;
		}
		set
		{
			if (value != null && value.m_strName != null)
			{
				m_tsslbAccount.Text = $"Account:{value.m_strName}";
			}
			_edcAccount = value;
		}
	}

	protected P_frmUserAuthentication m_frmUserAuthentication { get; set; } = new P_frmUserAuthentication();


	private bool m_blnLogin
	{
		get
		{
			return _blnLogin;
		}
		set
		{
			if (_blnLogin)
			{
				m_tsbtnUser.Text = "Login";
				m_tsbtnUser.Image = Resources._user;
			}
			else
			{
				m_tsbtnUser.Text = "Logout";
				m_tsbtnUser.Image = Resources.PNG_Benutzer_angemeldet_54x54;
			}
			_blnLogin = value;
		}
	}

	public P_frmMain()
	{
		InitializeComponent();
	}

	public P_frmMain(Edc_ConfigBase i_ConfigBase, Inf_Logger i_Logger, Enum_Language i_enuLanguage = Enum_Language.EN)
	{
		InitializeComponent();
		m_ConfigBase = i_ConfigBase;
		m_edcLogger = i_Logger;
		if (!i_ConfigBase.m_clsBasicSettings.m_blnShowLamp)
		{
			m_tsslbLampErsa.Visible = false;
			m_tsslbLampPLC.Visible = false;
		}
	}

	private void FrmParent_Load(object sender, EventArgs e)
	{
		try
		{
			Sub_InitializeOlv();
			Sub_InitializePLC();
		}
		catch (Exception ex)
		{
			Sub_ShowMessage(Enum_LogType.Error, "An unexpected error has accourred during initialization...Details:'" + ex.Message + "'");
		}
	}

	private void FrmMainParent_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (m_ConfigBase.m_clsBasicSettings.m_blnExitVerification)
		{
			if (m_frmUserAuthentication.ShowDialog() == DialogResult.OK)
			{
				e.Cancel = !m_frmUserAuthentication.m_blnVerify;
			}
			else
			{
				e.Cancel = true;
			}
		}
		else
		{
			Application.Exit();
		}
	}

	private void FrmMainChild_FormClosing(object sender, FormClosingEventArgs e)
	{
		e.Cancel = true;
	}

	private void P_frmMain_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void FrmMainParent_Shown(object sender, EventArgs e)
	{
		try
		{
			if (!m_ConfigBase.m_clsBasicSettings.m_blnAutoFunction || m_ConfigBase.m_clsBasicSettings.m_i32AutoFunctionDelay <= 0)
			{
				return;
			}
			Task.Run(delegate
			{
				Thread.Sleep(m_ConfigBase.m_clsBasicSettings.m_i32AutoFunctionDelay);
				if (!m_edcTaskContainer.m_blnTaskStart)
				{
					m_tsbtnStart_Click(null, null);
				}
			});
		}
		catch (Exception)
		{
		}
	}

	protected virtual void Sub_ControlTsbtn(bool i_blnRecipe, bool i_blnPLC, bool i_blnReport, bool i_blnErsaData, bool i_blnPlcData, bool i_blnEngineer = false, bool i_blnPlanDown = false)
	{
		if (!i_blnRecipe)
		{
			m_tsbtnRecipe.Visible = false;
		}
		if (!i_blnPLC)
		{
			m_tsbtnPLC.Visible = false;
		}
		if (!i_blnReport)
		{
			m_tsbtnReport.Visible = false;
		}
		if (!i_blnErsaData)
		{
			m_tsbtnErsaData.Visible = false;
		}
		if (!i_blnPlcData)
		{
			m_tsbtnPLCData.Visible = false;
		}
		if (!i_blnEngineer)
		{
			m_tsbtnEngineer.Visible = false;
		}
		if (!i_blnPlanDown)
		{
			m_tsbtnPlanDown.Visible = false;
		}
	}

	protected virtual void Sub_ControlTsbtn(int i_i32MainformLevel)
	{
		string a_strLevel = Convert.ToString(i_i32MainformLevel, 2).PadLeft(8, '0');
		int a_i32Index = 0;
		m_tsbtnRecipe.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnPLC.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnReport.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnCPK.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnErsaData.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnPLCData.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnEngineer.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
		a_i32Index++;
		m_tsbtnPlanDown.Visible = a_strLevel.Substring(a_i32Index, 1) == "1";
	}

	private void Sub_InitializeOlv()
	{
		Edc_OlvHelper.Fun_olvMessage(m_olvInfo);
		Edc_OlvHelper.Sub_Show(m_olvInfo, Enum_LogType.Info, "The configuration file has been read successfully.");
	}

	public virtual void Sub_InitializePLC()
	{
	}

	public virtual void Sub_InitializeHardware()
	{
	}

	protected virtual void Sub_InitializeMesTask()
	{
	}

	protected virtual void m_tsbtnStart_Click(object sender, EventArgs e)
	{
		if (m_blnInitialize)
		{
			Sub_InitializeMesTask();
			m_blnInitialize = false;
		}
		if (!m_edcTaskContainer.m_blnTaskStart)
		{
			Sub_TaskStart();
		}
		else
		{
			Sub_TaskStop();
		}
	}

	protected void m_tsbtnClear_Click(object sender, EventArgs e)
	{
		m_olvInfo.Items.Clear();
	}

	protected virtual void m_tsbtnErsa_Click(object sender, EventArgs e)
	{
		Sub_ShowMessage(Enum_LogType.Info, "Please restart the software after modifying the settings.");
		m_edcLogger.Debug(MethodBase.GetCurrentMethod().Name + "  Ersa Setting", null, "m_tsbtnErsa_Click", 315);
	}

	protected virtual void m_tsbtnCustom_Click(object sender, EventArgs e)
	{
		Sub_ShowMessage(Enum_LogType.Info, "Please restart the software after modifying the settings.");
		m_edcLogger.Debug(MethodBase.GetCurrentMethod().Name + "  Custom Setting", null, "m_tsbtnCustom_Click", 326);
	}

	protected virtual void m_tsbtnRecipe_Click(object sender, EventArgs e)
	{
	}

	protected virtual void m_tsbtnPLC_Click(object sender, EventArgs e)
	{
	}

	protected virtual void m_tsbtnReport_Click(object sender, EventArgs e)
	{
	}

	protected virtual void m_tsbtnParameter_Click(object sender, EventArgs e)
	{
	}

	protected virtual void m_tsbtnCPK_Click(object sender, EventArgs e)
	{
	}

	protected virtual void m_tsbtnPLCData_Click(object sender, EventArgs e)
	{
	}

	protected virtual void m_tsbtnEngineer_Click(object sender, EventArgs e)
	{
		MessageBox.Show("Only in maintenance mode can be switched to engineer mode...");
	}

	protected virtual void m_tsbtnPlan_Click(object sender, EventArgs e)
	{
		MessageBox.Show("Only in maintenance mode can be switched to PlanDowntime mode...");
	}

	protected virtual void m_tsbtnComfirm_Click(object sender, EventArgs e)
	{
	}

	private void Sub_TaskStart()
	{
		m_edcTaskContainer.Sub_Initialize();
		m_tsbtnStart.Image = Resources.PNG_Stoppen_48x48;
	}

	private void Sub_TaskStop()
	{
		m_edcTaskContainer.Sub_EndTask();
		m_tsbtnStart.Image = Resources.PNG_Starten_48x48;
	}

	protected virtual void Sub_Login()
	{
		m_tsbtnUser.Text = "Logout";
	}

	protected virtual void Sub_Logout()
	{
		m_blnLogin = false;
		Sub_LogoutButtonEnable(0);
		m_edcAccount = new Edc_Account
		{
			m_strName = "Visitor",
			m_i32Authority = 0
		};
		m_edcLogger.Info("User logout");
		m_frmUserAuthentication.m_blnDefault = true;
		m_frmUserAuthentication.m_blnVerify = false;
		m_frmUserAuthentication.m_edcAccount = m_edcAccount;
	}

	protected void Sub_LogoutButtonEnable(int i_i32Authority)
	{
		if (i_i32Authority <= 0)
		{
			m_tsbtnSettingCustom.Enabled = false;
			m_tsbtnStart.Enabled = false;
			m_tsbtnClear.Enabled = false;
			m_tsbtnSettingErsa.Enabled = false;
			m_tsbtnUser.Enabled = true;
			m_tsbtnLock.Enabled = false;
			m_tsbtnRecipe.Enabled = false;
			m_tsbtnPLC.Enabled = false;
			m_tsbtnReport.Enabled = false;
			m_tsbtnCPK.Enabled = false;
			m_tsbtnErsaData.Enabled = false;
			m_tsbtnPLCData.Enabled = false;
			m_tsbtnEngineer.Enabled = false;
			m_tsbtnPlanDown.Enabled = false;
		}
	}

	protected void Sub_ShowOee(string i_strCode, string i_strText, int i_i32PcbInMachine = 0)
	{
		Invoke((Action)delegate
		{
			m_tsslbOEE.Text = $"Code:'{i_strCode}' Text:'{i_strText}' Pcb:{i_i32PcbInMachine}";
		});
	}

	protected void Sub_ShowErsasoftCount(int i_i32Cycle)
	{
		Invoke((Action)delegate
		{
			if (i_i32Cycle > 0 && !m_blnLampErsa)
			{
				m_blnLampErsa = true;
				m_tsslbLampErsa.Image = Resources.PNG_LED_gruen_24x24;
			}
			m_tsslbErsasoft.Text = $"Ersasoft Count:'{i_i32Cycle}'";
		});
	}

	protected void Sub_ShowPlcCount(int i_i32Cycle)
	{
		Invoke((Action)delegate
		{
			if (i_i32Cycle > 0 && !m_blnLampPLC)
			{
				m_blnLampPLC = true;
				m_tsslbLampPLC.Image = Resources.PNG_LED_gruen_24x24;
			}
			m_tsslbPLC.Text = $"PLC Count:'{i_i32Cycle}'";
		});
	}

	protected void Sub_ShowMessage(Enum_LogType i_enuLogType, string i_strMessage)
	{
		Invoke((Action)delegate
		{
			OlvMessageModel modelObject = new OlvMessageModel
			{
				m_strTime = DateTime.Now.ToString("HH:mm:ss,fff"),
				m_enuMessageLevel = i_enuLogType,
				m_strMessage = i_strMessage
			};
			if (m_olvInfo.Items.Count > 2000)
			{
				m_olvInfo.Items.Clear();
			}
			m_olvInfo.AddObject(modelObject);
			m_olvInfo.EnsureModelVisible(modelObject);
		});
	}

	protected virtual void m_tsbtnLock_Click(object sender, EventArgs e)
	{
		if (m_blnFrmLock)
		{
			if (m_frmUserAuthentication.ShowDialog() == DialogResult.OK)
			{
				if (m_frmUserAuthentication.m_blnVerify)
				{
					Sub_FrmActive();
				}
				m_blnFrmLock = false;
			}
		}
		else
		{
			Sub_FrmLock();
			m_blnFrmLock = true;
		}
	}

	private void m_btnUser_Click(object sender, EventArgs e)
	{
		if (m_blnLogin)
		{
			Sub_Logout();
			return;
		}
		m_frmUserAuthentication.m_strConfigUser = m_ConfigBase.m_clsBasicSettings.m_strUser;
		m_frmUserAuthentication.m_strConfigPassword = m_ConfigBase.m_clsBasicSettings.m_strPassword;
		m_frmUserAuthentication.ShowDialog();
		Sub_CheckLoginState(m_frmUserAuthentication.m_edcAccount);
	}

	protected void Sub_CheckLoginState(Edc_Account i_edcAccount)
	{
		m_edcAccount = i_edcAccount;
		Sub_tsbtnEnable(m_edcAccount.m_i32Authority);
		if (m_edcAccount.m_i32Authority >= 1)
		{
			m_blnLogin = true;
		}
	}

	private void Sub_tsbtnEnable(int i_i32Authority)
	{
		Sub_LogoutButtonEnable(i_i32Authority);
		if (i_i32Authority >= 1)
		{
			m_tsbtnSettingCustom.Enabled = true;
			m_tsbtnStart.Enabled = true;
			m_tsbtnClear.Enabled = true;
			m_tsbtnRecipe.Enabled = true;
			m_tsbtnErsaData.Enabled = true;
		}
		if (i_i32Authority >= 2)
		{
			m_tsbtnLock.Enabled = true;
			m_tsbtnSettingErsa.Enabled = true;
			m_tsbtnPLCData.Enabled = true;
		}
		if (i_i32Authority >= 9)
		{
			m_tsbtnPLC.Enabled = true;
		}
	}

	private void Sub_FrmLock()
	{
		m_tsbtnSettingCustom.Enabled = false;
		m_tsbtnStart.Enabled = false;
		m_tsbtnClear.Enabled = false;
		m_tsbtnSettingErsa.Enabled = false;
		m_tsbtnPLC.Enabled = false;
		m_tsbtnReport.Enabled = false;
		base.FormClosing += FrmMainChild_FormClosing;
	}

	private void Sub_FrmActive()
	{
		m_tsbtnSettingCustom.Enabled = true;
		m_tsbtnStart.Enabled = true;
		m_tsbtnClear.Enabled = true;
		m_tsbtnSettingErsa.Enabled = true;
		m_tsbtnPLC.Enabled = true;
		m_tsbtnReport.Enabled = true;
		base.FormClosing -= FrmMainChild_FormClosing;
	}

	private void FrmMainParent_SizeChanged(object sender, EventArgs e)
	{
		if (base.WindowState == FormWindowState.Minimized)
		{
			if (m_ConfigBase.m_clsBasicSettings.m_blnAllowMini)
			{
				notifyIcon1.Visible = true;
				base.ShowInTaskbar = false;
				Hide();
			}
		}
		else
		{
			notifyIcon1.Visible = false;
			Show();
		}
	}

	private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
	{
		Show();
		base.WindowState = FormWindowState.Normal;
	}

	private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		Show();
		base.WindowState = FormWindowState.Normal;
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ersa.Mes.Middleware.P_frmMain));
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.m_tsbtnSettingCustom = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.m_tsbtnStart = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnClear = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnSettingErsa = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnUser = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnLock = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.m_tsbtnRecipe = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnPLC = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnReport = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnCPK = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnErsaData = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnPLCData = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnEngineer = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnPlanDown = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.m_tsbtnComfirm = new System.Windows.Forms.ToolStripButton();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.m_tsslbLampErsa = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbErsasoft = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbEmpty1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbLampPLC = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbPLC = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbEmpty2 = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbOEE = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbEmpty3 = new System.Windows.Forms.ToolStripStatusLabel();
		this.m_tsslbAccount = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_olvInfo = new BrightIdeasSoftware.ObjectListView();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
		this.toolStrip1.SuspendLayout();
		this.statusStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvInfo).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(38, 38);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[21]
		{
			this.m_tsbtnSettingCustom, this.toolStripSeparator6, this.toolStripSeparator1, this.m_tsbtnStart, this.m_tsbtnClear, this.m_tsbtnSettingErsa, this.m_tsbtnUser, this.m_tsbtnLock, this.toolStripSeparator2, this.toolStripSeparator3,
			this.m_tsbtnRecipe, this.m_tsbtnPLC, this.m_tsbtnReport, this.m_tsbtnCPK, this.m_tsbtnErsaData, this.m_tsbtnPLCData, this.m_tsbtnEngineer, this.m_tsbtnPlanDown, this.toolStripSeparator5, this.toolStripSeparator4,
			this.m_tsbtnComfirm
		});
		resources.ApplyResources(this.toolStrip1, "toolStrip1");
		this.toolStrip1.Name = "toolStrip1";
		this.m_tsbtnSettingCustom.Image = Ersa.Mes.Middleware.Properties.Resources.PNG_Einstellungen_Ruesten_48x48;
		resources.ApplyResources(this.m_tsbtnSettingCustom, "m_tsbtnSettingCustom");
		this.m_tsbtnSettingCustom.Name = "m_tsbtnSettingCustom";
		this.m_tsbtnSettingCustom.Tag = "1_1";
		this.m_tsbtnSettingCustom.Click += new System.EventHandler(m_tsbtnCustom_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
		this.m_tsbtnStart.Image = Ersa.Mes.Middleware.Properties.Resources.PNG_Starten_48x48;
		resources.ApplyResources(this.m_tsbtnStart, "m_tsbtnStart");
		this.m_tsbtnStart.Name = "m_tsbtnStart";
		this.m_tsbtnStart.Click += new System.EventHandler(m_tsbtnStart_Click);
		this.m_tsbtnClear.Image = Ersa.Mes.Middleware.Properties.Resources.order;
		resources.ApplyResources(this.m_tsbtnClear, "m_tsbtnClear");
		this.m_tsbtnClear.Name = "m_tsbtnClear";
		this.m_tsbtnClear.Click += new System.EventHandler(m_tsbtnClear_Click);
		this.m_tsbtnSettingErsa.Image = Ersa.Mes.Middleware.Properties.Resources.PNG_Einstellungen_48x48;
		resources.ApplyResources(this.m_tsbtnSettingErsa, "m_tsbtnSettingErsa");
		this.m_tsbtnSettingErsa.Name = "m_tsbtnSettingErsa";
		this.m_tsbtnSettingErsa.Click += new System.EventHandler(m_tsbtnErsa_Click);
		resources.ApplyResources(this.m_tsbtnUser, "m_tsbtnUser");
		this.m_tsbtnUser.Image = Ersa.Mes.Middleware.Properties.Resources._user;
		this.m_tsbtnUser.Name = "m_tsbtnUser";
		this.m_tsbtnUser.Click += new System.EventHandler(m_btnUser_Click);
		this.m_tsbtnLock.Image = Ersa.Mes.Middleware.Properties.Resources._lock;
		resources.ApplyResources(this.m_tsbtnLock, "m_tsbtnLock");
		this.m_tsbtnLock.Name = "m_tsbtnLock";
		this.m_tsbtnLock.Click += new System.EventHandler(m_tsbtnLock_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
		this.m_tsbtnRecipe.Image = Ersa.Mes.Middleware.Properties.Resources._agreement;
		resources.ApplyResources(this.m_tsbtnRecipe, "m_tsbtnRecipe");
		this.m_tsbtnRecipe.Name = "m_tsbtnRecipe";
		this.m_tsbtnRecipe.Click += new System.EventHandler(m_tsbtnRecipe_Click);
		this.m_tsbtnPLC.Image = Ersa.Mes.Middleware.Properties.Resources._memory_one;
		resources.ApplyResources(this.m_tsbtnPLC, "m_tsbtnPLC");
		this.m_tsbtnPLC.Name = "m_tsbtnPLC";
		this.m_tsbtnPLC.Click += new System.EventHandler(m_tsbtnPLC_Click);
		this.m_tsbtnReport.Image = Ersa.Mes.Middleware.Properties.Resources._analysis;
		resources.ApplyResources(this.m_tsbtnReport, "m_tsbtnReport");
		this.m_tsbtnReport.Name = "m_tsbtnReport";
		this.m_tsbtnReport.Click += new System.EventHandler(m_tsbtnReport_Click);
		this.m_tsbtnCPK.Image = Ersa.Mes.Middleware.Properties.Resources._chart_histogram;
		resources.ApplyResources(this.m_tsbtnCPK, "m_tsbtnCPK");
		this.m_tsbtnCPK.Name = "m_tsbtnCPK";
		this.m_tsbtnCPK.Click += new System.EventHandler(m_tsbtnCPK_Click);
		this.m_tsbtnErsaData.Image = Ersa.Mes.Middleware.Properties.Resources._data_four;
		resources.ApplyResources(this.m_tsbtnErsaData, "m_tsbtnErsaData");
		this.m_tsbtnErsaData.Name = "m_tsbtnErsaData";
		this.m_tsbtnErsaData.Click += new System.EventHandler(m_tsbtnParameter_Click);
		this.m_tsbtnPLCData.Image = Ersa.Mes.Middleware.Properties.Resources._data_four;
		resources.ApplyResources(this.m_tsbtnPLCData, "m_tsbtnPLCData");
		this.m_tsbtnPLCData.Name = "m_tsbtnPLCData";
		this.m_tsbtnPLCData.Click += new System.EventHandler(m_tsbtnPLCData_Click);
		this.m_tsbtnEngineer.Image = Ersa.Mes.Middleware.Properties.Resources.worker;
		resources.ApplyResources(this.m_tsbtnEngineer, "m_tsbtnEngineer");
		this.m_tsbtnEngineer.Name = "m_tsbtnEngineer";
		this.m_tsbtnEngineer.Click += new System.EventHandler(m_tsbtnEngineer_Click);
		resources.ApplyResources(this.m_tsbtnPlanDown, "m_tsbtnPlanDown");
		this.m_tsbtnPlanDown.Image = Ersa.Mes.Middleware.Properties.Resources._plan;
		this.m_tsbtnPlanDown.Name = "m_tsbtnPlanDown";
		this.m_tsbtnPlanDown.Click += new System.EventHandler(m_tsbtnPlan_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
		resources.ApplyResources(this.m_tsbtnComfirm, "m_tsbtnComfirm");
		this.m_tsbtnComfirm.Image = Ersa.Mes.Middleware.Properties.Resources.check;
		this.m_tsbtnComfirm.Name = "m_tsbtnComfirm";
		this.m_tsbtnComfirm.Click += new System.EventHandler(m_tsbtnComfirm_Click);
		this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.m_tsslbLampErsa, this.m_tsslbErsasoft, this.m_tsslbEmpty1, this.m_tsslbLampPLC, this.m_tsslbPLC, this.m_tsslbEmpty2, this.m_tsslbOEE, this.m_tsslbEmpty3, this.m_tsslbAccount, this.toolStripStatusLabel2 });
		resources.ApplyResources(this.statusStrip1, "statusStrip1");
		this.statusStrip1.Name = "statusStrip1";
		this.m_tsslbLampErsa.Image = Ersa.Mes.Middleware.Properties.Resources.PNG_LED_rot_24x24;
		this.m_tsslbLampErsa.Name = "m_tsslbLampErsa";
		resources.ApplyResources(this.m_tsslbLampErsa, "m_tsslbLampErsa");
		this.m_tsslbErsasoft.Name = "m_tsslbErsasoft";
		resources.ApplyResources(this.m_tsslbErsasoft, "m_tsslbErsasoft");
		this.m_tsslbEmpty1.Name = "m_tsslbEmpty1";
		resources.ApplyResources(this.m_tsslbEmpty1, "m_tsslbEmpty1");
		this.m_tsslbLampPLC.Image = Ersa.Mes.Middleware.Properties.Resources.PNG_LED_rot_24x24;
		this.m_tsslbLampPLC.Name = "m_tsslbLampPLC";
		resources.ApplyResources(this.m_tsslbLampPLC, "m_tsslbLampPLC");
		this.m_tsslbPLC.Name = "m_tsslbPLC";
		resources.ApplyResources(this.m_tsslbPLC, "m_tsslbPLC");
		this.m_tsslbEmpty2.Name = "m_tsslbEmpty2";
		resources.ApplyResources(this.m_tsslbEmpty2, "m_tsslbEmpty2");
		this.m_tsslbOEE.Name = "m_tsslbOEE";
		resources.ApplyResources(this.m_tsslbOEE, "m_tsslbOEE");
		this.m_tsslbEmpty3.Name = "m_tsslbEmpty3";
		resources.ApplyResources(this.m_tsslbEmpty3, "m_tsslbEmpty3");
		this.m_tsslbAccount.Name = "m_tsslbAccount";
		resources.ApplyResources(this.m_tsslbAccount, "m_tsslbAccount");
		this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
		resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
		this.groupBox1.Controls.Add(this.m_olvInfo);
		resources.ApplyResources(this.groupBox1, "groupBox1");
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.TabStop = false;
		this.m_olvInfo.AlternateRowBackColor = System.Drawing.SystemColors.InactiveCaption;
		this.m_olvInfo.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvInfo.CellEditUseWholeCell = false;
		this.m_olvInfo.Cursor = System.Windows.Forms.Cursors.Default;
		resources.ApplyResources(this.m_olvInfo, "m_olvInfo");
		this.m_olvInfo.FullRowSelect = true;
		this.m_olvInfo.HideSelection = false;
		this.m_olvInfo.Name = "m_olvInfo";
		this.m_olvInfo.ShowImagesOnSubItems = true;
		this.m_olvInfo.UseCompatibleStateImageBehavior = false;
		this.m_olvInfo.View = System.Windows.Forms.View.Details;
		this.timer1.Interval = 200;
		resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
		this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon1_MouseClick);
		this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon1_MouseDoubleClick);
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.toolStrip1);
		base.MaximizeBox = false;
		base.Name = "P_frmMain";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FrmMainParent_FormClosing);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(P_frmMain_FormClosed);
		base.Load += new System.EventHandler(FrmParent_Load);
		base.Shown += new System.EventHandler(FrmMainParent_Shown);
		base.SizeChanged += new System.EventHandler(FrmMainParent_SizeChanged);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.m_olvInfo).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
