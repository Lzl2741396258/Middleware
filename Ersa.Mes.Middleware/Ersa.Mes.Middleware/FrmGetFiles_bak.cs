using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Config.Trace05;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;

namespace Ersa.Mes.Middleware;

public class FrmGetFiles_bak : Form
{
	public static List<string[]> m_scProtocol;

	private IContainer components = null;

	private RichTextBox m_rtbInfo;

	private Button m_btnTrace05;

	private Button m_btnProtocol;

	private Button btn_ztxt;

	private Button m_btnTrend;

	private Panel panel1;

	private Inf_Logger m_edcLogger { get; }

	public FrmGetFiles_bak()
	{
		InitializeComponent();
	}

	private void FrmGetFiles_Load(object sender, EventArgs e)
	{
		m_scProtocol = new List<string[]>();
	}

	private void m_btnTrace05_Click(object sender, EventArgs e)
	{
		GetTrace05();
	}

	private void m_btnProtocol_Click(object sender, EventArgs e)
	{
		GetProtocol();
	}

	private void btn_ztxt_Click(object sender, EventArgs e)
	{
		GetZtxt();
	}

	private void GetProtocol()
	{
		string a_strDirectoryProtocol = string.Empty;
		try
		{
			List<FileInfo> list = new DirectoryInfo(a_strDirectoryProtocol).GetFiles("*", SearchOption.AllDirectories).ToList();
			foreach (FileInfo a_FileInfo in list)
			{
				using (new FileStream(a_FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					using StreamReader sr = new StreamReader(a_FileInfo.FullName, Encoding.Default);
					int a_i32LineIndex = 1;
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						switch (a_i32LineIndex)
						{
						case 1:
							a_i32LineIndex++;
							break;
						case 2:
						{
							string[] a_strCell = line.Split(';');
							m_scProtocol.Add(a_strCell);
							break;
						}
						}
					}
				}
				string a_strNewPath = a_strDirectoryProtocol + "_bak\\";
				string a_strNewFullName = a_strNewPath + a_FileInfo.Name;
				if (!Directory.Exists(a_strNewPath))
				{
					Directory.CreateDirectory(a_strNewPath);
				}
				if (File.Exists(a_strNewFullName))
				{
					FileInfo fileinfo = new FileInfo(a_strNewFullName);
					fileinfo.Delete();
				}
				File.Move(a_FileInfo.FullName, a_strNewPath + a_FileInfo.Name);
			}
		}
		catch (Exception ex)
		{
			m_edcLogger.Error(MethodBase.GetCurrentMethod().Name + "  " + ex.Message, null, "GetProtocol", 119);
			ShowInfo(ex.Message);
		}
	}

