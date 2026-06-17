using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.Model;
using MesXPT.XPT_MesTask;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesFunction;

public class XPT_ReleaseInfeed : ReleaseInfeed
{
    protected new Inf_Logger m_edcLogger;

    private XPT_Config m_Config { get; set; }

    string lastChangeProgram = string.Empty;
    private readonly HttpClient _httpClient;

    public XPT_ReleaseInfeed(XPT_Config i_Config, Inf_Logger i_edcLogger)
        : base(i_Config.m_lstMesFunction, i_edcLogger, i_Config.m_clsBasicSettings.m_enuMachineType, i_Config.m_clsDevice.m_strCameraNoRead)
    {
        m_Config = i_Config;
        m_edcLogger = i_edcLogger;

    }

    public string Fun_change(string i_value)
    {
        string o_strValue = i_value;
        if (o_strValue.Contains(","))
        {
            o_strValue = o_strValue.Replace(',', '.');
        }
        return o_strValue;
    }

    protected override Task Fun_blnConnectMesPlatform()
    {
        base.r_edcResult = new Edc_Result
        {
            m_enuResultCode = Enum_ResponseCode.NichtDefiniert,
            m_strText = string.Empty
        };
        base.r_enuComingRelease = Enum_ComingRelease.NichtDefiniert;

        try
        {
            OnShowMessage(Enum_LogType.Info, "Release Infeed start! ");
            m_edcLogger.Info(" seletiv :" + "Release Infeed start! ");
            m_edcLogger.Info(" 条码 : " + base.m_Request.ma_edcIdentifier[0].m_strValue);
            OnShowMessage(Enum_LogType.Info, "条码： " + base.m_Request.ma_edcIdentifier[0].m_strValue);

            SendProgram(base.m_Request.m_sttSolderingProgram.m_strName);


            if (string.IsNullOrEmpty(base.m_Request.ma_edcIdentifier[0].m_strValue) || XPT_Data.m_byPass)
            {
                
                XPT_Data.m_strAlarmMessage += "条码为空";
                //base.r_edcResult = new Edc_Result
                //{
                //    m_enuResultCode = Enum_ResponseCode.fehler,
                //    m_strText = XPT_Data.m_strAlarmMessage
                //};
                //base.r_enuComingRelease = Enum_ComingRelease.transfer;
                base.r_edcResult = new Edc_Result
                {
                    m_enuResultCode = Enum_ResponseCode.ok
                };
                base.r_enuComingRelease = Enum_ComingRelease.NichtDefiniert;

                XPT_Data.m_result = false;

                XPT_Data.m_resultReaseinfeed = "进板失败";
                m_edcLogger.Info(" seletiv ： " + "Release Infeed failed! 设备不生产 执行bypass模式");
                OnShowMessage(Enum_LogType.Info, "Release Infeed : " );
            }
            else { 
                // 有条码将条码存入到GUI上
                m_Config.m_strCode = base.m_Request.ma_edcIdentifier[0].m_strValue;
                XPT_Data.m_strAlarmMessage = "当前条码为:" + m_Config.m_strCode + "---" + DateTime.Now.ToString();

                XPT_Data.m_blnActiveSelectProgram = false;
                XPT_Data.m_result = false;
                bool isChangeOver = bool.TryParse(m_Config.m_checkChangeOver, out bool checkChangeOver) && checkChangeOver;
                if (isChangeOver)
                {
                    if (m_Config.m_ErsachooseRecipe)
                    {
                        base.r_edcResult = new Edc_Result
                        {
                            m_enuResultCode = Enum_ResponseCode.ok
                        };
                        OnShowMessage(Enum_LogType.Info, "设备执行换型操作换型成功允许进板" );

                    }
                    else
                    {

                        base.r_edcResult = new Edc_Result
                        {
                            m_enuResultCode = Enum_ResponseCode.fehler
                        };
                        OnShowMessage(Enum_LogType.Info, "软件勾选换型选项但未执行换型接口");
                    }
                }
                else
                {
                    OnShowMessage(Enum_LogType.Info, "上个流程执行结果: " + XPT_Data.m_result + m_Config.m_strCode + base.r_enuComingRelease);
                    //
                    var request = new MoveInVerifyRequest
                    {
                        CommandType = "BarcodeCheck",
                        LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        Line = m_Config.m_Account,
                        MachineCode = m_Config.m_RecipeName,
                        Barcode = base.m_Request.ma_edcIdentifier[0].m_strValue,
                        Lane = m_Config.m_Lane,
                        Layer = m_Config.m_Layer
                    };

                    string jsonRequest = JsonConvert.SerializeObject(request);
                    m_edcLogger.Info($"MoveInVerify Request: {jsonRequest}");
                    OnShowMessage(Enum_LogType.Info, $"发送进站请求: {base.m_Request.ma_edcIdentifier[0].m_strValue}");

                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    int iTimtout = 5000;
                    var response = HTTPComm.Post(m_Config.m_ersaDownLineUrl, jsonRequest, iTimtout);
                    //string responseJson = response.Content.ReadAsStringAsync();
                    m_edcLogger.Info($"MoveInVerify Response: {response}");

                    var result = JsonConvert.DeserializeObject<MoveInVerifyResponse>(response);
                    if (result.ValueReturn=="0")
                    {
                        base.r_edcResult = new Edc_Result
                        {
                            m_enuResultCode = Enum_ResponseCode.ok
                        };
                        OnShowMessage(Enum_LogType.Info, "Release Infeed  sucess: " + m_Config.m_strCode + base.r_enuComingRelease);

                    }
                    else
                    {

                        base.r_edcResult = new Edc_Result
                        {
                            m_enuResultCode = Enum_ResponseCode.fehler
                        };

                        //base.r_enuComingRelease = Enum_ComingRelease.transfer;

                        OnShowMessage(Enum_LogType.Info, "Release Infeed  fail: " + m_Config.m_strCode + base.r_enuComingRelease);


                    }
                }
            }

        }
        catch (Exception ex)
        {
            m_edcLogger.Error(MethodBase.GetCurrentMethod().Name + "  Details:" + ex.Message, null, "Fun_blnConnectMesPlatform", 92);
            base.r_edcResult = new Edc_Result
            {
                m_enuResultCode = Enum_ResponseCode.ok
            };
        }
        return Task.CompletedTask;
    }

