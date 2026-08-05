using System;
using System.Collections.Generic;
using System.IO.Ports;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.PLC.Model;

namespace MesXPT.Model;

public static class XPT_Data
{
	public static Dictionary<string, List<Edc_PLCElement>> m_dicPLCData = new Dictionary<string, List<Edc_PLCElement>>();

	public static bool m_blnLoadLanguage = false;

	public static Dictionary<string, string> m_dicLanguage = new Dictionary<string, string>();

	public static Enum_OEECode m_enuOeeState = Enum_OEECode.NichtDefiniert;

	public static bool m_blnActiveSelectProgram = false;

	public static string m_strLibrary = string.Empty;

	public static string m_strProgram = string.Empty;

	public static bool m_blnActivePopupDialog = false;

	public static string m_strAlarmMessage = "";

	public static bool m_result = false;

	public static string m_resultReaseinfeed = "null";

	public static bool m_outCreateProtocol = false;

	public static bool m_byPass = false;
	public static XPT_Config m_Config { get; set; }

	public static List<Struct_ProcessParameter> m_lstErsaData { get; set; } = new List<Struct_ProcessParameter>();

	// 全局共享串口对象：FrmMain 启动时打开，FrmSettingPlatform / XPT_ReleaseInfeed 复用同一个
	public static SerialPort SharedSerialPort = new SerialPort();

	/// <summary>
	/// 按 m_Config 当前串口配置打开 SharedSerialPort。已开则直接返回 true。
	/// 失败时通过 out errorMessage 返回具体原因（供 UI 提示用户）。
	/// </summary>
	public static bool OpenSharedSerialPort(XPT_Config config, Inf_Logger logger, out string errorMessage)
	{
		errorMessage = null;
		try
		{
			if (config == null)
			{
				errorMessage = "配置未加载";
				return false;
			}
			if (SharedSerialPort.IsOpen)
			{
				return true;
			}

			SharedSerialPort.PortName = string.IsNullOrEmpty(config.m_strcomPort) ? "COM 1" : config.m_strcomPort;

			if (int.TryParse(config.m_strBaudRate, out int baud))
				SharedSerialPort.BaudRate = baud;
			if (int.TryParse(config.m_strDataBits, out int dataBits))
				SharedSerialPort.DataBits = dataBits;

			if (Enum.TryParse<StopBits>(config.m_strStopBits == "1.5" ? "OnePointFive" : config.m_strStopBits, out StopBits stopBits))
				SharedSerialPort.StopBits = stopBits;

			if (Enum.TryParse<Parity>(config.m_strParity, out Parity parity))
				SharedSerialPort.Parity = parity;

			SharedSerialPort.Open();
			return true;
		}
		catch (Exception ex)
		{
			errorMessage = ex.Message;
			logger?.Error("OpenSharedSerialPort Error: " + ex.Message, ex, "OpenSharedSerialPort", 0);
			return false;
		}
	}

}
