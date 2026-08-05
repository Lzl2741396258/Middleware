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
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            // SendProgram(base.m_Request.m_sttSolderingProgram.m_strName);

            if (XPT_Data.m_byPass)
            {
                m_edcLogger.Info(" seletiv ： " + $"Release Infeed failed! 执行ByPass模式");
                OnShowMessage(Enum_LogType.Info, $"Release Infeed : 执行ByPass模式");
            }
            else if (string.IsNullOrEmpty(base.m_Request.ma_edcIdentifier[0].m_strValue))
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

                //XPT_Data.m_resultReaseinfeed = "进板失败";
                m_edcLogger.Info(" seletiv ： " + $"Release Infeed failed! {XPT_Data.m_strAlarmMessage}");
                OnShowMessage(Enum_LogType.Info, $"Release Infeed : {XPT_Data.m_strAlarmMessage}" );
            }
            else 
            { 
                // 有条码将条码存入到GUI上
                m_Config.m_strCode = base.m_Request.ma_edcIdentifier[0].m_strValue;
                XPT_Data.m_strAlarmMessage = "当前条码为:" + m_Config.m_strCode + "---" + DateTime.Now.ToString();

                XPT_Data.m_blnActiveSelectProgram = false;
                XPT_Data.m_result = true;
                OnShowMessage(Enum_LogType.Info, "上个流程执行结果: " + XPT_Data.m_result + m_Config.m_strCode + base.r_enuComingRelease);
                //
                var request = new ReadBarcode
                {
                    Ib_Code = base.m_Request.ma_edcIdentifier[0].m_strValue,
                    Dev_Code = m_Config.m_RecipeName
                };

                string jsonRequest = JsonConvert.SerializeObject(request);
                m_edcLogger.Info($"MoveInVerify Request: {jsonRequest}");
                OnShowMessage(Enum_LogType.Info, $"发送进站请求: {base.m_Request.ma_edcIdentifier[0].m_strValue}");

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                int iTimtout = 5000;
                var response = HTTPComm.Post(m_Config.m_ersaDownLineUrl, jsonRequest, iTimtout);
                m_edcLogger.Info($"MoveInVerify Response: {response}");

                Response result = GetResponse(response);
                if (result.Code == "0")
                {
                    base.r_edcResult = new Edc_Result
                    {
                        m_enuResultCode = Enum_ResponseCode.ok
                    };
                    OnShowMessage(Enum_LogType.Info, "Release Infeed  sucess: " + m_Config.m_strCode + base.r_enuComingRelease);
                    try
                    {
                        // 若共享串口未打开，尝试按 m_Config 当前配置打开
                        if (XPT_Data.SharedSerialPort == null || !XPT_Data.SharedSerialPort.IsOpen)
                        {
                            if (!XPT_Data.OpenSharedSerialPort(m_Config, m_edcLogger, out string openErr))
                            {
                                OnShowMessage(Enum_LogType.Error, "打开串口失败: " + openErr);
                                return Task.CompletedTask;
                            }
                        }

                        // 按配置的前缀/后缀拼接报文
                        string prefix = m_Config?.m_strPrefix ?? string.Empty;
                        string suffix = m_Config?.m_strSuffix ?? string.Empty;
                        string payload = ResolveControlChars(prefix) + m_Config.m_strCode + ResolveControlChars(suffix);

                        // 串口发送
                        XPT_Data.SharedSerialPort.Write(payload);
                        m_edcLogger?.Info($"SerialPort Send: [{prefix}]{m_Config.m_strCode}[{suffix}]", null);

                        OnShowMessage(Enum_LogType.Info, $"已通过 {XPT_Data.SharedSerialPort.PortName} @ {XPT_Data.SharedSerialPort.BaudRate} 发送：\r\n{m_Config.m_strCode}");
                    }
                    catch (Exception ex)
                    {
                        m_edcLogger?.Error("SerialPort Send Error: " + ex.Message, ex, "Fun_blnConnectMesPlatform", 0);
                        OnShowMessage(Enum_LogType.Error, "串口发送失败: " + ex.Message);
                    }
                }
                else
                {

                    base.r_edcResult = new Edc_Result
                    {
                        m_enuResultCode = Enum_ResponseCode.fehler
                    };

                    //base.r_enuComingRelease = Enum_ComingRelease.transfer;
                    OnShowMessage(Enum_LogType.Info, result.Message);
                    OnShowMessage(Enum_LogType.Info, "Release Infeed  fail: " + m_Config.m_strCode + base.r_enuComingRelease);

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

    private Response GetResponse(string message)
    {
        return JsonConvert.DeserializeObject<Response>(message);
    }

    /// <summary>
    /// 把形如 "STX (0x02)" / "CR (0x0D)" / "CRLF (0x0D 0x0A)" / "ETX (0x03)" / "无" 的描述转换为对应控制字符，
    /// 其它文本原样返回。空字符串直接返回空。
    /// </summary>
    private string ResolveControlChars(string token)
    {
        if (string.IsNullOrEmpty(token)) return string.Empty;
        string t = token.Trim();
        if (t.Equals("无", StringComparison.OrdinalIgnoreCase)) return string.Empty;

        switch (t.ToUpperInvariant())
        {
            case "STX (0X02)": return "\u0002";
            case "ETX (0X03)": return "\u0003";
            case "CR (0X0D)": return "\r";
            case "LF (0X0A)": return "\n";
            case "CRLF (0X0D 0X0A)": return "\r\n";
            default: return t;
        }
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
