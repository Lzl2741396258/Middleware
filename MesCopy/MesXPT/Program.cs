using Autofac;
using AutoUpdaterDotNET;
using Ersa.Mes.Common.Helper;
using Ersa.Mes.FileSystem.Model;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Global;
using Excetec.LicenseVaild;
using Helpers;
using MesXPT.Model;
using System;
using System.Threading;
using System.Windows.Forms;

namespace MesXPT;

internal static class Program
{
	[STAThread]
	private static void Main()
	{

        //var (isValid, message) = ExcetecLicenseManager.IsLicenseValid();
        //if (!isValid)
        //{
        //    string uuid = ExcetecLicenseManager.GetDeviceId();
        //    MessageBox.Show($"{message}\n…Ë±∏ID: {uuid}");
        //    Clipboard.Clear();
        //    Clipboard.SetText(uuid);
        //    return;
        //}
        Application.ThreadException += Form_UIThreadException;
		Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		string a_strPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\ConfigXPT.xml";
		XPT_Config m_Config = new XPT_Config();
		m_Config = Edc_OperationConfig.Fun_ReadConfig<XPT_Config>(a_strPath);
		if (m_Config == null)
		{
			MessageBox.Show("Load Config File Error, Please Check...");
			return;
		}
		m_Config.m_edcFilesPath.m_strPathConfig = a_strPath;
		XPT_Data.m_Config = m_Config;
		Edc_Logger m_edcLogger = new Edc_Logger((Enum_LogLevels)m_Config.m_clsBasicSettings.m_strLogLevel, i_blnInitialize: true);
		if (!string.IsNullOrEmpty(m_Config.m_clsBasicSettings.m_strAutoUpdateUrl))
		{
			AutoUpdater.Synchronous = true;
			AutoUpdater.ShowSkipButton = false;
			AutoUpdater.ShowRemindLaterButton = false;
			AutoUpdater.Start(m_Config.m_clsBasicSettings.m_strAutoUpdateUrl);
		}
		AutoStart edc_AutoStart = new AutoStart();
		edc_AutoStart.Sub_SetAutoStart(m_Config.m_clsBasicSettings.m_blnWindowsStartup);

		ContainerManager.Instance.Register(a => { 
			a.RegisterInstance(m_Config).AsSelf().SingleInstance();
			a.RegisterInstance(m_edcLogger).As<Inf_Logger>().SingleInstance();

        });
        ContainerManager.Instance.Build();

        Application.ApplicationExit += Application_ApplicationExit;
		if (Edc_Global.Fun_blnAppRunning(Application.ProductName))
		{
			MessageBox.Show("System is already running...");
		}
		else
		{
			Application.Run(new FrmMain(m_Config, m_edcLogger));
		}
	}

    private static void Application_ApplicationExit(object sender, EventArgs e)
    {
        WebServer.Instance.StopWebServer();
    }

    private static void Form_UIThreadException(object sender, ThreadExceptionEventArgs t)
	{
		MessageBox.Show("Sorry, your operation has not been completed, please restart the software and try again...");
	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		MessageBox.Show("Sorry, your operation has not been completed, please restart the software and try again...");
	}
}
