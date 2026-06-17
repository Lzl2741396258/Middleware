using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Interfaces;
using Ersa.Mes.Middleware.MesTaskFolder;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclProcess : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private GroupBox groupBox1;

	private Label label14;

	private ComboBox m_cbxOutputDataFormat;

	private CheckBox m_chkCheckRepeat;

	private Label label11;

	private ComboBox m_cbxInputDataFormat;

	private Label label10;

	private ComboBox m_cbxOutput;

	private Label label9;

	private ComboBox m_cbxInput;

	private Button m_btnMinus;

	private Button m_btnAdd;

	private TextBox m_txtName;

	private Label label1;

	private ObjectListView m_olvProcess;

	private OLVColumn olvColumn2;

	private OLVColumn olvColumn1;

	private OLVColumn olvColumn3;

	private OLVColumn olvColumn4;

	private OLVColumn olvColumn5;

	private OLVColumn olvColumn6;

	private OLVColumn olvColumn7;

	public int m_i32FormID { get; set; } = 405;


	public string Pro_FormName => "Process";

	private Edc_ConfigBase m_Config { get; set; }

	private List<Edc_Process> m_lstProcess { get; set; }

	public UclProcess(Edc_ConfigBase m_edcConfigBase)
	{
		InitializeComponent();
		m_Config = m_edcConfigBase;
		m_lstProcess = m_Config.m_clsDevice.m_lstProcess;
		Sub_SetDefault();
	}

	public void Sub_SetDefault()
	{
		m_olvProcess.CellEditUseWholeCell = true;
		m_olvProcess.SetObjects(m_lstProcess);
		Sub_SetControlsDefault();
		if (m_lstProcess.Count > 0)
		{
			m_olvProcess.SelectedIndex = m_lstProcess.Count - 1;
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		int index = m_olvProcess.SelectedIndex;
		if (index >= 0)
		{
			m_lstProcess[index].m_strName = m_txtName.Text.Trim();
			m_lstProcess[index].m_strInput = m_cbxInput.Text.Trim();
			m_lstProcess[index].m_strInputDataFormat = m_cbxInputDataFormat.Text;
			m_lstProcess[index].m_blnInputDataRepeat = m_chkCheckRepeat.Checked;
			m_lstProcess[index].m_strOutput = m_cbxOutput.Text;
			m_lstProcess[index].m_strOutputDataFormat = m_cbxOutputDataFormat.Text;
		}
		m_Config.m_clsDevice.m_lstProcess = m_lstProcess;
		m_olvProcess.SetObjects(m_lstProcess);
		return true;
	}

	private void Sub_SetControlsDefault()
	{
		m_cbxInput.Items.Clear();
		IEnumerable<string> a_lstName = Fun_lstGetPortName();
		ComboBox.ObjectCollection items = m_cbxInput.Items;
		object[] portNames = SerialPort.GetPortNames();
		items.AddRange(portNames);
		IEnumerable<string> a_lstName2 = m_Config.m_clsDevice.m_lstSockets.Select((Edc_Socket t) => t.m_strIP + " " + t.m_i32Port);
		ComboBox.ObjectCollection items2 = m_cbxInput.Items;
		portNames = a_lstName2.ToArray();
		items2.AddRange(portNames);
		m_cbxInputDataFormat.Items.Clear();
		ComboBox.ObjectCollection items3 = m_cbxInputDataFormat.Items;
		portNames = Enum.GetNames(typeof(Enum_DataFormat));
		items3.AddRange(portNames);
		m_chkCheckRepeat.Checked = false;
		m_cbxOutput.Items.Clear();
		ComboBox.ObjectCollection items4 = m_cbxOutput.Items;
		portNames = new string[1] { "" };
		items4.AddRange(portNames);
		ComboBox.ObjectCollection items5 = m_cbxOutput.Items;
		portNames = a_lstName.ToArray();
		items5.AddRange(portNames);
		ComboBox.ObjectCollection items6 = m_cbxOutput.Items;
		portNames = a_lstName2.ToArray();
		items6.AddRange(portNames);
		string[] portNames2 = SerialPort.GetPortNames();
		foreach (string item in portNames2)
		{
			if (!m_cbxOutput.Items.Contains(item))
			{
				m_cbxOutput.Items.Add(item);
			}
		}
		m_cbxOutputDataFormat.Items.Clear();
		ComboBox.ObjectCollection items7 = m_cbxOutputDataFormat.Items;
		portNames = Enum.GetNames(typeof(Enum_DataFormat));
		items7.AddRange(portNames);
	}

	private void m_btnAdd_Click(object sender, EventArgs e)
	{
		m_lstProcess.Add(new Edc_Process
		{
			m_strName = m_txtName.Text,
			m_strInput = m_cbxInput.Text,
			m_strOutput = m_cbxOutput.Text,
			m_blnInputDataRepeat = false,
			m_blnOutputDataRepeat = false,
			m_strInputDataFormat = m_cbxInputDataFormat.Text,
			m_strOutputDataFormat = m_cbxOutputDataFormat.Text
		});
		Sub_SetDefault();
		m_olvProcess.SelectedIndex = m_lstProcess.Count - 1;
	}

	private void m_btnMinus_Click(object sender, EventArgs e)
	{
		int index = m_olvProcess.SelectedIndex;
		if (index >= 0)
		{
			m_lstProcess.RemoveAt(index);
		}
		Sub_SetDefault();
	}

	private void m_olvProcess_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (m_olvProcess.SelectedObject != null)
		{
			Edc_Process process = (Edc_Process)m_olvProcess.SelectedObject;
			m_txtName.Text = process.m_strName;
			m_cbxInput.SelectedItem = process.m_strInput;
			m_cbxInputDataFormat.SelectedItem = process.m_strInputDataFormat;
			m_chkCheckRepeat.Checked = process.m_blnInputDataRepeat;
			m_cbxOutput.SelectedItem = process.m_strOutput;
			m_cbxOutputDataFormat.SelectedItem = process.m_strOutputDataFormat;
		}
	}

	private void m_olvProcess_CellEditStarting(object sender, CellEditEventArgs e)
	{
		if (e.Column.AspectName.Equals("m_strInput", StringComparison.OrdinalIgnoreCase))
		{
			Edc_Process process = (Edc_Process)e.RowObject;
			ComboBox cb = new ComboBox();
			cb.Bounds = e.CellBounds;
			cb.Font = ((ObjectListView)sender).Font;
			cb.DropDownStyle = ComboBoxStyle.DropDownList;
			ComboBox.ObjectCollection items = cb.Items;
			object[] items2 = Fun_lstGetPortName().ToArray();
			items.AddRange(items2);
			cb.SelectedIndex = cb.FindString(process.m_strInput);
			cb.SelectedIndexChanged += delegate
			{
				process.m_strInput = cb.Text;
			};
			e.Control = cb;
		}
		if (e.Column.AspectName.Equals("m_strOutput", StringComparison.OrdinalIgnoreCase))
		{
			Edc_Process process2 = (Edc_Process)e.RowObject;
			ComboBox cb2 = new ComboBox();
			cb2.Bounds = e.CellBounds;
			cb2.Font = ((ObjectListView)sender).Font;
			cb2.DropDownStyle = ComboBoxStyle.DropDownList;
			ComboBox.ObjectCollection items3 = cb2.Items;
			object[] items2 = Fun_lstGetPortName().ToArray();
			items3.AddRange(items2);
			cb2.SelectedIndex = cb2.FindString(process2.m_strOutput);
			cb2.SelectedIndexChanged += delegate
			{
				process2.m_strOutput = cb2.Text;
			};
			e.Control = cb2;
		}
		if (e.Column.AspectName.Equals("m_strInputDataFormat", StringComparison.OrdinalIgnoreCase))
		{
			Edc_Process process3 = (Edc_Process)e.RowObject;
			ComboBox cb3 = new ComboBox();
			cb3.Bounds = e.CellBounds;
			cb3.Font = ((ObjectListView)sender).Font;
			cb3.DropDownStyle = ComboBoxStyle.DropDownList;
			ComboBox.ObjectCollection items4 = cb3.Items;
			object[] items2 = Enum.GetNames(typeof(Enum_DataFormat));
			items4.AddRange(items2);
			cb3.SelectedIndex = cb3.FindString(process3.m_strInputDataFormat);
			cb3.SelectedIndexChanged += delegate
			{
				process3.m_strInputDataFormat = cb3.Text;
			};
			e.Control = cb3;
		}
		if (e.Column.AspectName.Equals("m_strOutputDataFormat", StringComparison.OrdinalIgnoreCase))
		{
			Edc_Process process4 = (Edc_Process)e.RowObject;
			ComboBox cb4 = new ComboBox();
			cb4.Bounds = e.CellBounds;
			cb4.Font = ((ObjectListView)sender).Font;
			cb4.DropDownStyle = ComboBoxStyle.DropDownList;
			ComboBox.ObjectCollection items5 = cb4.Items;
			object[] items2 = Enum.GetNames(typeof(Enum_DataFormat));
			items5.AddRange(items2);
			cb4.SelectedIndex = cb4.FindString(process4.m_strOutputDataFormat);
			cb4.SelectedIndexChanged += delegate
			{
				process4.m_strOutputDataFormat = cb4.Text;
			};
			e.Control = cb4;
		}
	}

	private void m_olvProcess_CellEditFinishing(object sender, CellEditEventArgs e)
	{
		if (e.Column.AspectName.Equals("m_strInput", StringComparison.OrdinalIgnoreCase) || e.Column.AspectName.Equals("m_strOutput", StringComparison.OrdinalIgnoreCase) || e.Column.AspectName.Equals("m_strInputDataFormat", StringComparison.OrdinalIgnoreCase) || e.Column.AspectName.Equals("m_strOutputDataFormat", StringComparison.OrdinalIgnoreCase))
		{
			((ObjectListView)sender).RefreshItem(e.ListViewItem);
			e.Cancel = true;
		}
	}

	private IEnumerable<string> Fun_lstGetPortName()
	{
		List<string> list = new List<string>();
		list.AddRange(SerialPort.GetPortNames());
		IEnumerable<string> listNet = m_Config.m_clsDevice.m_lstSockets.Select((Edc_Socket t) => t.m_strIP + " " + t.m_i32Port);
		list.AddRange(listNet);
		return list;
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_txtName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.m_btnMinus = new System.Windows.Forms.Button();
		this.m_btnAdd = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.m_cbxOutputDataFormat = new System.Windows.Forms.ComboBox();
		this.m_chkCheckRepeat = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.m_cbxInputDataFormat = new System.Windows.Forms.ComboBox();
		this.label10 = new System.Windows.Forms.Label();
		this.m_cbxOutput = new System.Windows.Forms.ComboBox();
		this.label9 = new System.Windows.Forms.Label();
		this.m_cbxInput = new System.Windows.Forms.ComboBox();
		this.m_olvProcess = new BrightIdeasSoftware.ObjectListView();
		this.olvColumn2 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn1 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn3 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn4 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn5 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn6 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn7 = new BrightIdeasSoftware.OLVColumn();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvProcess).BeginInit();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.m_txtName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.m_btnMinus);
		this.groupBox1.Controls.Add(this.m_btnAdd);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.m_cbxOutputDataFormat);
		this.groupBox1.Controls.Add(this.m_chkCheckRepeat);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.m_cbxInputDataFormat);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.m_cbxOutput);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.m_cbxInput);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.groupBox1.Size = new System.Drawing.Size(848, 193);
		this.groupBox1.TabIndex = 58;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Process";
		this.m_txtName.Location = new System.Drawing.Point(98, 49);
		this.m_txtName.Name = "m_txtName";
		this.m_txtName.Size = new System.Drawing.Size(140, 23);
		this.m_txtName.TabIndex = 36;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(49, 52);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(43, 17);
		this.label1.TabIndex = 35;
		this.label1.Text = "Name";
		this.m_btnMinus.Location = new System.Drawing.Point(509, 128);
		this.m_btnMinus.Name = "m_btnMinus";
		this.m_btnMinus.Size = new System.Drawing.Size(80, 30);
		this.m_btnMinus.TabIndex = 34;
		this.m_btnMinus.Text = "Remove";
		this.m_btnMinus.UseVisualStyleBackColor = true;
		this.m_btnMinus.Click += new System.EventHandler(m_btnMinus_Click);
		this.m_btnAdd.Location = new System.Drawing.Point(509, 92);
		this.m_btnAdd.Name = "m_btnAdd";
		this.m_btnAdd.Size = new System.Drawing.Size(80, 30);
		this.m_btnAdd.TabIndex = 33;
		this.m_btnAdd.Text = "Add";
		this.m_btnAdd.UseVisualStyleBackColor = true;
		this.m_btnAdd.Click += new System.EventHandler(m_btnAdd_Click);
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(263, 138);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 17);
		this.label14.TabIndex = 32;
		this.label14.Text = "DataFormat";
		this.m_cbxOutputDataFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxOutputDataFormat.FormattingEnabled = true;
		this.m_cbxOutputDataFormat.Location = new System.Drawing.Point(345, 134);
		this.m_cbxOutputDataFormat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.m_cbxOutputDataFormat.Name = "m_cbxOutputDataFormat";
		this.m_cbxOutputDataFormat.Size = new System.Drawing.Size(140, 25);
		this.m_cbxOutputDataFormat.TabIndex = 31;
		this.m_chkCheckRepeat.AutoSize = true;
		this.m_chkCheckRepeat.Location = new System.Drawing.Point(266, 52);
		this.m_chkCheckRepeat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.m_chkCheckRepeat.Name = "m_chkCheckRepeat";
		this.m_chkCheckRepeat.Size = new System.Drawing.Size(103, 21);
		this.m_chkCheckRepeat.TabIndex = 29;
		this.m_chkCheckRepeat.Text = "CheckRepeat";
		this.m_chkCheckRepeat.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(263, 96);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(76, 17);
		this.label11.TabIndex = 28;
		this.label11.Text = "DataFormat";
		this.m_cbxInputDataFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxInputDataFormat.FormattingEnabled = true;
		this.m_cbxInputDataFormat.Location = new System.Drawing.Point(345, 92);
		this.m_cbxInputDataFormat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.m_cbxInputDataFormat.Name = "m_cbxInputDataFormat";
		this.m_cbxInputDataFormat.Size = new System.Drawing.Size(140, 25);
		this.m_cbxInputDataFormat.TabIndex = 27;
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(45, 140);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(48, 17);
		this.label10.TabIndex = 25;
		this.label10.Text = "Output";
		this.m_cbxOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxOutput.FormattingEnabled = true;
		this.m_cbxOutput.Location = new System.Drawing.Point(98, 135);
		this.m_cbxOutput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.m_cbxOutput.Name = "m_cbxOutput";
		this.m_cbxOutput.Size = new System.Drawing.Size(140, 25);
		this.m_cbxOutput.TabIndex = 24;
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(54, 96);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(38, 17);
		this.label9.TabIndex = 23;
		this.label9.Text = "Input";
		this.m_cbxInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxInput.FormattingEnabled = true;
		this.m_cbxInput.Location = new System.Drawing.Point(98, 91);
		this.m_cbxInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		this.m_cbxInput.Name = "m_cbxInput";
		this.m_cbxInput.Size = new System.Drawing.Size(140, 25);
		this.m_cbxInput.TabIndex = 22;
		this.m_olvProcess.AllColumns.Add(this.olvColumn2);
		this.m_olvProcess.AllColumns.Add(this.olvColumn1);
		this.m_olvProcess.AllColumns.Add(this.olvColumn3);
		this.m_olvProcess.AllColumns.Add(this.olvColumn4);
		this.m_olvProcess.AllColumns.Add(this.olvColumn5);
		this.m_olvProcess.AllColumns.Add(this.olvColumn6);
		this.m_olvProcess.AllColumns.Add(this.olvColumn7);
		this.m_olvProcess.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvProcess.CellEditUseWholeCell = false;
		this.m_olvProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[7] { this.olvColumn2, this.olvColumn1, this.olvColumn3, this.olvColumn4, this.olvColumn5, this.olvColumn6, this.olvColumn7 });
		this.m_olvProcess.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvProcess.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvProcess.FullRowSelect = true;
		this.m_olvProcess.HideSelection = false;
		this.m_olvProcess.Location = new System.Drawing.Point(0, 193);
		this.m_olvProcess.Name = "m_olvProcess";
		this.m_olvProcess.ShowGroups = false;
		this.m_olvProcess.ShowImagesOnSubItems = true;
		this.m_olvProcess.Size = new System.Drawing.Size(848, 313);
		this.m_olvProcess.TabIndex = 59;
		this.m_olvProcess.UseCompatibleStateImageBehavior = false;
		this.m_olvProcess.View = System.Windows.Forms.View.Details;
		this.m_olvProcess.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(m_olvProcess_CellEditFinishing);
		this.m_olvProcess.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(m_olvProcess_CellEditStarting);
		this.m_olvProcess.SelectedIndexChanged += new System.EventHandler(m_olvProcess_SelectedIndexChanged);
		this.olvColumn2.AspectName = "m_strName";
		this.olvColumn2.Text = "Name";
		this.olvColumn2.Width = 100;
		this.olvColumn1.AspectName = "m_strInput";
		this.olvColumn1.Text = "Input";
		this.olvColumn1.Width = 150;
		this.olvColumn3.AspectName = "m_strInputDataFormat";
		this.olvColumn3.Text = "DataFormat";
		this.olvColumn3.Width = 100;
		this.olvColumn4.AspectName = "m_blnInputDataRepeat";
		this.olvColumn4.Text = "CheckRepeat";
		this.olvColumn4.Width = 108;
		this.olvColumn5.AspectName = "m_strOutput";
		this.olvColumn5.Text = "Output";
		this.olvColumn5.Width = 150;
		this.olvColumn6.AspectName = "m_strOutputDataFormat";
		this.olvColumn6.Text = "DataFormat";
		this.olvColumn6.Width = 117;
		this.olvColumn7.AspectName = "m_blnOutputDataRepeat";
		this.olvColumn7.Text = "DataRepeat";
		this.olvColumn7.Width = 111;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.m_olvProcess);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclProcess";
		base.Size = new System.Drawing.Size(848, 506);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvProcess).EndInit();
		base.ResumeLayout(false);
	}
}