	private void GetTrace05()
	{
		try
		{
			string a_strPathAndFilename = string.Empty;
			m_edcLogger.Info(MethodBase.GetCurrentMethod().Name + "  获得ConfigTrace05全路径+文件名  " + a_strPathAndFilename);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Struct_ConfigTrace05));
			Struct_ConfigTrace05 a_sttConfigTrace5 = a_strPathAndFilename.Fun_DeserializeContent<Struct_ConfigTrace05>();
			ShowInfo("Get Trace05 success... StationsName:" + a_sttConfigTrace5.m_strStationsName + "  LineName:" + a_sttConfigTrace5.m_strLineName);
		}
		catch (Exception ex)
		{
			m_edcLogger.Error(MethodBase.GetCurrentMethod().Name + "  " + ex.Message, null, "GetTrace05", 141);
			ShowInfo(ex.Message);
		}
	}

	private void GetZtxt()
	{
		string a_strPathAndFilename = string.Empty;
		m_edcLogger.Info(MethodBase.GetCurrentMethod().Name + "  获得z.txt全路径+文件名  " + a_strPathAndFilename);
		try
		{
			string[] a_strLine = File.ReadAllLines(a_strPathAndFilename);
			ShowInfo(GetAttributeAndValue(a_strLine).m_dblWaitingTimeManual.ToString());
		}
		catch (Exception ex)
		{
			m_edcLogger.Error(MethodBase.GetCurrentMethod().Name + "  " + ex.Message, null, "GetZtxt", 163);
			ShowInfo(ex.Message);
		}
	}

	private Struct_Ztxt GetAttributeAndValue(string[] i_strLines)
	{
		Struct_Ztxt a_sttZtxt = default(Struct_Ztxt);
		a_sttZtxt.SUB_Init();
		a_sttZtxt.TimedataFrom = i_strLines[1].Split('=')[1];
		a_sttZtxt.m_dblWaitingTimeManual = double.Parse(i_strLines[3].Split('=')[1].ToString());
		a_sttZtxt.m_dblWorkingTimeProduction = double.Parse(i_strLines[4].Split('=')[1].ToString());
		a_sttZtxt.m_dblWaitingTimeAUTO = double.Parse(i_strLines[5].Split('=')[1].ToString());
		a_sttZtxt.m_dblWorkingTimeTotal = double.Parse(i_strLines[6].Split('=')[1].ToString());
		a_sttZtxt.m_dblWaitingTimeCongestion = double.Parse(i_strLines[7].Split('=')[1].ToString());
		a_sttZtxt.ConsumedPower = i_strLines[9].Split('=')[1];
		a_sttZtxt.ProducedPCBs = int.Parse(i_strLines[11].Split('=')[1]);
		a_sttZtxt.DefectivePCBs = int.Parse(i_strLines[12].Split('=')[1]);
		a_sttZtxt.TotalPCBs = int.Parse(i_strLines[13].Split('=')[1]);
		return a_sttZtxt;
	}

	private void ShowInfo(string i_strInfo)
	{
		Invoke((Action)delegate
		{
			RichTextBox rtbInfo = m_rtbInfo;
			rtbInfo.Text = rtbInfo.Text + i_strInfo + "\r\n";
		});
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
		this.m_rtbInfo = new System.Windows.Forms.RichTextBox();
		this.m_btnTrace05 = new System.Windows.Forms.Button();
		this.m_btnProtocol = new System.Windows.Forms.Button();
		this.btn_ztxt = new System.Windows.Forms.Button();
		this.m_btnTrend = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.m_rtbInfo.Dock = System.Windows.Forms.DockStyle.Left;
		this.m_rtbInfo.Location = new System.Drawing.Point(0, 0);
		this.m_rtbInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.m_rtbInfo.Name = "m_rtbInfo";
		this.m_rtbInfo.Size = new System.Drawing.Size(815, 842);
		this.m_rtbInfo.TabIndex = 0;
		this.m_rtbInfo.Text = "";
		this.m_btnTrace05.Location = new System.Drawing.Point(4, 10);
		this.m_btnTrace05.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.m_btnTrace05.Name = "m_btnTrace05";
		this.m_btnTrace05.Size = new System.Drawing.Size(92, 54);
		this.m_btnTrace05.TabIndex = 1;
		this.m_btnTrace05.Text = "Trace05";
		this.m_btnTrace05.UseVisualStyleBackColor = true;
		this.m_btnTrace05.Click += new System.EventHandler(m_btnTrace05_Click);
		this.m_btnProtocol.Location = new System.Drawing.Point(4, 69);
		this.m_btnProtocol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.m_btnProtocol.Name = "m_btnProtocol";
		this.m_btnProtocol.Size = new System.Drawing.Size(92, 54);
		this.m_btnProtocol.TabIndex = 2;
		this.m_btnProtocol.Text = "Protocol";
		this.m_btnProtocol.UseVisualStyleBackColor = true;
		this.m_btnProtocol.Click += new System.EventHandler(m_btnProtocol_Click);
		this.btn_ztxt.Location = new System.Drawing.Point(4, 128);
		this.btn_ztxt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.btn_ztxt.Name = "btn_ztxt";
		this.btn_ztxt.Size = new System.Drawing.Size(92, 54);
		this.btn_ztxt.TabIndex = 3;
		this.btn_ztxt.Text = "ztxt";
		this.btn_ztxt.UseVisualStyleBackColor = true;
		this.btn_ztxt.Click += new System.EventHandler(btn_ztxt_Click);
		this.m_btnTrend.Location = new System.Drawing.Point(4, 187);
		this.m_btnTrend.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.m_btnTrend.Name = "m_btnTrend";
		this.m_btnTrend.Size = new System.Drawing.Size(92, 54);
		this.m_btnTrend.TabIndex = 4;
		this.m_btnTrend.Text = "Trend";
		this.m_btnTrend.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.m_btnTrace05);
		this.panel1.Controls.Add(this.m_btnTrend);
		this.panel1.Controls.Add(this.m_btnProtocol);
		this.panel1.Controls.Add(this.btn_ztxt);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(815, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(101, 842);
		this.panel1.TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(916, 842);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.m_rtbInfo);
		base.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
		base.Name = "FrmGetFiles";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "DocumentCollection";
		base.Load += new System.EventHandler(FrmGetFiles_Load);
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
