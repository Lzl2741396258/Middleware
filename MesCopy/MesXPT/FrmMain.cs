using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware;
using MesXPT.Factory;
using MesXPT.Model;

namespace MesXPT;

public class FrmMain : P_frmMain
{
	private IContainer components = null;

	private XPT_Config m_Config { get; set; }

	public FrmMain(XPT_Config i_Config, Inf_Logger i_Logger)
		: base(i_Config, i_Logger)
	{
		InitializeComponent();
		m_Config = i_Config;
		base.Size = new Size
		{
			Width = 1250,
			Height = 800
		};
		base.StartPosition = FormStartPosition.CenterScreen;
		Text = $"ErsaMesMiddleware_DingYi v{Assembly.GetExecutingAssembly().GetName().Version}";
		Sub_ControlTsbtn(i_Config.m_clsBasicSettings.m_i32MainFormLevel);
		i_Logger.Debug(" ersa frmMain softName" + i_Config.m_clsBasicSettings.m_strSoftName);

		// 主窗体首次显示后打开共享串口（不阻塞启动，失败仅提示一次）
		this.Shown += FrmMain_Shown;
    }

	/// <summary>
	/// 启动时按 m_Config 打开共享串口；失败弹窗一次（不阻塞后续流程，指示灯保持灰色）。
	/// </summary>
	private void FrmMain_Shown(object sender, EventArgs e)
	{
		// 只在第一次 Shown 时执行，避免最小化/恢复时重复触发
		this.Shown -= FrmMain_Shown;

		try
		{
			if (XPT_Data.OpenSharedSerialPort(m_Config, base.m_edcLogger, out string err))
			{
				m_edcLogger.Info($"Startup: serial port opened {XPT_Data.SharedSerialPort.PortName} @ {XPT_Data.SharedSerialPort.BaudRate}");
			}
			else
			{
				m_edcLogger.Warn("Startup: open serial port failed: " + err);
				MessageBox.Show(
					$"启动时打开串口失败：{err}\r\n\r\n可能原因：COM 口不存在、被其他程序占用、或参数错误。\r\n\r\n程序将继续运行，串口指示灯保持灰色。可在 \"Custom\" → \"串口配置\" 中修改后重试。",
					"串口未连接",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}
		}
		catch (Exception ex)
		{
			m_edcLogger.Error("Startup OpenSerialPort Unexpected Error: " + ex.Message, ex, "FrmMain_Shown", 0);
		}
	}

	// Overwrite Initialize (Fac & show)
	protected override void Sub_InitializeMesTask()
	{
		m_edcTaskContainer = new XPT_MesTaskFactory(m_Config, base.m_edcLogger);
		m_edcTaskContainer.Evt_ShowMessage += base.Sub_ShowMessage;
		m_edcTaskContainer.Evt_ShowErsasoft += base.Sub_ShowErsasoftCount;
		m_edcTaskContainer.Evt_ShowPLC += base.Sub_ShowPlcCount;
		m_edcTaskContainer.Evt_ShowOEE += base.Sub_ShowOee;
    }

	protected override void m_tsbtnErsa_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(m_Config.m_clsBasicSettings.m_strUser) && string.IsNullOrEmpty(m_Config.m_clsBasicSettings.m_strPassword))
		{
			new FrmSettingErsa(m_Config, base.m_edcLogger).ShowDialog();
			return;
		}
		FrmUserAuthentication form = new FrmUserAuthentication(m_Config, base.m_edcLogger);
		if (form.ShowDialog() == DialogResult.OK && form.m_blnVerify)
		{
			new FrmSettingErsa(m_Config, base.m_edcLogger).ShowDialog();
		}
		base.m_tsbtnErsa_Click(sender, e);
	}

	protected override void m_tsbtnCustom_Click(object sender, EventArgs e)
	{
		new FrmSettingPlatform(m_Config, base.m_edcLogger, base.m_edcAccount).ShowDialog();
		base.m_tsbtnCustom_Click(sender, e);
	}

	protected override void m_tsbtnPLC_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(m_Config.m_clsBasicSettings.m_strUser) && string.IsNullOrEmpty(m_Config.m_clsBasicSettings.m_strPassword))
		{
			new FrmTestPLC(m_Config).ShowDialog();
			return;
		}

		FrmUserAuthentication form = new FrmUserAuthentication(m_Config, base.m_edcLogger);
		if (form.ShowDialog() == DialogResult.OK && form.m_blnVerify)
		{
			new FrmTestPLC(m_Config).ShowDialog();
		}
		base.m_tsbtnPLC_Click(sender, e);
	}

    protected override void m_tsbtnStart_Click(object sender, EventArgs e)
	{
		base.m_tsbtnStart_Click(sender, e);

		m_edcTaskContainer.Func_Test();


    }


    protected override void m_tsbtnRecipe_Click(object sender, EventArgs e)
	{
	}

	protected override void m_tsbtnParameter_Click(object sender, EventArgs e)
	{
		new P_frmDataErsa(XPT_Data.m_lstErsaData).ShowDialog();
	}

	protected override void m_tsbtnPLCData_Click(object sender, EventArgs e)
	{
		new P_frmDataPlc(XPT_Data.m_dicPLCData).ShowDialog();
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
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 101);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.groupBox1.Size = new System.Drawing.Size(1600, 778);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 736);
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

	}
}
