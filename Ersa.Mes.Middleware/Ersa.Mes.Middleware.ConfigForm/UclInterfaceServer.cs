using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Helper;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclInterfaceServer : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private Label label4;

	private TextBox m_txtHost;

	private TextBox m_txtPort;

	private Label label2;

	private Button m_btnDefault;

	private Button m_btnTest;

	private ObjectListView objectListView1;

	private Button m_btnAdd;

	private Button m_btnRemove;

	private OLVColumn olvColumn1;

	private OLVColumn olvColumn2;

	public int m_i32FormID { get; set; } = 407;


	public string Pro_FormName => "InterfaceServer";

	private Edc_InterfaceAddress[] m_ConfigInterfaceAddresses { get; set; }

	private Inf_Logger m_logger { get; }

	public UclInterfaceServer(Edc_ConfigBase i_ConfigBase, Inf_Logger i_logger)
	{
		InitializeComponent();
		base.BorderStyle = BorderStyle.FixedSingle;
		m_logger = i_logger;
		m_ConfigInterfaceAddresses = i_ConfigBase.m_edcInterfaceAddresses;
		Sub_Initialize();
	}

	public void Sub_Initialize()
	{
		objectListView1.SetObjects(m_ConfigInterfaceAddresses.ToList());
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

	private void m_btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			Edc_InterfaceAddress address = new Edc_InterfaceAddress
			{
				Pro_strClientIP = m_txtHost.Text.Trim(),
				Pro_i32ClientPort = int.Parse(m_txtPort.Text.Trim())
			};
			Edc_InterfaceAddress[] configInterfaceAddresses = m_ConfigInterfaceAddresses;
			foreach (Edc_InterfaceAddress item in configInterfaceAddresses)
			{
				if (address.Pro_strClientIP.Equals(item.Pro_strClientIP))
				{
					MessageBox.Show("IP is already exist...");
					return;
				}
			}
			m_ConfigInterfaceAddresses.ToList().Add(address);
			objectListView1.AddObject(address);
			objectListView1.EnsureModelVisible(address);
		}
		catch (Exception ex)
		{
			string a_strMessage = "Failed to Add Item...Details:'" + ex.Message + "'";
			MessageBox.Show(a_strMessage);
			m_logger.Error(a_strMessage, null, "m_btnAdd_Click", 120);
		}
	}

	private void m_btnRemove_Click(object sender, EventArgs e)
	{
		if (objectListView1.SelectedObject != null)
		{
			Edc_InterfaceAddress item = (Edc_InterfaceAddress)objectListView1.SelectedObject;
			m_ConfigInterfaceAddresses.ToList().Remove(item);
			objectListView1.RemoveObject(item);
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_ConfigBase, bool i_blnShowMessagebox = true)
	{
		try
		{
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
		this.m_btnDefault = new System.Windows.Forms.Button();
		this.m_btnTest = new System.Windows.Forms.Button();
		this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
		this.olvColumn1 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn2 = new BrightIdeasSoftware.OLVColumn();
		this.m_btnAdd = new System.Windows.Forms.Button();
		this.m_btnRemove = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.objectListView1).BeginInit();
		base.SuspendLayout();
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(26, 52);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(44, 20);
		this.label4.TabIndex = 22;
		this.label4.Text = "Host";
		this.m_txtHost.Location = new System.Drawing.Point(76, 50);
		this.m_txtHost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtHost.Name = "m_txtHost";
		this.m_txtHost.Size = new System.Drawing.Size(116, 26);
		this.m_txtHost.TabIndex = 21;
		this.m_txtHost.Text = "7.24.27.134";
		this.m_txtPort.Location = new System.Drawing.Point(257, 50);
		this.m_txtPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtPort.Name = "m_txtPort";
		this.m_txtPort.Size = new System.Drawing.Size(116, 26);
		this.m_txtPort.TabIndex = 19;
		this.m_txtPort.Text = "12121";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(211, 52);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(40, 20);
		this.label2.TabIndex = 20;
		this.label2.Text = "Port";
		this.m_btnDefault.Location = new System.Drawing.Point(497, 18);
		this.m_btnDefault.Margin = new System.Windows.Forms.Padding(2);
		this.m_btnDefault.Name = "m_btnDefault";
		this.m_btnDefault.Size = new System.Drawing.Size(70, 35);
		this.m_btnDefault.TabIndex = 23;
		this.m_btnDefault.Text = "Default";
		this.m_btnDefault.UseVisualStyleBackColor = true;
		this.m_btnDefault.Click += new System.EventHandler(btn_Default_Click);
		this.m_btnTest.Location = new System.Drawing.Point(423, 18);
		this.m_btnTest.Margin = new System.Windows.Forms.Padding(2);
		this.m_btnTest.Name = "m_btnTest";
		this.m_btnTest.Size = new System.Drawing.Size(70, 35);
		this.m_btnTest.TabIndex = 24;
		this.m_btnTest.Text = "Test";
		this.m_btnTest.UseVisualStyleBackColor = true;
		this.m_btnTest.Click += new System.EventHandler(m_btnTest_Click);
		this.objectListView1.AllColumns.Add(this.olvColumn1);
		this.objectListView1.AllColumns.Add(this.olvColumn2);
		this.objectListView1.CellEditUseWholeCell = false;
		this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.olvColumn1, this.olvColumn2 });
		this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
		this.objectListView1.FullRowSelect = true;
		this.objectListView1.HideSelection = false;
		this.objectListView1.Location = new System.Drawing.Point(10, 104);
		this.objectListView1.Margin = new System.Windows.Forms.Padding(10);
		this.objectListView1.Name = "objectListView1";
		this.objectListView1.ShowGroups = false;
		this.objectListView1.Size = new System.Drawing.Size(557, 483);
		this.objectListView1.TabIndex = 25;
		this.objectListView1.UseCompatibleStateImageBehavior = false;
		this.objectListView1.View = System.Windows.Forms.View.Details;
		this.olvColumn1.AspectName = "m_strClientIP";
		this.olvColumn1.Text = "ClientIP";
		this.olvColumn1.Width = 200;
		this.olvColumn2.AspectName = "m_strClientPort";
		this.olvColumn2.Text = "ClientPort";
		this.olvColumn2.Width = 200;
		this.m_btnAdd.Location = new System.Drawing.Point(423, 57);
		this.m_btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.m_btnAdd.Name = "m_btnAdd";
		this.m_btnAdd.Size = new System.Drawing.Size(70, 35);
		this.m_btnAdd.TabIndex = 27;
		this.m_btnAdd.Text = "Add";
		this.m_btnAdd.UseVisualStyleBackColor = true;
		this.m_btnAdd.Click += new System.EventHandler(m_btnAdd_Click);
		this.m_btnRemove.Location = new System.Drawing.Point(497, 57);
		this.m_btnRemove.Margin = new System.Windows.Forms.Padding(2);
		this.m_btnRemove.Name = "m_btnRemove";
		this.m_btnRemove.Size = new System.Drawing.Size(70, 35);
		this.m_btnRemove.TabIndex = 26;
		this.m_btnRemove.Text = "Remove";
		this.m_btnRemove.UseVisualStyleBackColor = true;
		this.m_btnRemove.Click += new System.EventHandler(m_btnRemove_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 20f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.m_btnAdd);
		base.Controls.Add(this.m_btnRemove);
		base.Controls.Add(this.objectListView1);
		base.Controls.Add(this.m_btnTest);
		base.Controls.Add(this.m_btnDefault);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.m_txtHost);
		base.Controls.Add(this.m_txtPort);
		base.Controls.Add(this.label2);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclInterfaceServer";
		base.Size = new System.Drawing.Size(575, 595);
		((System.ComponentModel.ISupportInitialize)this.objectListView1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
