using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.Initialize;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Interfaces;
using Ersa.Mes.PLC.Modules;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclPLC : UserControl, Inf_ConfigForm
{
	private string mC_strDirectory = "Configuration";

	private string mC_strFilename = "Initialize_PLC.xml";

	private IContainer components = null;

	private Label label4;

	private TextBox m_txtIP;

	private TextBox m_txtPort;

	private Label label2;

	private Label label1;

	private TextBox m_txtService;

	private Label label3;

	private TextBox m_txtCpu;

	private ComboBox m_cbxType;

	private Label label5;

	private GroupBox groupBox1;

	private GroupBox groupBox3;

	private Button button1;

	private Button m_btnAdd;

	private TextBox m_txtPath;

	private Label label7;

	private Label m_lbVariable;

	private TextBox m_txtVariable;

	private GroupBox groupBox2;

	private Label label9;

	private TextBox m_txtInterval;

	private Label label6;

	private TextBox m_txtRepetitions;

	private Label label8;

	private TextBox m_txtName;

	private ObjectListView m_olvPlcVariable;

	public int m_i32FormID { get; set; } = 406;


	public string Pro_FormName => "PLC";

	private Edc_PLC m_ConfigPLC { get; set; }

	private Inf_Logger m_edcLogger { get; }

	public string m_strFullname { get; set; }

	private Edc_InitializePLC m_edcfile { get; set; }

	public UclPLC()
	{
		InitializeComponent();
	}

	public UclPLC(Edc_ConfigBase i_ConfigBase, Inf_Logger i_logger)
	{
		InitializeComponent();
		m_ConfigPLC = i_ConfigBase.m_sttPLC;
		m_edcLogger = i_logger;
		m_strFullname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mC_strDirectory, mC_strFilename);
		Sub_Initialize();
		Sub_InitializeOlv();
	}

	public void Sub_Initialize()
	{
		if (m_ConfigPLC == null)
		{
			return;
		}
		try
		{
			m_cbxType.DataSource = ComboboxContorl.Sub_GetDeviceType();
			m_cbxType.SelectedIndex = m_cbxType.FindStringExact(m_ConfigPLC.m_strConnectionType);
			m_txtIP.Text = m_ConfigPLC.m_strIP;
			m_txtPort.Text = m_ConfigPLC.m_i32Port.ToString();
			m_txtService.Text = m_ConfigPLC.m_strServiceName;
			m_txtCpu.Text = m_ConfigPLC.m_strCpuName;
		}
		catch (Exception ex)
		{
			m_edcLogger.Error("UclPLC Sub_Initialize() error...Details:'" + ex.Message + "'", null, "Sub_Initialize", 90);
		}
	}

	private void Sub_InitializeOlv()
	{
		try
		{
			if (File.Exists(m_strFullname))
			{
				m_txtPath.Text = m_strFullname;
			}
			else
			{
				m_txtPath.Text = "Not found  " + m_strFullname;
			}
			m_olvPlcVariable.EmptyListMsg = "Please check the config file...";
			m_olvPlcVariable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			m_olvPlcVariable.ShowImagesOnSubItems = true;
			m_olvPlcVariable.ShowGroups = false;
			m_olvPlcVariable.FullRowSelect = true;
			m_olvPlcVariable.View = View.Details;
			m_olvPlcVariable.Columns.Clear();
			PropertyInfo[] properties = typeof(Edc_ParameterPLC).GetProperties();
			foreach (PropertyInfo item in properties)
			{
				string title = item.Name;
				DescriptionAttribute result = (DescriptionAttribute)item.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault();
				string a_strDescriptionName = result.Description;
				OLVColumn column = new OLVColumn(a_strDescriptionName, item.Name);
				column.Width = 120;
				m_olvPlcVariable.Columns.Add(column);
			}
			Sub_LoadPLCParameter();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		try
		{
			m_ConfigPLC.m_strConnectionType = m_cbxType.Text.Trim();
			m_ConfigPLC.m_strIP = m_txtIP.Text.Trim();
			m_ConfigPLC.m_i32Port = int.Parse(m_txtPort.Text.Trim());
			m_ConfigPLC.m_strServiceName = m_txtService.Text.Trim();
			m_ConfigPLC.m_strCpuName = m_txtCpu.Text.Trim();
			i_Config.m_sttPLC = m_ConfigPLC;
		}
		catch
		{
			return false;
		}
		try
		{
			m_edcfile.m_strName = m_txtName.Text;
			int.TryParse(m_txtRepetitions.Text, out var o_i32Repetitions);
			m_edcfile.m_i32Repetitions = o_i32Repetitions;
			int.TryParse(m_txtInterval.Text, out var o_i32Interval);
			m_edcfile.m_i32Interval = o_i32Interval;
			bool result = Edc_OperationConfig.Fun_WriteConfig<Edc_InitializePLC>(m_strFullname, m_edcfile);
		}
		catch (Exception ex)
		{
			m_edcLogger.Error("Save initailize file error..." + ex.Message + "  " + MethodBase.GetCurrentMethod().Name, null, "Fun_blnSave", 173);
		}
		return true;
	}

	private void Sub_LoadPLCParameter()
	{
		try
		{
			if (!string.IsNullOrEmpty(m_strFullname) && File.Exists(m_strFullname))
			{
				m_edcfile = m_strFullname.Fun_edcDeserializeByFilePath<Edc_InitializePLC>();
				m_txtName.Text = m_edcfile.m_strName;
				m_txtRepetitions.Text = m_edcfile.m_i32Repetitions.ToString();
				m_txtInterval.Text = m_edcfile.m_i32Interval.ToString();
				m_olvPlcVariable.SetObjects(m_edcfile.m_lstParameterPLC.ToList());
			}
		}
		catch (Exception ex)
		{
			string a_strMessage = "Damaged file Initialize_PLC.xml";
			m_edcLogger.Error(a_strMessage + "  Details:" + ex.Message, null, "Sub_LoadPLCParameter", 203);
			throw;
		}
	}

	private void m_btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			string[] variable = m_txtVariable.Text.Split('.');
			if (variable.Length >= 2)
			{
				Edc_ParameterPLC para = new Edc_ParameterPLC
				{
					m_strTask = variable[0],
					m_strPVariable = variable[1],
					m_strStructMember = variable[2]
				};
				m_edcfile.m_lstParameterPLC.Add(para);
				m_olvPlcVariable.AddObject(para);
				m_olvPlcVariable.EnableObject(para);
			}
		}
		catch (Exception ex)
		{
			m_edcLogger.Error("Initailize file add parameter error..." + ex.Message + "  " + MethodBase.GetCurrentMethod().Name, null, "m_btnAdd_Click", 228);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
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
		this.label4 = new System.Windows.Forms.Label();
		this.m_txtIP = new System.Windows.Forms.TextBox();
		this.m_txtPort = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.m_txtService = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_txtCpu = new System.Windows.Forms.TextBox();
		this.m_cbxType = new System.Windows.Forms.ComboBox();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.m_btnAdd = new System.Windows.Forms.Button();
		this.m_txtPath = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.m_lbVariable = new System.Windows.Forms.Label();
		this.m_txtVariable = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.m_txtInterval = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.m_txtRepetitions = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.m_txtName = new System.Windows.Forms.TextBox();
		this.m_olvPlcVariable = new BrightIdeasSoftware.ObjectListView();
		this.groupBox1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvPlcVariable).BeginInit();
		base.SuspendLayout();
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(289, 37);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(19, 17);
		this.label4.TabIndex = 28;
		this.label4.Text = "IP";
		this.m_txtIP.Location = new System.Drawing.Point(312, 34);
		this.m_txtIP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtIP.Name = "m_txtIP";
		this.m_txtIP.Size = new System.Drawing.Size(150, 23);
		this.m_txtIP.TabIndex = 27;
		this.m_txtPort.Location = new System.Drawing.Point(534, 34);
		this.m_txtPort.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtPort.Name = "m_txtPort";
		this.m_txtPort.Size = new System.Drawing.Size(150, 23);
		this.m_txtPort.TabIndex = 25;
		this.m_txtPort.Text = "20000";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(498, 37);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(32, 17);
		this.label2.TabIndex = 26;
		this.label2.Text = "Port";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(19, 79);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(49, 17);
		this.label1.TabIndex = 32;
		this.label1.Text = "Service";
		this.m_txtService.Location = new System.Drawing.Point(72, 77);
		this.m_txtService.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtService.Name = "m_txtService";
		this.m_txtService.Size = new System.Drawing.Size(150, 23);
		this.m_txtService.TabIndex = 31;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(277, 79);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(31, 17);
		this.label3.TabIndex = 34;
		this.label3.Text = "Cpu";
		this.m_txtCpu.Location = new System.Drawing.Point(312, 76);
		this.m_txtCpu.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtCpu.Name = "m_txtCpu";
		this.m_txtCpu.Size = new System.Drawing.Size(150, 23);
		this.m_txtCpu.TabIndex = 33;
		this.m_cbxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxType.FormattingEnabled = true;
		this.m_cbxType.Location = new System.Drawing.Point(72, 34);
		this.m_cbxType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_cbxType.Name = "m_cbxType";
		this.m_cbxType.Size = new System.Drawing.Size(150, 25);
		this.m_cbxType.TabIndex = 37;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(32, 37);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(36, 17);
		this.label5.TabIndex = 38;
		this.label5.Text = "Type";
		this.groupBox1.Controls.Add(this.m_cbxType);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.m_txtPort);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.m_txtIP);
		this.groupBox1.Controls.Add(this.m_txtCpu);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.m_txtService);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(5, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(712, 130);
		this.groupBox1.TabIndex = 39;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "PLC";
		this.groupBox3.Controls.Add(this.button1);
		this.groupBox3.Controls.Add(this.m_btnAdd);
		this.groupBox3.Controls.Add(this.m_txtPath);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.m_lbVariable);
		this.groupBox3.Controls.Add(this.m_txtVariable);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox3.Location = new System.Drawing.Point(5, 135);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(712, 110);
		this.groupBox3.TabIndex = 43;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Viriable";
		this.button1.Location = new System.Drawing.Point(609, 62);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 32);
		this.button1.TabIndex = 38;
		this.button1.Text = "Remove";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.m_btnAdd.Location = new System.Drawing.Point(528, 62);
		this.m_btnAdd.Name = "m_btnAdd";
		this.m_btnAdd.Size = new System.Drawing.Size(75, 32);
		this.m_btnAdd.TabIndex = 37;
		this.m_btnAdd.Text = "Add";
		this.m_btnAdd.UseVisualStyleBackColor = true;
		this.m_btnAdd.Click += new System.EventHandler(m_btnAdd_Click);
		this.m_txtPath.Location = new System.Drawing.Point(72, 34);
		this.m_txtPath.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtPath.Name = "m_txtPath";
		this.m_txtPath.ReadOnly = true;
		this.m_txtPath.Size = new System.Drawing.Size(612, 23);
		this.m_txtPath.TabIndex = 36;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(35, 37);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(33, 17);
		this.label7.TabIndex = 35;
		this.label7.Text = "Path";
		this.m_lbVariable.AutoSize = true;
		this.m_lbVariable.Location = new System.Drawing.Point(12, 70);
		this.m_lbVariable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.m_lbVariable.Name = "m_lbVariable";
		this.m_lbVariable.Size = new System.Drawing.Size(56, 17);
		this.m_lbVariable.TabIndex = 34;
		this.m_lbVariable.Text = "Variable";
		this.m_txtVariable.Location = new System.Drawing.Point(72, 67);
		this.m_txtVariable.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtVariable.Name = "m_txtVariable";
		this.m_txtVariable.Size = new System.Drawing.Size(451, 23);
		this.m_txtVariable.TabIndex = 33;
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.m_txtInterval);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.m_txtRepetitions);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.m_txtName);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(5, 245);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(712, 82);
		this.groupBox2.TabIndex = 44;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "List";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(478, 36);
		this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(51, 17);
		this.label9.TabIndex = 40;
		this.label9.Text = "Interval";
		this.m_txtInterval.Location = new System.Drawing.Point(534, 33);
		this.m_txtInterval.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtInterval.Name = "m_txtInterval";
		this.m_txtInterval.Size = new System.Drawing.Size(150, 23);
		this.m_txtInterval.TabIndex = 39;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(235, 35);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(73, 17);
		this.label6.TabIndex = 38;
		this.label6.Text = "Repetitions";
		this.m_txtRepetitions.Location = new System.Drawing.Point(312, 33);
		this.m_txtRepetitions.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtRepetitions.Name = "m_txtRepetitions";
		this.m_txtRepetitions.Size = new System.Drawing.Size(150, 23);
		this.m_txtRepetitions.TabIndex = 37;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(25, 35);
		this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 17);
		this.label8.TabIndex = 36;
		this.label8.Text = "Name";
		this.m_txtName.Location = new System.Drawing.Point(72, 33);
		this.m_txtName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
		this.m_txtName.Name = "m_txtName";
		this.m_txtName.Size = new System.Drawing.Size(150, 23);
		this.m_txtName.TabIndex = 35;
		this.m_olvPlcVariable.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvPlcVariable.CellEditUseWholeCell = false;
		this.m_olvPlcVariable.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvPlcVariable.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvPlcVariable.Font = new System.Drawing.Font("阿里巴巴普惠体 2.0 55 Regular", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.m_olvPlcVariable.FullRowSelect = true;
		this.m_olvPlcVariable.HideSelection = false;
		this.m_olvPlcVariable.Location = new System.Drawing.Point(5, 327);
		this.m_olvPlcVariable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_olvPlcVariable.Name = "m_olvPlcVariable";
		this.m_olvPlcVariable.ShowGroups = false;
		this.m_olvPlcVariable.ShowImagesOnSubItems = true;
		this.m_olvPlcVariable.Size = new System.Drawing.Size(712, 350);
		this.m_olvPlcVariable.TabIndex = 45;
		this.m_olvPlcVariable.UseCompatibleStateImageBehavior = false;
		this.m_olvPlcVariable.View = System.Windows.Forms.View.Details;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.m_olvPlcVariable);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("微软雅黑", 8.765218f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclPLC";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(722, 682);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvPlcVariable).EndInit();
		base.ResumeLayout(false);
	}
}
