using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using BrightIdeasSoftware;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Bibs;
using Ersa.Mes.FileSystem.Model.RequestResponse;

namespace Ersa.Mes.Middleware;

public class FrmRecipe : Form
{
	private class OlvValue
	{
		public string ColumnName { get; set; }

		public string Value1 { get; set; }

		public string Value2 { get; set; }

		public string Value3 { get; set; }

		public string Value4 { get; set; }

		public string Value5 { get; set; }

		public string Value6 { get; set; }

		public string Value7 { get; set; }

		public string Value8 { get; set; }

		public string Value9 { get; set; }

		public string Value10 { get; set; }

		public string Value11 { get; set; }

		public string Value12 { get; set; }

		public string Value13 { get; set; }

		public string Value31 { get; set; }

		public string Value32 { get; set; }

		public string Value33 { get; set; }

		public string Value34 { get; set; }

		public OlvValue()
		{
		}

		public OlvValue(string i_ColumnName, string _Value1, string _Value2, string _Value3, string _Value4, string _Value5, string _Value6, string _Value7, string _Value8, string _Value9, string _Value10, string _Value11, string _Value12, string _Value13, string _Value31, string _Value32, string _Value33, string _Value34)
		{
			ColumnName = i_ColumnName;
			Value1 = _Value1;
			Value2 = _Value2;
			Value3 = _Value3;
			Value4 = _Value4;
			Value5 = _Value5;
			Value6 = _Value6;
			Value7 = _Value7;
			Value8 = _Value8;
			Value9 = _Value9;
			Value10 = _Value10;
			Value11 = _Value11;
			Value12 = _Value12;
			Value13 = _Value13;
			Value31 = _Value31;
			Value32 = _Value32;
			Value33 = _Value33;
			Value34 = _Value34;
		}
	}

	private XmlDocument m_edcXmlDoc = null;

	private IContainer components = null;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private TextBox m_txtProgramName;

	private Label label1;

	private TextBox m_txtUserName;

	private Label label3;

	private TextBox m_txtBibsName;

	private Label label2;

	private TextBox m_txtFullPath;

	private Label label4;

	private TextBox m_txtUser;

	private Label label5;

	private TextBox m_txtStatus;

	private Label label7;

	private TextBox m_txtVersion;

	private Label label6;

	private TextBox m_txtHistory;

	private GroupBox groupBox3;

	private GroupBox groupBox4;

	private Button m_btnOpen;

	private TextBox textBox3;

	private Label label9;

	private Label label8;

	private TextBox m_txtTestPullpath;

	private FastObjectListView m_olvTemperature;

	private string m_strPathBibs { get; set; }

	private Edc_BibsReflow m_edcRecipe { get; set; }

	private List<Struct_ProcessParameter> m_lstParameter { get; set; }

	public FrmRecipe(string i_strPathBibs, Edc_BibsReflow i_edcRecipe, List<Struct_ProcessParameter> i_lstParameter)
	{
		InitializeComponent();
		m_lstParameter = i_lstParameter;
		m_strPathBibs = i_strPathBibs;
		m_edcRecipe = i_edcRecipe;
	}

	private void FrmRecipe_Load(object sender, EventArgs e)
	{
		if (m_edcRecipe != null)
		{
			if (m_lstParameter.Count == 0)
			{
				MessageBox.Show("No realtime data");
				return;
			}
			string a_strRecipeName = m_lstParameter.Fun_strGetData("stfProg");
			m_txtProgramName.Text = a_strRecipeName;
			string a_strBib = m_lstParameter.Fun_strGetData("stfBib");
			m_txtBibsName.Text = a_strBib;
			string a_strUserName = m_lstParameter.Fun_strGetData("stfUserErsasoft");
			m_txtUserName.Text = a_strUserName;
			Sub_Show(m_strPathBibs, a_strBib, a_strRecipeName, a_strUserName);
			Sub_InitializeOlv();
		}
	}

	private void m_btnOpen_Click(object sender, EventArgs e)
	{
		DirectoryFilesHelper.Sub_OpenFile(Sub_ShowPathBibs);
	}

	public void Sub_ShowPathBibs(string i_strPath)
	{
		FileInfo file = new FileInfo(i_strPath);
		if (file.Exists)
		{
			m_txtTestPullpath.Text = i_strPath;
			string a_strPath = i_strPath.Replace(Path.Combine(file.Directory.Name, file.Name), "");
			Sub_Show(a_strPath, file.Directory.Name, file.Name, "-");
		}
	}

