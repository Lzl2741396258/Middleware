using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Ersa.Mes.Database.Model.EF_Ersasoft5;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclDatabase : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private GroupBox m_gbxControl;

	private TextBox m_txtConnectionString;

	private Label label4;

	private Button m_btnGet;

	private Edc_ConfigBase m_Config { get; set; }

	public int m_i32FormID { get; set; }

	public string Pro_FormName => "Database";

	public UclDatabase(Edc_ConfigBase i_Config)
	{
		InitializeComponent();
		m_Config = i_Config;
	}

	private string Fun_strGetConnectString(string i_strServer, string catalog, string user, string pass, bool winAuth = false)
	{
		SqlConnectionStringBuilder a_strSql = new SqlConnectionStringBuilder
		{
			DataSource = i_strServer
		};
		return string.Empty;
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		throw new NotImplementedException();
	}

	private void m_btnTest_Click(object sender, EventArgs e)
	{
		if (m_Config.m_clsBasicSettings.m_strUser.Equals("ERSA", StringComparison.OrdinalIgnoreCase))
		{
			using (Ersasoft5 db = new Ersasoft5())
			{
				m_txtConnectionString.Text = db.Database.Connection.ConnectionString;
				return;
			}
		}
		m_txtConnectionString.Text = "******";
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
		this.m_gbxControl = new System.Windows.Forms.GroupBox();
		this.m_txtConnectionString = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.m_btnGet = new System.Windows.Forms.Button();
		this.m_gbxControl.SuspendLayout();
		base.SuspendLayout();
		this.m_gbxControl.Controls.Add(this.m_btnGet);
		this.m_gbxControl.Controls.Add(this.m_txtConnectionString);
		this.m_gbxControl.Controls.Add(this.label4);
		this.m_gbxControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_gbxControl.Location = new System.Drawing.Point(5, 5);
		this.m_gbxControl.Margin = new System.Windows.Forms.Padding(4);
		this.m_gbxControl.Name = "m_gbxControl";
		this.m_gbxControl.Padding = new System.Windows.Forms.Padding(4);
		this.m_gbxControl.Size = new System.Drawing.Size(832, 130);
		this.m_gbxControl.TabIndex = 1;
		this.m_gbxControl.TabStop = false;
		this.m_gbxControl.Text = "Database";
		this.m_txtConnectionString.Location = new System.Drawing.Point(159, 56);
		this.m_txtConnectionString.Margin = new System.Windows.Forms.Padding(2);
		this.m_txtConnectionString.Name = "m_txtConnectionString";
		this.m_txtConnectionString.Size = new System.Drawing.Size(638, 23);
		this.m_txtConnectionString.TabIndex = 16;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(47, 59);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(107, 17);
		this.label4.TabIndex = 17;
		this.label4.Text = "ConnectionString";
		this.m_btnGet.Location = new System.Drawing.Point(159, 82);
		this.m_btnGet.Margin = new System.Windows.Forms.Padding(1);
		this.m_btnGet.Name = "m_btnGet";
		this.m_btnGet.Size = new System.Drawing.Size(68, 26);
		this.m_btnGet.TabIndex = 62;
		this.m_btnGet.Text = "Get";
		this.m_btnGet.UseVisualStyleBackColor = true;
		this.m_btnGet.Click += new System.EventHandler(m_btnTest_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.m_gbxControl);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "UclDatabase";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(842, 140);
		this.m_gbxControl.ResumeLayout(false);
		this.m_gbxControl.PerformLayout();
		base.ResumeLayout(false);
	}
}
