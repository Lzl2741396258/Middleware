using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclMesTask : UserControl, Inf_ConfigForm
{
	private List<Edc_MesTaskAttributes> m_lstConfigMesTask;

	private IContainer components = null;

	private GroupBox groupBox1;

	private ObjectListView m_olvMesTask;

	private Button m_btnAdd;

	private TextBox m_txtName;

	private Label label1;

	private TextBox m_txtDelayTime;

	private Label label2;

	private TextBox m_txtInterval;

	private Label label4;

	private TextBox m_txtRepetition;

	private Label label3;

	private Button m_btnRemove;

	public int m_i32FormID { get; set; } = 404;


	public string Pro_FormName => "MesTask";

	public UclMesTask(Edc_ConfigBase i_Config)
	{
		InitializeComponent();
		Sub_Initialize();
		m_lstConfigMesTask = i_Config.ma_MesTask.ToList();
		m_olvMesTask.SetObjects(m_lstConfigMesTask);
	}

	public void Sub_Initialize()
	{
		m_olvMesTask.EmptyListMsg = "Please check the config file...";
		m_olvMesTask.HeaderStyle = ColumnHeaderStyle.Nonclickable;
		m_olvMesTask.ShowImagesOnSubItems = true;
		m_olvMesTask.ShowGroups = false;
		m_olvMesTask.FullRowSelect = true;
		m_olvMesTask.Scrollable = true;
		m_olvMesTask.View = View.Details;
		m_olvMesTask.Columns.Clear();
		PropertyInfo[] properties = typeof(Edc_MesTaskAttributes).GetProperties();
		foreach (PropertyInfo item in properties)
		{
			string title = item.Name;
			DescriptionAttribute result = (DescriptionAttribute)item.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault();
			string a_strDescriptionName = result.Description;
			OLVColumn column = new OLVColumn(a_strDescriptionName, item.Name);
			column.Width = 150;
			m_olvMesTask.Columns.Add(column);
		}
	}

	private void m_btnAdd_Click(object sender, EventArgs e)
	{
		int.TryParse(m_txtInterval.Text, out var a_i32Interval);
		int.TryParse(m_txtDelayTime.Text, out var a_i32DelayTime);
		int.TryParse(m_txtRepetition.Text, out var a_i32Repetition);
		m_lstConfigMesTask.Add(new Edc_MesTaskAttributes
		{
			Pro_blnActive = true,
			Pro_strName = m_txtName.Text,
			Pro_i32Interval = a_i32Interval,
			Pro_i32DelayTime = a_i32DelayTime,
			Pro_i32Repetition = a_i32Repetition
		});
		m_olvMesTask.SetObjects(m_lstConfigMesTask);
	}

	private void m_btnRemove_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < m_olvMesTask.SelectedObjects.Count; i++)
			{
				m_lstConfigMesTask.Remove((Edc_MesTaskAttributes)m_olvMesTask.SelectedObjects[i]);
			}
			m_olvMesTask.RemoveObjects(m_olvMesTask.SelectedObjects);
		}
		catch (Exception ex)
		{
			MessageBox.Show(MethodBase.GetCurrentMethod().Name + "  Details:" + ex.Message);
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		try
		{
			i_Config.ma_MesTask = m_lstConfigMesTask.ToArray();
		}
		catch (Exception ex)
		{
			MessageBox.Show(MethodBase.GetCurrentMethod().Name + "  Details:" + ex.Message);
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_btnRemove = new System.Windows.Forms.Button();
		this.m_txtInterval = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.m_txtRepetition = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_txtDelayTime = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.m_txtName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.m_btnAdd = new System.Windows.Forms.Button();
		this.m_olvMesTask = new BrightIdeasSoftware.ObjectListView();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvMesTask).BeginInit();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.m_btnRemove);
		this.groupBox1.Controls.Add(this.m_txtInterval);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.m_txtRepetition);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.m_txtDelayTime);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.m_txtName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.m_btnAdd);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(848, 135);
		this.groupBox1.TabIndex = 4;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Control";
		this.m_btnRemove.Location = new System.Drawing.Point(702, 77);
		this.m_btnRemove.Name = "m_btnRemove";
		this.m_btnRemove.Size = new System.Drawing.Size(80, 30);
		this.m_btnRemove.TabIndex = 9;
		this.m_btnRemove.Text = "Remove";
		this.m_btnRemove.UseVisualStyleBackColor = true;
		this.m_btnRemove.Click += new System.EventHandler(m_btnRemove_Click);
		this.m_txtInterval.Location = new System.Drawing.Point(452, 46);
		this.m_txtInterval.Name = "m_txtInterval";
		this.m_txtInterval.Size = new System.Drawing.Size(244, 21);
		this.m_txtInterval.TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(400, 49);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 15);
		this.label4.TabIndex = 7;
		this.label4.Text = "Interval";
		this.m_txtRepetition.Location = new System.Drawing.Point(452, 82);
		this.m_txtRepetition.Name = "m_txtRepetition";
		this.m_txtRepetition.Size = new System.Drawing.Size(244, 21);
		this.m_txtRepetition.TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(383, 84);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 5;
		this.label3.Text = "Repetition";
		this.m_txtDelayTime.Location = new System.Drawing.Point(102, 82);
		this.m_txtDelayTime.Name = "m_txtDelayTime";
		this.m_txtDelayTime.Size = new System.Drawing.Size(244, 21);
		this.m_txtDelayTime.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(30, 85);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(66, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "DelayTime";
		this.m_txtName.Location = new System.Drawing.Point(102, 46);
		this.m_txtName.Name = "m_txtName";
		this.m_txtName.Size = new System.Drawing.Size(244, 21);
		this.m_txtName.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(55, 49);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(41, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Name";
		this.m_btnAdd.Location = new System.Drawing.Point(702, 41);
		this.m_btnAdd.Name = "m_btnAdd";
		this.m_btnAdd.Size = new System.Drawing.Size(80, 30);
		this.m_btnAdd.TabIndex = 0;
		this.m_btnAdd.Text = "Add";
		this.m_btnAdd.UseVisualStyleBackColor = true;
		this.m_btnAdd.Click += new System.EventHandler(m_btnAdd_Click);
		this.m_olvMesTask.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvMesTask.CellEditUseWholeCell = false;
		this.m_olvMesTask.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvMesTask.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvMesTask.Font = new System.Drawing.Font("阿里巴巴普惠体 2.0 55 Regular", 8.25f);
		this.m_olvMesTask.FullRowSelect = true;
		this.m_olvMesTask.HideSelection = false;
		this.m_olvMesTask.Location = new System.Drawing.Point(0, 135);
		this.m_olvMesTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_olvMesTask.Name = "m_olvMesTask";
		this.m_olvMesTask.ShowGroups = false;
		this.m_olvMesTask.ShowImagesOnSubItems = true;
		this.m_olvMesTask.Size = new System.Drawing.Size(848, 606);
		this.m_olvMesTask.TabIndex = 5;
		this.m_olvMesTask.UseCompatibleStateImageBehavior = false;
		this.m_olvMesTask.View = System.Windows.Forms.View.Details;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.m_olvMesTask);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclMesTask";
		base.Size = new System.Drawing.Size(848, 741);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvMesTask).EndInit();
		base.ResumeLayout(false);
	}
}