	private void Sub_Show(string i_strPath, string i_strBib, string i_strRecipeName, string i_strUserName)
	{
		m_txtProgramName.Text = i_strRecipeName;
		m_txtBibsName.Text = i_strBib;
		m_txtUserName.Text = i_strUserName;
		textBox3.Text = i_strPath.Replace(i_strBib + "\\", "");
		string a_strFullname = Path.Combine(i_strPath, i_strBib, i_strRecipeName);
		if (File.Exists(a_strFullname))
		{
			m_txtFullPath.Text = a_strFullname;
			m_edcXmlDoc = Edc_BibsReflowHelper.Fun_edcXmlRepairEmpty(a_strFullname);
			Edc_BibsReflow recipe = Edc_BibsReflowHelper.Fun_edcGetBibReflow(a_strFullname);
			m_txtUser.Text = recipe.m_edcUser.m_strUserName;
			m_txtVersion.Text = recipe.m_strVersion;
			m_txtStatus.Text = recipe.m_edcFast.FastActive.ToString();
			int a_i32Item = recipe.m_edcHistory.m_i32Items;
			groupBox1.Text = "History " + a_i32Item;
			string a_strHistory = string.Empty;
			for (int i = 0; i < a_i32Item; i++)
			{
				XmlNode node = m_edcXmlDoc.SelectSingleNode($"SOLDER_PRG/History/Item_{i}");
				a_strHistory = a_strHistory + node.InnerText + "\r\n";
			}
			m_txtHistory.Text = a_strHistory;
		}
		else
		{
			m_txtFullPath.Text = "Recipe dose not exist...";
		}
	}

