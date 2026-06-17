using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware;

namespace MesXPT;

public class FrmUserAuthentication : P_frmUserAuthentication
{
	private Inf_Logger m_edcLogger;

	private IContainer components = null;

	public FrmUserAuthentication(Edc_ConfigBase i_Config, Inf_Logger i_edcLogger)
	{
		InitializeComponent();
		m_edcLogger = i_edcLogger;
		base.Size = new Size(362, 227);
		base.StartPosition = FormStartPosition.CenterScreen;
	}

	protected override bool Fun_blnLoginPlatformAccount(out string o_strAccount)
	{
		o_strAccount = string.Empty;
		return false;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(753, 538);
		base.Name = "FrmUserAuthentication";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
