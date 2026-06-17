using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.ConfigForm;
using Ersa.Mes.Middleware.Interfaces;
using Ersa.Mes.Middleware.Properties;

namespace Ersa.Mes.Middleware;

public class P_frmSettingErsa : Form
{
	public string m_strConfigPath = string.Empty;

	public bool[] m_blnPath = new bool[6];

	private UserControl m_form;

	private IContainer components = null;

	private ToolStripButton m_tsbtnExit;

	private ToolStripButton m_tsbtnSave;

	public ToolStrip toolStrip1;

	public StatusStrip statusStrip1;

	private SplitContainer splitContainer1;

	private TreeView m_tvwMenu;

	private Inf_Logger m_edcLogger { get; }

	public Edc_ConfigBase m_edcConfigBase { get; set; }

	public Inf_ConfigForm m_edcCurrentForm { get; set; }

	public P_frmSettingErsa()
	{
		InitializeComponent();
	}

	public P_frmSettingErsa(Edc_ConfigBase i_Config, Inf_Logger i_edcLogger)
	{
		InitializeComponent();
		m_edcConfigBase = i_Config;
		m_edcLogger = i_edcLogger;
		Sub_LoadTree();
	}

	public virtual void m_tsbtnSave_Click(object sender, EventArgs e)
	{
		bool result = Sub_SaveConfig();
		try
		{
			if (Fun_blnWriteToConfigFile())
			{
				MessageBox.Show("Save Config File Successed...");
			}
		}
		catch (Exception ex)
		{
			m_edcLogger.Error("Save Config Error...Details:'" + ex.Message + "'", null, "m_tsbtnSave_Click", 78);
			MessageBox.Show("Save Config Error...Result:'" + ex.Message + "'");
		}
	}

	public virtual bool Sub_SaveConfig()
	{
		if (m_edcCurrentForm == null)
		{
			MessageBox.Show("Please select the config type...");
			return false;
		}
		return m_edcCurrentForm.Fun_blnSave(m_edcConfigBase);
	}

	public virtual bool Fun_blnWriteToConfigFile()
	{
		return Edc_OperationConfig.Fun_WriteConfig<Edc_ConfigBase>(m_strConfigPath, m_edcConfigBase);
	}

	private void m_tvwMenu_AfterSelect(object sender, TreeViewEventArgs e)
	{
		m_form = null;
		switch ((Enum_ConfigForm)Enum.Parse(typeof(Enum_ConfigForm), e.Node.Text))
		{
		case Enum_ConfigForm.Basic:
			m_form = new UclBasic(m_edcConfigBase);
			break;
		case Enum_ConfigForm.Interface:
			m_form = new UclInterface(m_edcConfigBase);
			break;
		case Enum_ConfigForm.Path:
			m_form = new UclPath(m_edcConfigBase, m_blnPath[0], m_blnPath[1], m_blnPath[2], m_blnPath[3], m_blnPath[4], m_blnPath[5]);
			break;
		case Enum_ConfigForm.MesFunction:
			m_form = new UclMesFunction(m_edcConfigBase);
			break;
		case Enum_ConfigForm.MesTask:
			m_form = new UclMesTask(m_edcConfigBase);
			break;
		case Enum_ConfigForm.Initialize:
			m_form = new UclInitialize(m_edcConfigBase);
			break;
		case Enum_ConfigForm.Device:
			m_form = new UclDevice(m_edcConfigBase);
			break;
		case Enum_ConfigForm.Process:
			m_form = new UclProcess(m_edcConfigBase);
			break;
		case Enum_ConfigForm.Database:
			m_form = new UclDatabase(m_edcConfigBase);
			break;
		case Enum_ConfigForm.PLC:
			m_form = new UclPLC(m_edcConfigBase, m_edcLogger);
			break;
		case Enum_ConfigForm.MES:
			m_form = new UclAutoSettings(m_edcConfigBase, m_edcLogger);
			break;
		}
		if (m_form != null)
		{
			m_edcCurrentForm = (Inf_ConfigForm)m_form;
			splitContainer1.Panel2.Controls.Clear();
			splitContainer1.Panel2.Controls.Add(m_form);
			m_form.Dock = DockStyle.Fill;
			m_form.Show();
		}
	}

	private void tsbtnExit_Click(object sender, EventArgs e)
	{
		m_form?.Dispose();
		Close();
	}

	private void Sub_LoadTree()
	{
		m_tvwMenu.Nodes.Clear();
		string[] names = Enum.GetNames(typeof(Enum_ConfigForm));
		foreach (string item in names)
		{
			m_tvwMenu.Nodes.Add(new TreeNode(item));
		}
	}

