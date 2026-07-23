using System;
using System.Collections.Generic;
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

	public static volatile bool m_blnActiveSelectProgram = false;

	public static string m_strLibrary = string.Empty;

	public static string m_strProgram = string.Empty;

	public static string m_strCurrentDeviceProgram = string.Empty;

	public static bool m_blnActivePopupDialog = false;

	public static string m_strAlarmMessage = "";

	public static bool m_result = false;

	public static string m_resultReaseinfeed = "null";

	public static bool m_outCreateProtocol = false;

	public static bool m_byPass = false;
	public static XPT_Config m_Config { get; set; }

	public static List<Struct_ProcessParameter> m_lstErsaData { get; set; } = new List<Struct_ProcessParameter>();

    // 静态事件：用于跨组件发送消息到界面
    public static event Action<Enum_LogType, string> Evt_ShowMessage;

    // 触发消息显示
    public static void ShowMessage(Enum_LogType logType, string message)
    {
        Evt_ShowMessage?.Invoke(logType, message);
    }

}