	private void Sub_InitializeOlv()
	{
		Edc_BibsReflow recipe = m_edcRecipe;
		if (recipe == null)
		{
			return;
		}
		List<Struct_ProcessParameter> parameter = m_lstParameter;
		if (parameter.Count != 0)
		{
			OLVColumn ch0 = new OLVColumn("", "ColumnName");
			ch0.Width = 130;
			m_olvTemperature.Columns.Add(ch0);
			for (int i = 0; i < 13; i++)
			{
				OLVColumn ch1 = new OLVColumn($"Zone{i + 1}", $"Value{i + 1}");
				ch1.Width = 50;
				m_olvTemperature.Columns.Add(ch1);
			}
			List<OlvValue> list = new List<OlvValue>();
			list.Add(new OlvValue("Set Value[℃]", recipe.m_edcHeaters.m_dblTemperture0.ToString(), recipe.m_edcHeaters.m_dblTemperture1.ToString(), recipe.m_edcHeaters.m_dblTemperture2.ToString(), recipe.m_edcHeaters.m_dblTemperture3.ToString(), recipe.m_edcHeaters.m_dblTemperture4.ToString(), recipe.m_edcHeaters.m_dblTemperture5.ToString(), recipe.m_edcHeaters.m_dblTemperture6.ToString(), recipe.m_edcHeaters.m_dblTemperture7.ToString(), recipe.m_edcHeaters.m_dblTemperture8.ToString(), recipe.m_edcHeaters.m_dblTemperture9.ToString(), recipe.m_edcHeaters.m_dblTemperture10.ToString(), recipe.m_edcHeaters.m_dblTemperture11.ToString(), recipe.m_edcHeaters.m_dblTemperture12.ToString(), recipe.m_edcHeaters.m_dblTemperture30.ToString(), recipe.m_edcHeaters.m_dblTemperture31.ToString(), recipe.m_edcHeaters.m_dblTemperture32.ToString(), recipe.m_edcHeaters.m_dblTemperture33.ToString()));
			double[] a_dblDefault = new double[50];
			for (int j = 0; j < 50; j++)
			{
				double.TryParse(parameter.Fun_strGetData($"intActualValueTemp[{j}]"), out a_dblDefault[j]);
			}
			list.Add(new OlvValue("Actual Value[℃]", (a_dblDefault[0] / 10.0).ToString("f2"), (a_dblDefault[1] / 10.0).ToString("f2"), (a_dblDefault[2] / 10.0).ToString("f2"), (a_dblDefault[3] / 10.0).ToString("f2"), (a_dblDefault[4] / 10.0).ToString("f2"), (a_dblDefault[5] / 10.0).ToString("f2"), (a_dblDefault[6] / 10.0).ToString("f2"), (a_dblDefault[7] / 10.0).ToString("f2"), (a_dblDefault[8] / 10.0).ToString("f2"), (a_dblDefault[9] / 10.0).ToString("f2"), (a_dblDefault[10] / 10.0).ToString("f2"), (a_dblDefault[11] / 10.0).ToString("f2"), (a_dblDefault[12] / 10.0).ToString("f2"), (a_dblDefault[30] / 10.0).ToString("f2"), (a_dblDefault[31] / 10.0).ToString("f2"), (a_dblDefault[32] / 10.0).ToString("f2"), (a_dblDefault[33] / 10.0).ToString("f2")));
			list.Add(new OlvValue("Tolerence+", recipe.m_edcHeaters.m_i32PositiveTolerance0.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance1.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance2.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance3.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance4.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance5.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance6.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance7.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance8.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance9.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance10.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance11.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance12.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance30.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance31.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance32.ToString(), recipe.m_edcHeaters.m_i32PositiveTolerance33.ToString()));
			list.Add(new OlvValue("Tolerence_", recipe.m_edcHeaters.m_i32NegativeTolerance0.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance1.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance2.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance3.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance4.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance5.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance6.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance7.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance8.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance9.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance10.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance11.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance12.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance30.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance31.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance32.ToString(), recipe.m_edcHeaters.m_i32NegativeTolerance33.ToString()));
			m_olvTemperature.SetObjects(list);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ersa.Mes.Middleware.FrmRecipe));
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.m_txtStatus = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.m_txtVersion = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.m_txtUser = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.m_txtFullPath = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.m_txtUserName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_txtBibsName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.m_txtProgramName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.m_txtHistory = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.m_olvTemperature = new BrightIdeasSoftware.FastObjectListView();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.m_btnOpen = new System.Windows.Forms.Button();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.m_txtTestPullpath = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvTemperature).BeginInit();
		this.groupBox4.SuspendLayout();
		base.SuspendLayout();
		this.groupBox2.Controls.Add(this.m_txtStatus);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.m_txtVersion);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.m_txtUser);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.m_txtFullPath);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.m_txtUserName);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.m_txtBibsName);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.m_txtProgramName);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox2.Size = new System.Drawing.Size(1361, 152);
		this.groupBox2.TabIndex = 2;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "BasicData";
		this.m_txtStatus.Enabled = false;
		this.m_txtStatus.Location = new System.Drawing.Point(892, 102);
		this.m_txtStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtStatus.Name = "m_txtStatus";
		this.m_txtStatus.Size = new System.Drawing.Size(263, 25);
		this.m_txtStatus.TabIndex = 14;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(832, 107);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(55, 15);
		this.label7.TabIndex = 13;
		this.label7.Text = "Status";
		this.m_txtVersion.Enabled = false;
		this.m_txtVersion.Location = new System.Drawing.Point(508, 102);
		this.m_txtVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtVersion.Name = "m_txtVersion";
		this.m_txtVersion.Size = new System.Drawing.Size(263, 25);
		this.m_txtVersion.TabIndex = 12;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(440, 107);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 15);
		this.label6.TabIndex = 11;
		this.label6.Text = "Version";
		this.m_txtUser.Enabled = false;
		this.m_txtUser.Location = new System.Drawing.Point(148, 102);
		this.m_txtUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtUser.Name = "m_txtUser";
		this.m_txtUser.Size = new System.Drawing.Size(263, 25);
		this.m_txtUser.TabIndex = 10;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(104, 107);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(39, 15);
		this.label5.TabIndex = 9;
		this.label5.Text = "User";
		this.m_txtFullPath.Enabled = false;
		this.m_txtFullPath.Location = new System.Drawing.Point(148, 70);
		this.m_txtFullPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtFullPath.Name = "m_txtFullPath";
		this.m_txtFullPath.Size = new System.Drawing.Size(1007, 25);
		this.m_txtFullPath.TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(72, 75);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 7;
		this.label4.Text = "FullPath";
		this.m_txtUserName.Enabled = false;
		this.m_txtUserName.Location = new System.Drawing.Point(892, 38);
		this.m_txtUserName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtUserName.Name = "m_txtUserName";
		this.m_txtUserName.Size = new System.Drawing.Size(263, 25);
		this.m_txtUserName.TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(792, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(95, 15);
		this.label3.TabIndex = 4;
		this.label3.Text = "CurrentUser";
		this.m_txtBibsName.Enabled = false;
		this.m_txtBibsName.Location = new System.Drawing.Point(508, 38);
		this.m_txtBibsName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtBibsName.Name = "m_txtBibsName";
		this.m_txtBibsName.Size = new System.Drawing.Size(263, 25);
		this.m_txtBibsName.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(432, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "BibsName";
		this.m_txtProgramName.Enabled = false;
		this.m_txtProgramName.Location = new System.Drawing.Point(148, 38);
		this.m_txtProgramName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtProgramName.Name = "m_txtProgramName";
		this.m_txtProgramName.Size = new System.Drawing.Size(263, 25);
		this.m_txtProgramName.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(48, 43);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(95, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "ProgramName";
		this.groupBox1.Controls.Add(this.m_txtHistory);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(4, 155);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.Size = new System.Drawing.Size(1361, 216);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "History";
		this.m_txtHistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_txtHistory.Enabled = false;
		this.m_txtHistory.Location = new System.Drawing.Point(3, 20);
		this.m_txtHistory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.m_txtHistory.Multiline = true;
		this.m_txtHistory.Name = "m_txtHistory";
		this.m_txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.m_txtHistory.Size = new System.Drawing.Size(1355, 194);
		this.m_txtHistory.TabIndex = 0;
		this.groupBox3.Controls.Add(this.m_olvTemperature);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox3.Location = new System.Drawing.Point(4, 371);
		this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox3.Size = new System.Drawing.Size(1361, 241);
		this.groupBox3.TabIndex = 4;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Temperature";
		this.m_olvTemperature.CellEditUseWholeCell = false;
		this.m_olvTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvTemperature.HideSelection = false;
		this.m_olvTemperature.Location = new System.Drawing.Point(3, 20);
		this.m_olvTemperature.Name = "m_olvTemperature";
		this.m_olvTemperature.ShowGroups = false;
		this.m_olvTemperature.Size = new System.Drawing.Size(1355, 219);
		this.m_olvTemperature.TabIndex = 0;
		this.m_olvTemperature.UseCompatibleStateImageBehavior = false;
		this.m_olvTemperature.View = System.Windows.Forms.View.Details;
		this.m_olvTemperature.VirtualMode = true;
		this.groupBox4.Controls.Add(this.m_btnOpen);
		this.groupBox4.Controls.Add(this.textBox3);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Controls.Add(this.m_txtTestPullpath);
		this.groupBox4.Controls.Add(this.label8);
		this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox4.Location = new System.Drawing.Point(4, 612);
		this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox4.Size = new System.Drawing.Size(1361, 152);
		this.groupBox4.TabIndex = 5;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Test";
		this.m_btnOpen.Location = new System.Drawing.Point(1169, 44);
		this.m_btnOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_btnOpen.Name = "m_btnOpen";
		this.m_btnOpen.Size = new System.Drawing.Size(84, 30);
		this.m_btnOpen.TabIndex = 13;
		this.m_btnOpen.Text = "Open";
		this.m_btnOpen.UseVisualStyleBackColor = true;
		this.m_btnOpen.Click += new System.EventHandler(m_btnOpen_Click);
		this.textBox3.Enabled = false;
		this.textBox3.Location = new System.Drawing.Point(148, 91);
		this.textBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(1007, 25);
		this.textBox3.TabIndex = 12;
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(103, 95);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(39, 15);
		this.label9.TabIndex = 11;
		this.label9.Text = "Bibs";
		this.m_txtTestPullpath.Enabled = false;
		this.m_txtTestPullpath.Location = new System.Drawing.Point(148, 47);
		this.m_txtTestPullpath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_txtTestPullpath.Name = "m_txtTestPullpath";
		this.m_txtTestPullpath.Size = new System.Drawing.Size(1007, 25);
		this.m_txtTestPullpath.TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(72, 52);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(71, 15);
		this.label8.TabIndex = 9;
		this.label8.Text = "FullPath";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1369, 773);
		base.Controls.Add(this.groupBox4);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.groupBox2);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		base.Name = "FrmRecipe";
		base.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Recipe";
		base.Load += new System.EventHandler(FrmRecipe_Load);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.m_olvTemperature).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		base.ResumeLayout(false);
	}
}