	public virtual void Sub_ShowPath(bool i_blnProtocol = false, bool i_blnZtxt = false, bool i_blnTrend = false, bool i_blnBibs = false, bool i_blnInitialize = false, bool i_blnData = false)
	{
		m_blnPath = new bool[6] { i_blnProtocol, i_blnZtxt, i_blnTrend, i_blnBibs, i_blnInitialize, i_blnData };
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
		System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点0");
		System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点1");
		System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点2");
		System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点3");
		System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点4");
		System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("节点5");
		System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("节点6");
		System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("节点7");
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ersa.Mes.Middleware.P_frmSettingErsa));
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.m_tsbtnSave = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnExit = new System.Windows.Forms.ToolStripButton();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.m_tvwMenu = new System.Windows.Forms.TreeView();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(38, 38);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.m_tsbtnSave, this.m_tsbtnExit });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(1052, 62);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.m_tsbtnSave.Image = Ersa.Mes.Middleware.Properties.Resources.PNG_Save_54x54;
		this.m_tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.m_tsbtnSave.Name = "m_tsbtnSave";
		this.m_tsbtnSave.Size = new System.Drawing.Size(42, 59);
		this.m_tsbtnSave.Text = "Save";
		this.m_tsbtnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
		this.m_tsbtnSave.Click += new System.EventHandler(m_tsbtnSave_Click);
		this.m_tsbtnExit.Image = Ersa.Mes.Middleware.Properties.Resources.png_Leave_54x54;
		this.m_tsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.m_tsbtnExit.Name = "m_tsbtnExit";
		this.m_tsbtnExit.Size = new System.Drawing.Size(42, 59);
		this.m_tsbtnExit.Text = "Exit";
		this.m_tsbtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
		this.m_tsbtnExit.Click += new System.EventHandler(tsbtnExit_Click);
		this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.statusStrip1.Location = new System.Drawing.Point(0, 685);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
		this.statusStrip1.Size = new System.Drawing.Size(1052, 22);
		this.statusStrip1.TabIndex = 28;
		this.statusStrip1.Text = "statusStrip1";
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 62);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add(this.m_tvwMenu);
		this.splitContainer1.Panel2.Font = new System.Drawing.Font("微软雅黑", 9f);
		this.splitContainer1.Size = new System.Drawing.Size(1052, 623);
		this.splitContainer1.SplitterDistance = 192;
		this.splitContainer1.TabIndex = 29;
		this.m_tvwMenu.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_tvwMenu.Font = new System.Drawing.Font("微软雅黑", 9f);
		this.m_tvwMenu.FullRowSelect = true;
		this.m_tvwMenu.ItemHeight = 28;
		this.m_tvwMenu.Location = new System.Drawing.Point(0, 0);
		this.m_tvwMenu.Name = "m_tvwMenu";
		treeNode1.ImageIndex = -2;
		treeNode1.Name = "节点0";
		treeNode1.Text = "节点0";
		treeNode2.ImageIndex = -2;
		treeNode2.Name = "节点1";
		treeNode2.Text = "节点1";
		treeNode3.ImageIndex = -2;
		treeNode3.Name = "节点2";
		treeNode3.Text = "节点2";
		treeNode4.ImageIndex = -2;
		treeNode4.Name = "节点3";
		treeNode4.Text = "节点3";
		treeNode5.ImageIndex = -2;
		treeNode5.Name = "节点4";
		treeNode5.Text = "节点4";
		treeNode6.ImageIndex = -2;
		treeNode6.Name = "节点5";
		treeNode6.Text = "节点5";
		treeNode7.ImageIndex = -2;
		treeNode7.Name = "节点6";
		treeNode7.Text = "节点6";
		treeNode8.ImageIndex = -2;
		treeNode8.Name = "节点7";
		treeNode8.Text = "节点7";
		this.m_tvwMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[8] { treeNode1, treeNode2, treeNode3, treeNode4, treeNode5, treeNode6, treeNode7, treeNode8 });
		this.m_tvwMenu.ShowLines = false;
		this.m_tvwMenu.Size = new System.Drawing.Size(192, 623);
		this.m_tvwMenu.TabIndex = 0;
		this.m_tvwMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(m_tvwMenu_AfterSelect);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1052, 707);
		base.Controls.Add(this.splitContainer1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.toolStrip1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
		base.Name = "P_frmSettingErsa";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Ersa Setting";
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.splitContainer1.Panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
