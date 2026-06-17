using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.FileSystem.Model.RequestResponse.Request;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.CommunicationService;
using MesXPT.Model;

namespace MesXPT.XPT_MesFunction;

public class XPT_SelectProgram : SelectProgram
{
	private XPT_Config m_Config { get; set; }

	private Edc_XPTCommunicationService m_edcService { get; set; }

	private string m_strCurrentProgram { get; set; }

	private string m_strChangeoverProgram { get; set; }

	public XPT_SelectProgram(XPT_Config i_Config, Inf_Logger i_edcLogger)
		: base(i_Config.m_lstMesFunction, i_edcLogger)
	{
		m_Config = i_Config;
		m_edcService = new Edc_XPTCommunicationService(i_Config, i_edcLogger);
		Task.Run((Action)Sub_Start);
	}
    private void ButtonName_Click(object sender, EventArgs e)
    {

    }
    public void Sub_Start()
	{
		while (true)
		{
			Thread.Sleep(5000);
			if (XPT_Data.m_blnActiveSelectProgram)
			{
				XPT_Data.m_blnActiveSelectProgram = false;
				try
				{
                    OnShowMessage(Enum_LogType.Info, "选择程序接口");
                    string a_strLibrary = XPT_Data.m_strLibrary;
					string a_strProram = XPT_Data.m_strProgram;//m_strProgram;
					string a_strRequest = Fun_strGetResponse(a_strLibrary, a_strProram);
					OnSendRequest(a_strRequest);
					OnShowMessage(Enum_LogType.Info,"select program "+ a_strProram);
				}
				catch (Exception ex)
				{
					OnShowMessage(Enum_LogType.Error, "Middleware -> " + MethodBase.GetCurrentMethod().DeclaringType.FullName + " Error...Details:'" + ex.Message + "'");
				}
			}
		}
	}

	public static string Fun_strGetResponse(string i_strBib, string i_strProgramName)
	{
		Request_SelectProgram model = new Request_SelectProgram
		{
			m_sttHeader = new Struct_HeaderWithTrack
			{
				m_dtmTimestamp = DateTime.Now,
				m_strProcessName = "Selektiv Soldering",
				m_strLineName = "19",
				m_strStationName = "34",
				m_strMessageId = Guid.NewGuid().ToString(),
				m_strVersion = "0.1"
			},
			m_sttSolderingProgram = new Struct_SolderingProgram
			{
				m_strLibrary = i_strBib,
				m_strName = i_strProgramName,

                m_enuChangeStatus = Enum_ChangeStatus.Geaendert
            }
		};
        return SerializerHelper.Fun_strSerializerModel<Request_SelectProgram>(model);
	}

	protected override Task Fun_blnConnectMesPlatform()
	{
		base.m_edcLogger.Debug("Begin " + MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name + "...", null, "Fun_blnConnectMesPlatform", 114);
        OnShowMessage(Enum_LogType.Info, "选择程序接口");
        if (m_Response.m_sttResult.m_enuCode == Enum_SolderingProgramErrorCode.Ok)
		{
			OnShowMessage(Enum_LogType.Info, "Ersasoft->Recived select program " + m_Response.m_sttSolderingProgram.m_strLibrary + "\\" + m_Response.m_sttSolderingProgram.m_strName);
		}
		else
		{
			OnShowMessage(Enum_LogType.Error, "Ersasoft->select program error...Details:" + m_Response.m_sttResult.m_strText);
		}
		return Task.FromResult(result: true);
	}
}
