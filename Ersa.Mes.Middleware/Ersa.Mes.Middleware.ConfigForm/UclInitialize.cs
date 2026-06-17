using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclInitialize : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private Label label4;

	private TextBox m_txtInitiailize;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private ObjectListView m_olvInitialize;

	private OLVColumn olvColumn1;

	private OLVColumn olvColumn2;

	private OLVColumn olvColumn3;

	private OLVColumn olvColumn4;

	private CheckBox m_chkActive;

	private TextBox m_txtEvent;

	private Label label5;

	private TextBox m_txtInterval;

	private Label label3;

	private TextBox m_txtRepetitions;

	private Label label2;

	private TextBox m_txtName;

	private Label label1;

	public int m_i32FormID { get; set; } = 409;


	public string Pro_FormName => "Initialize";

	private Edc_ConfigBase m_ConfigBase { get; set; }

	private Struct_Initialize m_edcInitialize { get; set; }

	public UclInitialize(Edc_ConfigBase i_ConfigBase)
	{
		InitializeComponent();
		m_ConfigBase = i_ConfigBase;
		m_txtInitiailize.Text = i_ConfigBase.m_edcFilesPath.m_strPathInitialize;
		if (!string.IsNullOrEmpty(i_ConfigBase.m_edcFilesPath.m_strPathInitialize))
		{
			Sub_LoadInitialize();
		}
	}

	public void Sub_LoadInitialize()
	{
		try
		{
			m_edcInitialize = m_ConfigBase.m_edcFilesPath.m_strPathInitialize.Fun_edcDeserializeByFilePath<Struct_Initialize>();
			m_txtName.Text = m_edcInitialize.m_strName;
			m_txtInterval.Text = m_edcInitialize.m_i32Interval.ToString();
			m_txtRepetitions.Text = m_edcInitialize.m_i32RepetationNumber.ToString();
			m_txtEvent.Text = m_edcInitialize.m_strEvent;
			m_olvInitialize.SetObjects(m_edcInitialize.m_sttParameterWithoutValue);
		}
		catch
		{
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_edcConfigBase, bool i_blnShowMessagebox = true)
	{
		try
		{
			m_edcInitialize.m_strName = m_txtName.Text;
			int.TryParse(m_txtInterval.Text, out var result);
			m_edcInitialize.m_i32Interval = result;
			int.TryParse(m_txtRepetitions.Text, out var result2);
			m_edcInitialize.m_i32RepetationNumber = result2;
			m_edcInitialize.m_strEvent = m_txtEvent.Text;
			string contents = SerializerHelper.Fun_strSerializerModel<Struct_Initialize>(m_edcInitialize);
			File.WriteAllText(m_ConfigBase.m_edcFilesPath.m_strPathInitialize, contents);
			return true;
		}
		catch
		{
			return false;
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
		this.label4 = new System.Windows.Forms.Label();
		this.m_txtInitiailize = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.m_olvInitialize = new BrightIdeasSoftware.ObjectListView();
		this.olvColumn1 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn2 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn3 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn4 = new BrightIdeasSoftware.OLVColumn();
		this.m_chkActive = new System.Windows.Forms.CheckBox();
		this.m_txtName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.m_txtRepetitions = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.m_txtInterval = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_txtEvent = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvInitialize).BeginInit();
		base.SuspendLayout();
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(99, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 17);
		this.label4.TabIndex = 22;
		this.label4.Text = "Initiailize";
		this.m_txtInitiailize.Enabled = false;
		this.m_txtInitiailize.Location = new System.Drawing.Point(162, 20);
		this.m_txtInitiailize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtInitiailize.Name = "m_txtInitiailize";
		this.m_txtInitiailize.Size = new System.Drawing.Size(633, 23);
		this.m_txtInitiailize.TabIndex = 21;
		this.groupBox1.Controls.Add(this.m_txtEvent);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.m_txtInterval);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.m_txtRepetitions);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.m_txtName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.m_chkActive);
		this.groupBox1.Controls.Add(this.m_txtInitiailize);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(5, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(832, 99);
		this.groupBox1.TabIndex = 25;
		this.groupBox1.TabStop = false;
		this.groupBox2.Controls.Add(this.m_olvInitialize);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox2.Location = new System.Drawing.Point(5, 104);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(832, 655);
		this.groupBox2.TabIndex = 26;
		this.groupBox2.TabStop = false;
		this.m_olvInitialize.AllColumns.Add(this.olvColumn1);
		this.m_olvInitialize.AllColumns.Add(this.olvColumn2);
		this.m_olvInitialize.AllColumns.Add(this.olvColumn3);
		this.m_olvInitialize.AllColumns.Add(this.olvColumn4);
		this.m_olvInitialize.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvInitialize.CellEditUseWholeCell = false;
		this.m_olvInitialize.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.olvColumn1, this.olvColumn2, this.olvColumn3, this.olvColumn4 });
		this.m_olvInitialize.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvInitialize.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvInitialize.Font = new System.Drawing.Font("阿里巴巴普惠体 2.0 55 Regular", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.m_olvInitialize.FullRowSelect = true;
		this.m_olvInitialize.HideSelection = false;
		this.m_olvInitialize.Location = new System.Drawing.Point(3, 19);
		this.m_olvInitialize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_olvInitialize.Name = "m_olvInitialize";
		this.m_olvInitialize.ShowGroups = false;
		this.m_olvInitialize.ShowImagesOnSubItems = true;
		this.m_olvInitialize.Size = new System.Drawing.Size(826, 633);
		this.m_olvInitialize.TabIndex = 5;
		this.m_olvInitialize.UseCompatibleStateImageBehavior = false;
		this.m_olvInitialize.View = System.Windows.Forms.View.Details;
		this.olvColumn1.AspectName = "m_blnActive";
		this.olvColumn1.CheckBoxes = true;
		this.olvColumn1.Text = "Active";
		this.olvColumn2.AspectName = "m_strName";
		this.olvColumn2.Text = "Name";
		this.olvColumn2.Width = 260;
		this.olvColumn3.AspectName = "m_bytTrackNumber";
		this.olvColumn3.Text = "Track";
		this.olvColumn4.AspectName = "m_strDisplay";
		this.olvColumn4.Text = "Display";
		this.olvColumn4.Width = 300;
		this.m_chkActive.AutoSize = true;
		this.m_chkActive.Location = new System.Drawing.Point(24, 22);
		this.m_chkActive.Name = "m_chkActive";
		this.m_chkActive.Size = new System.Drawing.Size(61, 21);
		this.m_chkActive.TabIndex = 23;
		this.m_chkActive.Text = "Active";
		this.m_chkActive.UseVisualStyleBackColor = true;
		this.m_txtName.Location = new System.Drawing.Point(110, 59);
		this.m_txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtName.Name = "m_txtName";
		this.m_txtName.Size = new System.Drawing.Size(158, 23);
		this.m_txtName.TabIndex = 24;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(24, 62);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 17);
		this.label1.TabIndex = 25;
		this.label1.Text = "GroupName";
		this.m_txtRepetitions.Location = new System.Drawing.Point(364, 59);
		this.m_txtRepetitions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtRepetitions.Name = "m_txtRepetitions";
		this.m_txtRepetitions.Size = new System.Drawing.Size(97, 23);
		this.m_txtRepetitions.TabIndex = 26;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(285, 62);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(73, 17);
		this.label2.TabIndex = 27;
		this.label2.Text = "Repetitions";
		this.m_txtInterval.Location = new System.Drawing.Point(534, 59);
		this.m_txtInterval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtInterval.Name = "m_txtInterval";
		this.m_txtInterval.Size = new System.Drawing.Size(97, 23);
		this.m_txtInterval.TabIndex = 28;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(477, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(51, 17);
		this.label3.TabIndex = 29;
		this.label3.Text = "Interval";
		this.m_txtEvent.Location = new System.Drawing.Point(698, 59);
		this.m_txtEvent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtEvent.Name = "m_txtEvent";
		this.m_txtEvent.Size = new System.Drawing.Size(97, 23);
		this.m_txtEvent.TabIndex = 30;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(653, 62);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(39, 17);
		this.label5.TabIndex = 31;
		this.label5.Text = "Event";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclInitialize";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(842, 764);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.m_olvInitialize).EndInit();
		base.ResumeLayout(false);
	}
}
