using Ersa.Mes.Logging;
using Ersa.Mes.Middleware;
using Helpers;
using MesXPT.Factory;
using MesXPT.Model;
using MesXPT.XPT_MesService;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace MesXPT;

public class FrmMain : P_frmMain
{
	private IContainer components = null;

	private XPT_Config m_Config { get; set; }
    private Edc_ChangeOverServiceHost m_ChangeOverServiceHost { get; set; }

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
		Text = $"{i_Config.m_clsBasicSettings.m_strSoftName} v{Assembly.GetExecutingAssembly().GetName().Version}";
		Sub_ControlTsbtn(i_Config.m_clsBasicSettings.m_i32MainFormLevel);
		i_Logger.Debug(" ersa frmMain softName" + i_Config.m_clsBasicSettings.m_strSoftName);

        // 启动 WCF 换型服务
        //StartChangeOverService(i_Config, i_Logger);
        Load += FrmMain_Load;
        WebServer.Instance.StartWebServer();

    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
    }

    private void StartChangeOverService(XPT_Config config, Inf_Logger logger)
    {
        try
        {
            bool isEnabled = bool.TryParse(config.m_strChangeOverServiceEnabled, out bool enabled) && enabled;
            if (!isEnabled)
            {
                logger.Info("[WCF] ChangeOver Service is disabled");
                return;
            }

            m_ChangeOverServiceHost = new Edc_ChangeOverServiceHost(config, logger);
			string ip = string.IsNullOrEmpty(config.m_strChangeOverServiceIP) ? "127.0.0.1" : config.m_strChangeOverServiceIP;
            // 订阅消息事件，转发到界面显示
            m_ChangeOverServiceHost.OnShowMessage += base.Sub_ShowMessage;
            int port = config.m_i32ChangeOverServicePort > 0 ? config.m_i32ChangeOverServicePort : 8080;

            bool started = m_ChangeOverServiceHost.Start(ip, port);
            if (started)
            {
                logger.Info("[WCF] ChangeOver Service started successfully");
            }
            else
            {
                logger.Error("[WCF] Failed to start ChangeOver Service");
            }
        }
        catch (Exception ex)
        {
            logger.Error($"[WCF] Error starting ChangeOver Service: {ex.Message}", ex, "StartChangeOverService", 0);
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