    private XPT_ersaOnlineResponse GetResponse(string message)
    {
        return JsonConvert.DeserializeObject<XPT_ersaOnlineResponse>(message);
    }

    private void SendProgram(string program)
    {
        try
        {
            string strProgram = XPT_Data.m_strProgram;

            if (program!= strProgram)
            {
                string errorMsg = $"程序换型检测到不一致！当前程序：{program}，程序需更改为：{strProgram}。";
                OnShowMessage(Enum_LogType.Info, errorMsg); 
                return; 
            }

            OnShowMessage(Enum_LogType.Info, "PROGRAM： " + program);


              //  m_edcLogger.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "程序名 ： " + program);
              //  OnShowMessage(Enum_LogType.Info, "PROGRAM： " + program + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "和上一笔程序一样未换型，不生成新文件");
         

            string path = m_Config.m_programFilePath;

            // 获取当前日期
            DateTime currentDate = DateTime.Now;
            string fileName = $"rel0_{currentDate:yyyyMMddHHmmss}.csv"; // 例如：20230928_data.csv

            // 完整文件路径
            string filePath = Path.Combine(path, fileName);

            OnShowMessage(Enum_LogType.Info, "PROGRAM 路径： " + filePath);

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

                string source=m_Config.m_side;

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

                OnShowMessage(Enum_LogType.Info, "处理 程序写入文件成功 ！"  );
            }
        }
        catch (Exception ex) {

            OnShowMessage(Enum_LogType.Info, "处理 程序异常： " + ex.Message);
        }


    }
}
