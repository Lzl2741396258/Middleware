using System;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.Model;

namespace MesXPT.XPT_MesFunction;

public class XPT_PopupDialog : PopupDialog
{
	private XPT_Config m_Config { get; set; }

	public XPT_PopupDialog(XPT_Config i_Config, Inf_Logger i_edcLogger)
		: base(i_edcLogger)
	{
		m_Config = i_Config;
		Task.Run(delegate
		{
			while (true)
			{
				Thread.Sleep(500);
				if (XPT_Data.m_blnActivePopupDialog)
				{
					XPT_Data.m_blnActivePopupDialog = false;
					string strAlarmMessage = XPT_Data.m_strAlarmMessage;
					string i_strMessage = Fun_strGetResponse(m_Config, Enum_PopupType.ExterneMeldung, strAlarmMessage);
					OnSendRequest(i_strMessage);
					OnShowMessage(Enum_LogType.Info, "Send PopupDialog " + strAlarmMessage + " successed...");
					Thread.Sleep(3000);
				}
			}
		});
	}

	public static string Fun_strGetResponse(XPT_Config i_Config, Enum_PopupType i_enuPopupType, string i_strMessage)
	{
		Request_PopupDialog model = new Request_PopupDialog
		{
			m_sttHeader = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strProcessName = "Reflow Soldering",
				m_strLineName = "02",
				m_strStationName = "02",
				m_strMessageId = Guid.NewGuid().ToString(),
				m_strVersion = "0.1"
			},
			m_strTerminalInformation = i_strMessage,
			m_enmShowPopup = i_enuPopupType
		};
		return SerializerHelper.Fun_strSerializerModel<Request_PopupDialog>(model);
	}
}
