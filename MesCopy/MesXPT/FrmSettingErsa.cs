using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware;
using MesXPT.Model;

namespace MesXPT;

public class FrmSettingErsa : P_frmSettingErsa
{
	private IContainer components = null;

	public XPT_Config m_Config { get; }

	public FrmSettingErsa(XPT_Config i_Config, Inf_Logger i_edcLogger)
		: base(i_Config, i_edcLogger)
	{
		m_Config = i_Config;
		m_strConfigPath = i_Config.m_edcFilesPath.m_strPathConfig;
		Sub_ShowPath(i_blnProtocol: false, i_blnZtxt: false, i_blnTrend: false, i_blnBibs: true, i_blnInitialize: true, i_blnData: true);
	}

	public override bool Fun_blnWriteToConfigFile()
	{
		m_Config.m_clsBasicSettings = base.m_edcConfigBase.m_clsBasicSettings;
		m_Config.m_edcFilesPath = base.m_edcConfigBase.m_edcFilesPath;
		m_Config.m_clsDevice = base.m_edcConfigBase.m_clsDevice;
		m_Config.ma_MesTask = base.m_edcConfigBase.ma_MesTask;
		m_Config.m_edcInterfaceAddress = base.m_edcConfigBase.m_edcInterfaceAddress;
		m_Config.m_clsDatabase = base.m_edcConfigBase.m_clsDatabase;
		m_Config.m_clsFTP = base.m_edcConfigBase.m_clsFTP;
		return Edc_OperationConfig.Fun_WriteConfig<XPT_Config>(m_strConfigPath, m_Config);
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(952, 805);
		base.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
		base.Name = "FrmSettingErsa";
		this.Text = "XPTSettings";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
