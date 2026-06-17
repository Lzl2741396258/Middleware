using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Helper;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclInterface : UserControl, Inf_ConfigForm
{
	private const string mC_strDefaultIP = "127.0.0.1";

	private const string mC_strDefaultPort = "12121";

	private IContainer components = null;

	private Label label4;

	private TextBox m_txtHost;

	private TextBox m_txtPort;

	private Label label2;

	private Button btn_Default;

	private Button m_btnTest;

	private GroupBox groupBox1;

	public int m_i32FormID { get; set; } = 402;


	public string Pro_FormName => "Interface";

	private Edc_InterfaceAddress m_edcInterfaceAddress { get; set; }

	public UclInterface(Edc_ConfigBase i_ConfigBase)
	{
		InitializeComponent();
		m_edcInterfaceAddress = i_ConfigBase.m_edcInterfaceAddress;
		Sub_Initialize();
	}

	public void Sub_Initialize()
	{
		m_txtHost.Text = m_edcInterfaceAddress.Pro_strClientIP ?? "127.0.0.1";
		m_txtPort.Text = m_edcInterfaceAddress.Pro_i32ClientPort.ToString();
	}

	private void btn_Default_Click(object sender, EventArgs e)
	{
		m_txtHost.Text = "127.0.0.1";
		m_txtPort.Text = "12121";
	}

	private async void m_btnTest_Click(object sender, EventArgs e)
	{
		try
		{
			if (await TestHelper.Sub_TestTcpConnect(m_txtHost.Text, int.Parse(m_txtPort.Text)))
			{
				MessageBox.Show("Successed...");
			}
			else
			{
				MessageBox.Show("failed...Please open the Ersasoft first...");
			}
		}
		catch
		{
			MessageBox.Show("failed...Please open the Ersasoft first...");
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		try
		{
			m_edcInterfaceAddress.Pro_strClientIP = m_txtHost.Text;
			m_edcInterfaceAddress.Pro_i32ClientPort = int.Parse(m_txtPort.Text);
			i_Config.m_edcInterfaceAddress = m_edcInterfaceAddress;
		}
		catch
		{
			return false;
		}
		return true;
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
		this.m_txtHost = new System.Windows.Forms.TextBox();
		this.m_txtPort = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.btn_Default = new System.Windows.Forms.Button();
		this.m_btnTest = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(35, 52);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(35, 17);
		this.label4.TabIndex = 22;
		this.label4.Text = "Host";
		this.m_txtHost.Location = new System.Drawing.Point(85, 49);
		this.m_txtHost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtHost.Name = "m_txtHost";
		this.m_txtHost.Size = new System.Drawing.Size(141, 23);
		this.m_txtHost.TabIndex = 21;
		this.m_txtHost.Text = "127.0.0.1";
		this.m_txtPort.Location = new System.Drawing.Point(299, 49);
		this.m_txtPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtPort.Name = "m_txtPort";
		this.m_txtPort.Size = new System.Drawing.Size(141, 23);
		this.m_txtPort.TabIndex = 19;
		this.m_txtPort.Text = "12121";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(253, 51);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(32, 17);
		this.label2.TabIndex = 20;
		this.label2.Text = "Port";
		this.btn_Default.Location = new System.Drawing.Point(554, 49);
		this.btn_Default.Margin = new System.Windows.Forms.Padding(2);
		this.btn_Default.Name = "btn_Default";
		this.btn_Default.Size = new System.Drawing.Size(67, 26);
		this.btn_Default.TabIndex = 23;
		this.btn_Default.Text = "Default";
		this.btn_Default.UseVisualStyleBackColor = true;
		this.btn_Default.Click += new System.EventHandler(btn_Default_Click);
		this.m_btnTest.Location = new System.Drawing.Point(483, 49);
		this.m_btnTest.Margin = new System.Windows.Forms.Padding(2);
		this.m_btnTest.Name = "m_btnTest";
		this.m_btnTest.Size = new System.Drawing.Size(67, 26);
		this.m_btnTest.TabIndex = 24;
		this.m_btnTest.Text = "Test";
		this.m_btnTest.UseVisualStyleBackColor = true;
		this.m_btnTest.Click += new System.EventHandler(m_btnTest_Click);
		this.groupBox1.Controls.Add(this.m_txtHost);
		this.groupBox1.Controls.Add(this.m_btnTest);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.btn_Default);
		this.groupBox1.Controls.Add(this.m_txtPort);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox1.Location = new System.Drawing.Point(5, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(656, 109);
		this.groupBox1.TabIndex = 25;
		this.groupBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclInterface";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(666, 119);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}
