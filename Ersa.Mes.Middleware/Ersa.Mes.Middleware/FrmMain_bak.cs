using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Protocol;

namespace Ersa.Mes.Middleware;

public class FrmMain_bak : Form
{
	private string m_FilePath = Directory.GetCurrentDirectory() + "\\test.xml";

	private IContainer components = null;

	private StatusStrip statusStrip1;

	private Panel panel1;

	private PictureBox pictureBox1;

	private Button btnExit;

	private Button btnConfig;

	private Button btnScan;

	private Button btnProtocol;

	private ToolTip toolTip1;

	private Timer timer1;

	public FrmMain_bak()
	{
		InitializeComponent();
	}

	private void FormMain_Load(object sender, EventArgs e)
	{
		Text += Application.ProductVersion;
	}

	private void ButtonClick(object sender, EventArgs e)
	{
		Button a_Button = sender as Button;
		switch (a_Button.Text)
		{
		case "Files":
		{
			Form form = new FrmGetFiles_bak();
			form.ShowDialog();
			break;
		}
		case "Settings":
		case "Exit":
			Close();
			break;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			Edc_ProtocolSelectiveZevi a_ReflowProtocol = new Edc_ProtocolSelectiveZevi();
			string str1 = SerializerHelper.Fun_strSerializerModel<Edc_ProtocolSelectiveZevi>(a_ReflowProtocol);
			MemoryStream a_MemoryStream = new MemoryStream();
			XmlTextWriter a_XmlTextWriter = new XmlTextWriter(a_MemoryStream, Encoding.UTF8);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Edc_ProtocolSelectiveZevi));
			xmlSerializer.Serialize(a_XmlTextWriter, a_ReflowProtocol);
			byte[] utf8EncodedData = a_MemoryStream.ToArray();
			string result2 = Encoding.UTF8.GetString(utf8EncodedData);
			Edc_ProtocolSelectiveZevi result = str1.Fun_DeserializeContent<Edc_ProtocolSelectiveZevi>();
			Edc_ProtocolSelectiveZevi reflow2 = result;
			XmlSerializer a_XmlSerializer = new XmlSerializer(typeof(Edc_ProtocolSelectiveZevi));
			TextReader a_reader = new StringReader(m_FilePath);
		}
		catch (Exception)
		{
			throw;
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ersa.Mes.Middleware.FrmMain_bak));
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btnProtocol = new System.Windows.Forms.Button();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.btnExit = new System.Windows.Forms.Button();
		this.btnConfig = new System.Windows.Forms.Button();
		this.btnScan = new System.Windows.Forms.Button();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.statusStrip1.Location = new System.Drawing.Point(0, 1091);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 15, 0);
		this.statusStrip1.Size = new System.Drawing.Size(1920, 22);
		this.statusStrip1.TabIndex = 24;
		this.statusStrip1.Text = "statusStrip1";
		this.panel1.Controls.Add(this.btnProtocol);
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Controls.Add(this.btnExit);
		this.panel1.Controls.Add(this.btnConfig);
		this.panel1.Controls.Add(this.btnScan);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Padding = new System.Windows.Forms.Padding(22, 24, 22, 24);
		this.panel1.Size = new System.Drawing.Size(224, 1091);
		this.panel1.TabIndex = 25;
		this.btnProtocol.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.btnProtocol.Image = (System.Drawing.Image)resources.GetObject("btnProtocol.Image");
		this.btnProtocol.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnProtocol.Location = new System.Drawing.Point(0, 303);
		this.btnProtocol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnProtocol.Name = "btnProtocol";
		this.btnProtocol.Size = new System.Drawing.Size(224, 100);
		this.btnProtocol.TabIndex = 96;
		this.btnProtocol.Tag = "2";
		this.btnProtocol.Text = "Files";
		this.btnProtocol.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.btnProtocol.UseVisualStyleBackColor = true;
		this.btnProtocol.Click += new System.EventHandler(ButtonClick);
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pictureBox1.Location = new System.Drawing.Point(22, 24);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(180, 136);
		this.pictureBox1.TabIndex = 94;
		this.pictureBox1.TabStop = false;
		this.btnExit.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.btnExit.Image = (System.Drawing.Image)resources.GetObject("btnExit.Image");
		this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnExit.Location = new System.Drawing.Point(0, 504);
		this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnExit.Name = "btnExit";
		this.btnExit.Size = new System.Drawing.Size(224, 100);
		this.btnExit.TabIndex = 93;
		this.btnExit.Tag = "4";
		this.btnExit.Text = "Exit";
		this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.btnExit.UseVisualStyleBackColor = true;
		this.btnExit.Click += new System.EventHandler(ButtonClick);
		this.btnConfig.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.btnConfig.Image = (System.Drawing.Image)resources.GetObject("btnConfig.Image");
		this.btnConfig.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnConfig.Location = new System.Drawing.Point(0, 404);
		this.btnConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnConfig.Name = "btnConfig";
		this.btnConfig.Size = new System.Drawing.Size(224, 100);
		this.btnConfig.TabIndex = 92;
		this.btnConfig.Tag = "3";
		this.btnConfig.Text = "Settings";
		this.btnConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.btnConfig.UseVisualStyleBackColor = true;
		this.btnConfig.Click += new System.EventHandler(ButtonClick);
		this.btnScan.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.btnScan.Image = (System.Drawing.Image)resources.GetObject("btnScan.Image");
		this.btnScan.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnScan.Location = new System.Drawing.Point(0, 201);
		this.btnScan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnScan.Name = "btnScan";
		this.btnScan.Size = new System.Drawing.Size(224, 100);
		this.btnScan.TabIndex = 91;
		this.btnScan.Tag = "1";
		this.btnScan.Text = "MES";
		this.btnScan.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.btnScan.UseVisualStyleBackColor = true;
		this.btnScan.Click += new System.EventHandler(ButtonClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 18f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1920, 1113);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.statusStrip1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.IsMdiContainer = true;
		base.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		base.Name = "FrmMain";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Ersa Manufactuing Execution System v";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(FormMain_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
