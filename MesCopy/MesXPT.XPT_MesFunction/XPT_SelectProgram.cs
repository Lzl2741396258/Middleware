using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
			XPT_Data.m_strCurrentDeviceProgram = m_Response.m_sttSolderingProgram.m_strName;
            SendProgram(XPT_Data.m_strCurrentDeviceProgram);
        }
		else
		{
			OnShowMessage(Enum_LogType.Error, "Ersasoft->select program error...Details:" + m_Response.m_sttResult.m_strText);
            XPT_Data.m_strCurrentDeviceProgram = string.Empty;


        }
		return Task.FromResult(result: true);
	}

    private void SendProgram(string program)
    {

        try
        {
            //string strProgram = XPT_Data.m_strProgram;

            //if (program != strProgram)
            //{
            //    string errorMsg = $"程序换型检测到不一致！当前程序：{program}，上一次程序：{strProgram}。流程已中断，禁止继续执行。";
            //    MessageBox.Show(errorMsg);
            //    return;
            //}

            MessageBox.Show("PROGRAM： " + program);


            //  m_edcLogger.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "程序名 ： " + program);
            //  OnShowMessage(Enum_LogType.Info, "PROGRAM： " + program + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "和上一笔程序一样未换型，不生成新文件");


            string path = m_Config.m_txtFilePath;

            // 获取当前日期
            DateTime currentDate = DateTime.Now;
            string fileName = $"rel0_{currentDate:yyyyMMddHHmmss}.csv"; // 例如：20230928_data.csv

            // 完整文件路径
            string filePath = Path.Combine(path, fileName);

            MessageBox.Show("PROGRAM 路径： " + filePath);

            // 数据要写入的内容
            string[] headers = new string[]
            {
                    "Program",
                    "TrackNumber",
                    "Side"
            };

            // 变量赋值
            string ChangeProgram = program;

            if (!string.IsNullOrEmpty(ChangeProgram))
            {
                //3608937XXX02A-NIO-AE_TOP ProductNo:3608937XXX02A-NIO  ProductVersion:AE  PCBSurfaceID:TOP
                //string[] prog = ChangeProgram.Split('-');
                //string type = string.Empty;
                //if (prog.Length > 2)
                //{
                //    string source = prog[2];
                //    type = new string(source.Take(1).ToArray());
                //}

                string type = string.Empty;

                string source = string.IsNullOrEmpty(m_Config.m_Layer) ? "BOT" : m_Config.m_Layer;

                if (source.Equals("TOP", StringComparison.OrdinalIgnoreCase))
                {
                    type = "T";
                }
                else
                {
                    type = "B";
                }

                string trackNumber = "0";

                string side = type;

                // 创建或追加到文件
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    // 写入表头
                    writer.WriteLine(string.Join(",", headers));

                    // 写入值
                    writer.WriteLine($"{ChangeProgram},{trackNumber},{side}");
                }

                MessageBox.Show("处理 程序写入文件成功 ！");
            }
        }
        catch (Exception ex)
        {

            MessageBox.Show("处理 程序异常： " + ex.Message);
        }

    }
}
