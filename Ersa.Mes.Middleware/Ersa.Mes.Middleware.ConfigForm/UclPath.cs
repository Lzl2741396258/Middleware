using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclPath : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private Button m_btnData;

	private Label label22;

	private TextBox m_txtPathData;

	private Button m_btnInitialize;

	private Label label19;

	private TextBox m_txtInitialize;

	private Button m_btnProtocol;

	private Button m_btnTrend;

	private Button m_btnz;

	private Label label6;

	private TextBox m_txtProtocol;

	private Button m_btnBibs;

	private Label label5;

	private TextBox m_txtTrend;

	private Label label3;

	private TextBox m_txtZ;

	private Label label1;

	private TextBox m_txtBibs;

	private Button m_btnErsaSoft;

	private Label label2;

	private TextBox m_txtErsasoft;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private Button m_btnApply;

	private Label label4;

	private TextBox m_txtPassword;

	private TextBox m_txtServerIP;

	private Label label8;

	private Label label7;

	private TextBox m_txtUserID;

	private TextBox m_txtShareName;

	private Label label9;

	private Label label10;

	private Button m_btnAuto;

	public int m_i32FormID { get; set; } = 403;


	public string Pro_FormName => "Path";

	private Edc_FilesPath m_edcFilesPath { get; set; }

	public string Pro_strFullnameInitialize { get; set; }

	public UclPath()
	{
		InitializeComponent();
	}

	public UclPath(Edc_ConfigBase i_ConfigBase, bool i_blnProtocol = false, bool i_blnZtxt = false, bool i_blnTrend = false, bool i_blnBibs = false, bool i_blnInitialize = false, bool i_blnData = false)
	{
		InitializeComponent();
		m_edcFilesPath = i_ConfigBase.m_edcFilesPath;
		Sub_ShowPath(i_blnProtocol, i_blnZtxt, i_blnTrend, i_blnBibs, i_blnInitialize, i_blnData);
		m_txtErsasoft.Text = m_edcFilesPath.m_strPathErsasoft;
		m_txtProtocol.Text = m_edcFilesPath.m_strPathProtocol;
		m_txtBibs.Text = m_edcFilesPath.m_strPathBibs;
		m_txtZ.Text = m_edcFilesPath.m_strPathZtxt;
		m_txtTrend.Text = m_edcFilesPath.m_strPathTrend;
		m_txtInitialize.Text = m_edcFilesPath.m_strPathInitialize;
		m_txtPathData.Text = m_edcFilesPath.m_strPathData;
	}

	private void Sub_ClearPath()
	{
		m_txtProtocol.Text = string.Empty;
		m_txtZ.Text = string.Empty;
		m_txtBibs.Text = string.Empty;
		m_txtPathData.Text = string.Empty;
		m_txtInitialize.Text = string.Empty;
		m_txtTrend.Text = string.Empty;
	}

	public void Sub_ShowPathErsasoft(string i_strPath)
	{
		m_txtErsasoft.Text = i_strPath;
	}

	public void Sub_ShowPathProtocol(string i_strPath)
	{
		if (Directory.Exists(i_strPath))
		{
			m_txtProtocol.Text = i_strPath;
		}
	}

	public void Sub_ShowPathZtxt(string i_strPath)
	{
		if (File.Exists(i_strPath))
		{
			m_txtZ.Text = i_strPath;
		}
	}

	public void Sub_ShowPathBibs(string i_strPath)
	{
		if (Directory.Exists(i_strPath))
		{
			m_txtBibs.Text = i_strPath;
		}
	}

	public void Sub_ShowPathData(string i_strPath)
	{
		if (Directory.Exists(i_strPath))
		{
			m_txtPathData.Text = i_strPath;
		}
	}

	public void Sub_ShowPathInitialize(string i_strPath)
	{
		m_txtInitialize.Text = i_strPath;
	}

	private void Sub_ShowPathTrend(string i_strPath)
	{
		if (Directory.Exists(i_strPath))
		{
			m_txtTrend.Text = i_strPath;
		}
	}

	private void m_txtErsasoft_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r')
		{
			Sub_PathAutoEdit(m_txtErsasoft.Text.Trim());
		}
	}

	private void m_btnErsaSoft_Click(object sender, EventArgs e)
	{
		Sub_PathAutoEdit(DirectoryFilesHelper.Fun_strOpenBrowser(Sub_ShowPathErsasoft));
	}

	private void btnProtocol_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Fun_strOpenBrowser(Sub_ShowPathProtocol);
	}

	private void btnz_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Sub_OpenFile(Sub_ShowPathZtxt);
	}

	private void btnTrend_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Fun_strOpenBrowser(Sub_ShowPathTrend);
	}

	private void btnBibs_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Fun_strOpenBrowser(Sub_ShowPathBibs);
	}

	private void btnInitialize_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Sub_OpenFile(Sub_ShowPathInitialize);
	}

	private void btnData_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Fun_strOpenBrowser(Sub_ShowPathData);
	}

	public void Sub_ShowPath(bool i_blnProtocol = false, bool i_blnZtxt = false, bool i_blnTrend = false, bool i_blnBibs = false, bool i_blnInitialize = false, bool i_blnData = false)
	{
		if (!i_blnProtocol)
		{
			m_txtProtocol.Enabled = false;
		}
		if (!i_blnZtxt)
		{
			m_txtZ.Enabled = false;
		}
		if (!i_blnTrend)
		{
			m_txtTrend.Enabled = false;
		}
		if (!i_blnBibs)
		{
			m_txtBibs.Enabled = false;
		}
		if (!i_blnInitialize)
		{
			m_txtInitialize.Enabled = false;
		}
		if (!i_blnData)
		{
			m_txtPathData.Enabled = false;
		}
	}

	private void m_btnApply_Click(object sender, EventArgs e)
	{
		MessageBox.Show(Fun_blnConnect(m_txtServerIP.Text, m_txtShareName.Text, m_txtUserID.Text, m_txtPassword.Text).ToString());
	}

	public bool Fun_blnConnect(string i_strRemoteHost, string i_strShareName, string i_strUserName, string i_strPassWord)
	{
		bool Flag = false;
		Process process = new Process();
		try
		{
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			string dosLine = "net use \\\\" + i_strRemoteHost + "\\" + i_strShareName + " /User:" + i_strUserName + " " + i_strPassWord + " /PERSISTENT:YES";
			process.StandardInput.WriteLine(dosLine);
			process.StandardInput.WriteLine("exit");
			while (!process.HasExited)
			{
				process.WaitForExit(1000);
			}
			string errormsg = process.StandardError.ReadToEnd();
			process.StandardError.Close();
			if (string.IsNullOrEmpty(errormsg))
			{
				Flag = true;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			process.Close();
			process.Dispose();
		}
		return Flag;
	}

	public bool Fun_blnSave(Edc_ConfigBase i_ConfigBase, bool i_blnShowMessagebox = true)
	{
		try
		{
			m_edcFilesPath.m_strPathErsasoft = m_txtErsasoft.Text.TrimEnd();
			m_edcFilesPath.m_strPathProtocol = m_txtProtocol.Text.TrimEnd();
			m_edcFilesPath.m_strPathBibs = m_txtBibs.Text.TrimEnd();
			m_edcFilesPath.m_strPathZtxt = m_txtZ.Text.TrimEnd();
			m_edcFilesPath.m_strPathTrend = m_txtTrend.Text.TrimEnd();
			m_edcFilesPath.m_strPathInitialize = m_txtInitialize.Text.TrimEnd();
			m_edcFilesPath.m_strPathData = m_txtPathData.Text.TrimEnd();
			i_ConfigBase.m_edcFilesPath = m_edcFilesPath;
		}
		catch
		{
			return false;
		}
		return true;
	}

	private void Sub_PathAutoEdit(string i_strPath)
	{
		if (!string.IsNullOrEmpty(i_strPath) && MessageBox.Show("Do you need to fill in automatically?", "Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
		{
			Sub_ClearPath();
			Sub_ShowPathProtocol(Path.Combine(i_strPath, "Protocol"));
			Sub_ShowPathZtxt(Path.Combine(i_strPath, "Backup", "z.txt"));
			Sub_ShowPathBibs(Path.Combine(i_strPath, "Bibs"));
			Sub_ShowPathData(Path.Combine(i_strPath, "Data"));
			Sub_ShowPathTrend(Path.Combine(i_strPath, "Trend"));
			Pro_strFullnameInitialize = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Initialize", "Initialisierung.xml");
		}
	}

	private void m_btnAuto_Click(object sender, EventArgs e)
	{
		m_edcFilesPath.m_strPathErsasoft = DirectoryFilesHelper.Fun_strGetErsasoftPath();
		m_txtErsasoft.Text = m_edcFilesPath.m_strPathErsasoft;
		string a_strPathErsasoftIni = DirectoryFilesHelper.Fun_strGetErsasoftSettingFilePath(m_edcFilesPath.m_strPathErsasoft);
		if (string.IsNullOrEmpty(a_strPathErsasoftIni))
		{
			return;
		}
		m_edcFilesPath.m_strPathProtocol = Path.Combine(m_edcFilesPath.m_strPathErsasoft, Edc_IniHelper.Fun_strRead("Protocol", "Directory", "", a_strPathErsasoftIni));
		m_txtProtocol.Text = m_edcFilesPath.m_strPathProtocol;
		m_edcFilesPath.m_strPathTrend = Path.Combine(m_edcFilesPath.m_strPathErsasoft, Edc_IniHelper.Fun_strRead("Trend", "Directory", "", a_strPathErsasoftIni));
		m_txtTrend.Text = m_edcFilesPath.m_strPathTrend;
		m_edcFilesPath.m_strPathZtxt = Path.Combine(m_edcFilesPath.m_strPathErsasoft, Edc_IniHelper.Fun_strRead("MachineConfig", "Directory", "Backup", a_strPathErsasoftIni), "z.txt");
		m_txtZ.Text = m_edcFilesPath.m_strPathZtxt;
		m_edcFilesPath.m_strPathBibs = Path.Combine(m_edcFilesPath.m_strPathErsasoft, Edc_IniHelper.Fun_strRead("Setup", "LoetprgPfad", "", a_strPathErsasoftIni));
		m_txtBibs.Text = m_edcFilesPath.m_strPathBibs;
		string a_strPathInitialize = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Initialize");
		if (Directory.Exists(a_strPathInitialize))
		{
			string[] files = Directory.GetFiles(a_strPathInitialize);
			if (files.Length != 0)
			{
				m_edcFilesPath.m_strPathInitialize = files[0];
				m_txtInitialize.Text = m_edcFilesPath.m_strPathInitialize;
			}
		}
		m_edcFilesPath.m_strPathData = Path.Combine(m_edcFilesPath.m_strPathErsasoft, "Data");
		m_txtPathData.Text = m_edcFilesPath.m_strPathData;
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
		this.m_btnData = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.m_txtPathData = new System.Windows.Forms.TextBox();
		this.m_btnInitialize = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.m_txtInitialize = new System.Windows.Forms.TextBox();
		this.m_btnProtocol = new System.Windows.Forms.Button();
		this.m_btnTrend = new System.Windows.Forms.Button();
		this.m_btnz = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.m_txtProtocol = new System.Windows.Forms.TextBox();
		this.m_btnBibs = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.m_txtTrend = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_txtZ = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.m_txtBibs = new System.Windows.Forms.TextBox();
		this.m_btnErsaSoft = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.m_txtErsasoft = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.m_btnAuto = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_btnApply = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.m_txtPassword = new System.Windows.Forms.TextBox();
		this.m_txtServerIP = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.m_txtUserID = new System.Windows.Forms.TextBox();
		this.m_txtShareName = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.m_btnData.Location = new System.Drawing.Point(540, 276);
		this.m_btnData.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnData.Name = "m_btnData";
		this.m_btnData.Size = new System.Drawing.Size(68, 26);
		this.m_btnData.TabIndex = 58;
		this.m_btnData.Text = "Open";
		this.m_btnData.UseVisualStyleBackColor = true;
		this.m_btnData.Click += new System.EventHandler(btnData_Click);
		this.label22.AutoSize = true;
		this.label22.Location = new System.Drawing.Point(74, 281);
		this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(35, 17);
		this.label22.TabIndex = 56;
		this.label22.Text = "Data";
		this.m_txtPathData.Location = new System.Drawing.Point(133, 277);
		this.m_txtPathData.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtPathData.Name = "m_txtPathData";
		this.m_txtPathData.Size = new System.Drawing.Size(395, 23);
		this.m_txtPathData.TabIndex = 57;
		this.m_btnInitialize.Location = new System.Drawing.Point(540, 248);
		this.m_btnInitialize.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnInitialize.Name = "m_btnInitialize";
		this.m_btnInitialize.Size = new System.Drawing.Size(68, 26);
		this.m_btnInitialize.TabIndex = 55;
		this.m_btnInitialize.Text = "Open";
		this.m_btnInitialize.UseVisualStyleBackColor = true;
		this.m_btnInitialize.Click += new System.EventHandler(btnInitialize_Click);
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(55, 251);
		this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(55, 17);
		this.label19.TabIndex = 53;
		this.label19.Text = "Initialize";
		this.m_txtInitialize.Location = new System.Drawing.Point(133, 247);
		this.m_txtInitialize.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtInitialize.Name = "m_txtInitialize";
		this.m_txtInitialize.Size = new System.Drawing.Size(395, 23);
		this.m_txtInitialize.TabIndex = 54;
		this.m_btnProtocol.Location = new System.Drawing.Point(540, 90);
		this.m_btnProtocol.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnProtocol.Name = "m_btnProtocol";
		this.m_btnProtocol.Size = new System.Drawing.Size(68, 26);
		this.m_btnProtocol.TabIndex = 52;
		this.m_btnProtocol.Text = "Open";
		this.m_btnProtocol.UseVisualStyleBackColor = true;
		this.m_btnProtocol.Click += new System.EventHandler(btnProtocol_Click);
		this.m_btnTrend.Location = new System.Drawing.Point(540, 150);
		this.m_btnTrend.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnTrend.Name = "m_btnTrend";
		this.m_btnTrend.Size = new System.Drawing.Size(68, 26);
		this.m_btnTrend.TabIndex = 51;
		this.m_btnTrend.Text = "Open";
		this.m_btnTrend.UseVisualStyleBackColor = true;
		this.m_btnTrend.Click += new System.EventHandler(btnTrend_Click);
		this.m_btnz.Location = new System.Drawing.Point(540, 120);
		this.m_btnz.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnz.Name = "m_btnz";
		this.m_btnz.Size = new System.Drawing.Size(68, 26);
		this.m_btnz.TabIndex = 50;
		this.m_btnz.Text = "Open";
		this.m_btnz.UseVisualStyleBackColor = true;
		this.m_btnz.Click += new System.EventHandler(btnz_Click);
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(55, 95);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(57, 17);
		this.label6.TabIndex = 48;
		this.label6.Text = "Protocol";
		this.m_txtProtocol.Location = new System.Drawing.Point(133, 91);
		this.m_txtProtocol.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtProtocol.Name = "m_txtProtocol";
		this.m_txtProtocol.Size = new System.Drawing.Size(395, 23);
		this.m_txtProtocol.TabIndex = 49;
		this.m_btnBibs.Location = new System.Drawing.Point(540, 180);
		this.m_btnBibs.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnBibs.Name = "m_btnBibs";
		this.m_btnBibs.Size = new System.Drawing.Size(68, 26);
		this.m_btnBibs.TabIndex = 47;
		this.m_btnBibs.Text = "Open";
		this.m_btnBibs.UseVisualStyleBackColor = true;
		this.m_btnBibs.Click += new System.EventHandler(btnBibs_Click);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(69, 155);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(42, 17);
		this.label5.TabIndex = 45;
		this.label5.Text = "Trend";
		this.m_txtTrend.Location = new System.Drawing.Point(133, 151);
		this.m_txtTrend.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtTrend.Name = "m_txtTrend";
		this.m_txtTrend.Size = new System.Drawing.Size(395, 23);
		this.m_txtTrend.TabIndex = 46;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(79, 125);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(29, 17);
		this.label3.TabIndex = 43;
		this.label3.Text = "Ztxt";
		this.m_txtZ.Location = new System.Drawing.Point(133, 121);
		this.m_txtZ.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtZ.Name = "m_txtZ";
		this.m_txtZ.Size = new System.Drawing.Size(395, 23);
		this.m_txtZ.TabIndex = 44;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(78, 185);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(33, 17);
		this.label1.TabIndex = 42;
		this.label1.Text = "Bibs";
		this.m_txtBibs.Location = new System.Drawing.Point(133, 181);
		this.m_txtBibs.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtBibs.Name = "m_txtBibs";
		this.m_txtBibs.Size = new System.Drawing.Size(395, 23);
		this.m_txtBibs.TabIndex = 41;
		this.m_btnErsaSoft.Location = new System.Drawing.Point(540, 33);
		this.m_btnErsaSoft.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnErsaSoft.Name = "m_btnErsaSoft";
		this.m_btnErsaSoft.Size = new System.Drawing.Size(68, 26);
		this.m_btnErsaSoft.TabIndex = 61;
		this.m_btnErsaSoft.Text = "Open";
		this.m_btnErsaSoft.UseVisualStyleBackColor = true;
		this.m_btnErsaSoft.Click += new System.EventHandler(m_btnErsaSoft_Click);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(28, 38);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(85, 17);
		this.label2.TabIndex = 59;
		this.label2.Text = "ErsaSoft Path";
		this.m_txtErsasoft.Location = new System.Drawing.Point(133, 33);
		this.m_txtErsasoft.Margin = new System.Windows.Forms.Padding(1);
		this.m_txtErsasoft.Name = "m_txtErsasoft";
		this.m_txtErsasoft.Size = new System.Drawing.Size(395, 23);
		this.m_txtErsasoft.TabIndex = 60;
		this.m_txtErsasoft.KeyPress += new System.Windows.Forms.KeyPressEventHandler(m_txtErsasoft_KeyPress);
		this.groupBox2.Controls.Add(this.m_btnAuto);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.m_txtBibs);
		this.groupBox2.Controls.Add(this.m_btnErsaSoft);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.m_txtZ);
		this.groupBox2.Controls.Add(this.m_txtErsasoft);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.m_btnData);
		this.groupBox2.Controls.Add(this.m_txtTrend);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.m_txtPathData);
		this.groupBox2.Controls.Add(this.m_btnBibs);
		this.groupBox2.Controls.Add(this.m_btnInitialize);
		this.groupBox2.Controls.Add(this.m_txtProtocol);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.m_txtInitialize);
		this.groupBox2.Controls.Add(this.m_btnz);
		this.groupBox2.Controls.Add(this.m_btnProtocol);
		this.groupBox2.Controls.Add(this.m_btnTrend);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
		this.groupBox2.Size = new System.Drawing.Size(738, 320);
		this.groupBox2.TabIndex = 71;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Path";
		this.m_btnAuto.Location = new System.Drawing.Point(610, 33);
		this.m_btnAuto.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnAuto.Name = "m_btnAuto";
		this.m_btnAuto.Size = new System.Drawing.Size(68, 26);
		this.m_btnAuto.TabIndex = 71;
		this.m_btnAuto.Text = "Search";
		this.m_btnAuto.UseVisualStyleBackColor = true;
		this.m_btnAuto.Click += new System.EventHandler(m_btnAuto_Click);
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(132, 216);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(237, 17);
		this.label10.TabIndex = 70;
		this.label10.Text = "If it is a network path \"//127.0.0.1\\Bibs\"";
		this.groupBox1.Controls.Add(this.m_btnApply);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.m_txtPassword);
		this.groupBox1.Controls.Add(this.m_txtServerIP);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.m_txtUserID);
		this.groupBox1.Controls.Add(this.m_txtShareName);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(0, 320);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(738, 134);
		this.groupBox1.TabIndex = 72;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Net Use";
		this.m_btnApply.Location = new System.Drawing.Point(540, 74);
		this.m_btnApply.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnApply.Name = "m_btnApply";
		this.m_btnApply.Size = new System.Drawing.Size(68, 26);
		this.m_btnApply.TabIndex = 62;
		this.m_btnApply.Text = "Apply";
		this.m_btnApply.UseVisualStyleBackColor = true;
		this.m_btnApply.Click += new System.EventHandler(m_btnApply_Click);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(55, 48);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(56, 17);
		this.label4.TabIndex = 62;
		this.label4.Text = "ServerIP";
		this.m_txtPassword.Location = new System.Drawing.Point(377, 74);
		this.m_txtPassword.Name = "m_txtPassword";
		this.m_txtPassword.PasswordChar = '*';
		this.m_txtPassword.Size = new System.Drawing.Size(151, 23);
		this.m_txtPassword.TabIndex = 69;
		this.m_txtServerIP.Location = new System.Drawing.Point(133, 45);
		this.m_txtServerIP.Name = "m_txtServerIP";
		this.m_txtServerIP.Size = new System.Drawing.Size(136, 23);
		this.m_txtServerIP.TabIndex = 63;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(289, 77);
		this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(64, 17);
		this.label8.TabIndex = 68;
		this.label8.Text = "Password";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(277, 48);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 17);
		this.label7.TabIndex = 64;
		this.label7.Text = "ShareName";
		this.m_txtUserID.Location = new System.Drawing.Point(133, 74);
		this.m_txtUserID.Name = "m_txtUserID";
		this.m_txtUserID.Size = new System.Drawing.Size(136, 23);
		this.m_txtUserID.TabIndex = 67;
		this.m_txtShareName.Location = new System.Drawing.Point(377, 45);
		this.m_txtShareName.Name = "m_txtShareName";
		this.m_txtShareName.Size = new System.Drawing.Size(151, 23);
		this.m_txtShareName.TabIndex = 65;
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(61, 77);
		this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(48, 17);
		this.label9.TabIndex = 66;
		this.label9.Text = "UserID";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.groupBox2);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclPath";
		base.Size = new System.Drawing.Size(738, 474);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}
