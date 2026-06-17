using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclBasic : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private ComboBox m_cbxLogLevels;

	private Label label13;

	private Label label23;

	private TextBox m_txtMachineId;

	private Label label1;

	private TextBox m_txtTrack;

	private Label label2;

	private TextBox m_txtUser;

	private Label label3;

	private TextBox m_txtPassword;

	private CheckBox m_chkWindowStartup;

	private Label label4;

	private ComboBox m_cbxMachineType;

	private CheckBox m_chkAutoFunction;

	private Label label5;

	private TextBox m_txtDelay;

	private CheckBox m_chkAllowMini;

	private Label label6;

	private TextBox m_txtAutoUpdate;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private TextBox m_txtZoneNumber;

	private Label label7;

	private CheckBox m_chkExitVerification;

	private CheckBox m_chkShowLamp;

	private CheckBox m_chkLoginFirst;

	public int m_i32FormID { get; set; } = 401;


	public string Pro_FormName => "SystemBasic";

	private Edc_BasicSettings m_edcBasicSettings { get; set; }

	public UclBasic(Edc_ConfigBase i_Config)
	{
		InitializeComponent();
		base.BorderStyle = BorderStyle.FixedSingle;
		m_edcBasicSettings = i_Config.m_clsBasicSettings;
		Sub_Initialize();
	}

	public void Sub_Initialize()
	{
		m_chkWindowStartup.Checked = m_edcBasicSettings.m_blnWindowsStartup;
		m_chkShowLamp.Checked = m_edcBasicSettings.m_blnShowLamp;
		m_chkAutoFunction.Checked = m_edcBasicSettings.m_blnAutoFunction;
		m_txtDelay.Text = m_edcBasicSettings.m_i32AutoFunctionDelay.ToString();
		m_chkAllowMini.Checked = m_edcBasicSettings.m_blnAllowMini;
		m_chkExitVerification.Checked = m_edcBasicSettings.m_blnExitVerification;
		m_chkLoginFirst.Checked = m_edcBasicSettings.m_blnLoginFirst;
		m_txtMachineId.Text = m_edcBasicSettings.m_strMachineId;
		m_cbxMachineType.DataSource = Enum.GetNames(typeof(Enum_MachineType));
		m_cbxMachineType.SelectedIndex = m_cbxMachineType.FindStringExact(m_edcBasicSettings.m_strMachineType);
		m_txtZoneNumber.Text = m_edcBasicSettings.m_i32MaxHZoneNumber.ToString();
		m_cbxLogLevels.DataSource = Enum.GetNames(typeof(Enum_LogLevels));
		m_cbxLogLevels.SelectedIndex = m_cbxLogLevels.FindStringExact(Enum.GetName(typeof(Enum_LogLevels), m_edcBasicSettings.m_strLogLevel));
		m_txtTrack.Text = string.Join(",", m_edcBasicSettings.m_strTrackIds);
		m_txtUser.Text = m_edcBasicSettings.m_strUser;
		m_txtPassword.Text = m_edcBasicSettings.m_strPassword;
		m_txtAutoUpdate.Text = m_edcBasicSettings.m_strAutoUpdateUrl;
	}

	public bool Fun_blnSave(Edc_ConfigBase i_edcConfigBase, bool i_blnShowMessagebox = true)
	{
		try
		{
			m_edcBasicSettings.m_blnWindowsStartup = m_chkWindowStartup.Checked;
			m_edcBasicSettings.m_blnShowLamp = m_chkShowLamp.Checked;
			m_edcBasicSettings.m_blnAutoFunction = m_chkAutoFunction.Checked;
			int result = -1;
			int.TryParse(m_txtDelay.Text, out result);
			m_edcBasicSettings.m_blnAllowMini = m_chkAllowMini.Checked;
			m_edcBasicSettings.m_blnExitVerification = m_chkExitVerification.Checked;
			m_edcBasicSettings.m_blnLoginFirst = m_chkLoginFirst.Checked;
			m_edcBasicSettings.m_i32AutoFunctionDelay = result;
			m_edcBasicSettings.m_strMachineId = m_txtMachineId.Text.Trim();
			result = 10;
			int.TryParse(m_txtZoneNumber.Text, out result);
			m_edcBasicSettings.m_i32MaxHZoneNumber = result;
			m_edcBasicSettings.m_strMachineType = m_cbxMachineType.Text;
			if (string.IsNullOrEmpty(m_cbxLogLevels.Text))
			{
				m_cbxLogLevels.SelectedIndex = 0;
			}
			m_edcBasicSettings.m_strLogLevel = (int)(Enum_LogLevels)Enum.Parse(typeof(Enum_LogLevels), m_cbxLogLevels.Text);
			try
			{
				m_edcBasicSettings.m_strTrackIds = Array.ConvertAll(m_txtTrack.Text.Split(','), int.Parse).ToList();
			}
			catch
			{
				m_edcBasicSettings.m_strTrackIds = new List<int> { 1 };
			}
			m_edcBasicSettings.m_strUser = m_txtUser.Text.Trim();
			m_edcBasicSettings.m_strPassword = m_txtPassword.Text.Trim();
			m_edcBasicSettings.m_strAutoUpdateUrl = m_txtAutoUpdate.Text.Trim();
			i_edcConfigBase.m_clsBasicSettings = m_edcBasicSettings;
		}
		catch
		{
			return false;
		}
		return true;
	}

	private void m_cbxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!m_cbxMachineType.Text.Equals(Enum_MachineType.Reflow.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			m_txtZoneNumber.Enabled = false;
		}
		else
		{
			m_txtZoneNumber.Enabled = true;
		}
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
		this.m_cbxLogLevels = new System.Windows.Forms.ComboBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.m_txtMachineId = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.m_txtTrack = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.m_txtUser = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_txtPassword = new System.Windows.Forms.TextBox();
		this.m_chkWindowStartup = new System.Windows.Forms.CheckBox();
		this.label4 = new System.Windows.Forms.Label();
		this.m_cbxMachineType = new System.Windows.Forms.ComboBox();
		this.m_chkAutoFunction = new System.Windows.Forms.CheckBox();
		this.label5 = new System.Windows.Forms.Label();
		this.m_txtDelay = new System.Windows.Forms.TextBox();
		this.m_chkAllowMini = new System.Windows.Forms.CheckBox();
		this.m_txtAutoUpdate = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_chkLoginFirst = new System.Windows.Forms.CheckBox();
		this.m_chkShowLamp = new System.Windows.Forms.CheckBox();
		this.m_chkExitVerification = new System.Windows.Forms.CheckBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.m_txtZoneNumber = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		base.SuspendLayout();
		this.m_cbxLogLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxLogLevels.FormattingEnabled = true;
		this.m_cbxLogLevels.Location = new System.Drawing.Point(119, 38);
		this.m_cbxLogLevels.Margin = new System.Windows.Forms.Padding(2);
		this.m_cbxLogLevels.Name = "m_cbxLogLevels";
		this.m_cbxLogLevels.Size = new System.Drawing.Size(172, 25);
		this.m_cbxLogLevels.TabIndex = 23;
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(54, 41);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(59, 17);
		this.label13.TabIndex = 21;
		this.label13.Text = "LogLevel";
		this.label23.AutoSize = true;
		this.label23.Location = new System.Drawing.Point(44, 26);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(69, 17);
		this.label23.TabIndex = 22;
		this.label23.Text = "MachineId";
		this.m_txtMachineId.Location = new System.Drawing.Point(119, 23);
		this.m_txtMachineId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtMachineId.Name = "m_txtMachineId";
		this.m_txtMachineId.Size = new System.Drawing.Size(172, 23);
		this.m_txtMachineId.TabIndex = 20;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(73, 70);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(40, 17);
		this.label1.TabIndex = 25;
		this.label1.Text = "Track";
		this.m_txtTrack.Location = new System.Drawing.Point(119, 67);
		this.m_txtTrack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtTrack.Name = "m_txtTrack";
		this.m_txtTrack.Size = new System.Drawing.Size(172, 23);
		this.m_txtTrack.TabIndex = 24;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(78, 86);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(35, 17);
		this.label2.TabIndex = 27;
		this.label2.Text = "User";
		this.m_txtUser.Location = new System.Drawing.Point(119, 83);
		this.m_txtUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtUser.Name = "m_txtUser";
		this.m_txtUser.Size = new System.Drawing.Size(172, 23);
		this.m_txtUser.TabIndex = 26;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(354, 86);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 17);
		this.label3.TabIndex = 29;
		this.label3.Text = "Password";
		this.m_txtPassword.Location = new System.Drawing.Point(424, 83);
		this.m_txtPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtPassword.Name = "m_txtPassword";
		this.m_txtPassword.PasswordChar = '*';
		this.m_txtPassword.Size = new System.Drawing.Size(172, 23);
		this.m_txtPassword.TabIndex = 28;
		this.m_chkWindowStartup.AutoSize = true;
		this.m_chkWindowStartup.Location = new System.Drawing.Point(71, 32);
		this.m_chkWindowStartup.Name = "m_chkWindowStartup";
		this.m_chkWindowStartup.Size = new System.Drawing.Size(135, 21);
		this.m_chkWindowStartup.TabIndex = 30;
		this.m_chkWindowStartup.Text = "Start with windows";
		this.m_chkWindowStartup.UseVisualStyleBackColor = true;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(334, 25);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(85, 17);
		this.label4.TabIndex = 32;
		this.label4.Text = "MachineType";
		this.m_cbxMachineType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxMachineType.FormattingEnabled = true;
		this.m_cbxMachineType.Location = new System.Drawing.Point(424, 21);
		this.m_cbxMachineType.Margin = new System.Windows.Forms.Padding(2);
		this.m_cbxMachineType.Name = "m_cbxMachineType";
		this.m_cbxMachineType.Size = new System.Drawing.Size(172, 25);
		this.m_cbxMachineType.TabIndex = 33;
		this.m_cbxMachineType.SelectedIndexChanged += new System.EventHandler(m_cbxType_SelectedIndexChanged);
		this.m_chkAutoFunction.AutoSize = true;
		this.m_chkAutoFunction.Location = new System.Drawing.Point(71, 78);
		this.m_chkAutoFunction.Name = "m_chkAutoFunction";
		this.m_chkAutoFunction.Size = new System.Drawing.Size(138, 21);
		this.m_chkAutoFunction.TabIndex = 34;
		this.m_chkAutoFunction.Text = "Auto open function";
		this.m_chkAutoFunction.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(378, 79);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(40, 17);
		this.label5.TabIndex = 36;
		this.label5.Text = "Delay";
		this.m_txtDelay.Location = new System.Drawing.Point(424, 76);
		this.m_txtDelay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtDelay.Name = "m_txtDelay";
		this.m_txtDelay.Size = new System.Drawing.Size(172, 23);
		this.m_txtDelay.TabIndex = 35;
		this.m_chkAllowMini.AutoSize = true;
		this.m_chkAllowMini.Location = new System.Drawing.Point(71, 126);
		this.m_chkAllowMini.Name = "m_chkAllowMini";
		this.m_chkAllowMini.Size = new System.Drawing.Size(135, 21);
		this.m_chkAllowMini.TabIndex = 37;
		this.m_chkAllowMini.Text = "Allow minimization";
		this.m_chkAllowMini.UseVisualStyleBackColor = true;
		this.m_txtAutoUpdate.Location = new System.Drawing.Point(119, 123);
		this.m_txtAutoUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtAutoUpdate.Name = "m_txtAutoUpdate";
		this.m_txtAutoUpdate.Size = new System.Drawing.Size(477, 23);
		this.m_txtAutoUpdate.TabIndex = 38;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(35, 126);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 17);
		this.label6.TabIndex = 39;
		this.label6.Text = "AutoUpdate";
		this.groupBox1.Controls.Add(this.m_chkLoginFirst);
		this.groupBox1.Controls.Add(this.m_chkShowLamp);
		this.groupBox1.Controls.Add(this.m_chkExitVerification);
		this.groupBox1.Controls.Add(this.m_chkWindowStartup);
		this.groupBox1.Controls.Add(this.m_chkAutoFunction);
		this.groupBox1.Controls.Add(this.m_txtDelay);
		this.groupBox1.Controls.Add(this.m_chkAllowMini);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(5, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(676, 232);
		this.groupBox1.TabIndex = 40;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Universal";
		this.m_chkLoginFirst.AutoSize = true;
		this.m_chkLoginFirst.Location = new System.Drawing.Point(71, 174);
		this.m_chkLoginFirst.Name = "m_chkLoginFirst";
		this.m_chkLoginFirst.Size = new System.Drawing.Size(87, 21);
		this.m_chkLoginFirst.TabIndex = 40;
		this.m_chkLoginFirst.Text = "Login First";
		this.m_chkLoginFirst.UseVisualStyleBackColor = true;
		this.m_chkShowLamp.AutoSize = true;
		this.m_chkShowLamp.Location = new System.Drawing.Point(424, 32);
		this.m_chkShowLamp.Name = "m_chkShowLamp";
		this.m_chkShowLamp.Size = new System.Drawing.Size(138, 21);
		this.m_chkShowLamp.TabIndex = 39;
		this.m_chkShowLamp.Text = "Show bottom lamp";
		this.m_chkShowLamp.UseVisualStyleBackColor = true;
		this.m_chkExitVerification.AutoSize = true;
		this.m_chkExitVerification.Location = new System.Drawing.Point(424, 126);
		this.m_chkExitVerification.Name = "m_chkExitVerification";
		this.m_chkExitVerification.Size = new System.Drawing.Size(114, 21);
		this.m_chkExitVerification.TabIndex = 38;
		this.m_chkExitVerification.Text = "Exit verification";
		this.m_chkExitVerification.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.m_txtZoneNumber);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.m_txtMachineId);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.m_txtTrack);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.m_cbxMachineType);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(5, 237);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(676, 111);
		this.groupBox2.TabIndex = 41;
		this.groupBox2.TabStop = false;
		this.m_txtZoneNumber.Location = new System.Drawing.Point(424, 63);
		this.m_txtZoneNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtZoneNumber.Name = "m_txtZoneNumber";
		this.m_txtZoneNumber.Size = new System.Drawing.Size(172, 23);
		this.m_txtZoneNumber.TabIndex = 34;
		this.m_txtZoneNumber.Text = "10";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(333, 66);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(85, 17);
		this.label7.TabIndex = 35;
		this.label7.Text = "ZoneNumber";
		this.groupBox3.Controls.Add(this.m_cbxLogLevels);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.m_txtUser);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.m_txtAutoUpdate);
		this.groupBox3.Controls.Add(this.m_txtPassword);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox3.Location = new System.Drawing.Point(5, 348);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(676, 189);
		this.groupBox3.TabIndex = 42;
		this.groupBox3.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclBasic";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(686, 542);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		base.ResumeLayout(false);
	}
}
