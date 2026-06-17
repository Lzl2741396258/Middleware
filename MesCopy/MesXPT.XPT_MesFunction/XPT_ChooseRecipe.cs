using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesFunction;

public class XPT_ChooseRecipe : ChooseRecipe
{
    protected new Inf_Logger m_edcLogger;
    private XPT_Config m_Config { get; set; }
    string lastChangeProgram = string.Empty;

    private readonly HttpClient _httpClient;

    public XPT_ChooseRecipe(XPT_Config i_Config, Inf_Logger i_edcLogger)
        : base(i_Config.m_lstMesFunction, i_edcLogger)
    {
        m_Config = i_Config;
        m_edcLogger = i_edcLogger;
        _httpClient = new HttpClient();
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

    //public override void Fun_Test()
    //{
    //    Task.Run(() => HandleMoveInVerifyAsync("123"));
    //}

    protected override async Task Fun_blnConnectMesPlatform()
    {
        base.r_edcResult = new Edc_Result
        {
            m_enuResultCode = Enum_ResponseCode.NichtDefiniert,
            m_strText = string.Empty
        };

        try
        {
            OnShowMessage(Enum_LogType.Info, "chooseRecip start! ");
            m_edcLogger.Info("chooseRecip start! ");

            string barcode = base.m_Request.m_sttIdentifier.m_strValue;
            OnShowMessage(Enum_LogType.Info, "条码： " + barcode);
            m_edcLogger.Info("条码: " + barcode);

            if (string.IsNullOrEmpty(barcode))
            {
                XPT_Data.m_strAlarmMessage += "条码为空";
                XPT_Data.m_result = false;
                m_edcLogger.Info("条码为空: chooseRecip failed");
                OnShowMessage(Enum_LogType.Info, "chooseRecip failed: 条码为空");
            }
            else
            {
                //HandleMoveInVerify(barcode);

                var moveInResponse = MoveInVerify(barcode);

                if (moveInResponse.ValueReturn != "0")
                {
                    // 进站校验失败
                    XPT_Data.m_strAlarmMessage = $"进站校验失败 MES返回信息: {moveInResponse.Message}";
                    XPT_Data.m_result = false;
                    m_edcLogger.Info($"进站校验失败 MES返回信息: {moveInResponse.Message}");
                    OnShowMessage(Enum_LogType.Error, XPT_Data.m_strAlarmMessage);
                    base.r_edcResult = new Edc_Result
                    {
                        m_enuResultCode = Enum_ResponseCode.fehler,
                        m_strText = "进站校验失败"
                    };
                }
                else
                {
                    // 进站校验成功，继续处理
                    m_Config.m_strCode = barcode;
                    XPT_Data.m_strAlarmMessage = $"当前条码为:{barcode}---{DateTime.Now}";

                    bool isChangeOver = bool.TryParse(m_Config.m_checkChangeOver, out bool checkChangeOver) && checkChangeOver;
                    if (isChangeOver || !string.IsNullOrEmpty(moveInResponse.Program))
                    {
                        if (!isChangeOver)
                        {
                            // 进站校验失败
                            XPT_Data.m_strAlarmMessage = $"请开启换型功能";
                            XPT_Data.m_result = false;
                            m_edcLogger.Info($"请开启换型功能");
                            OnShowMessage(Enum_LogType.Error, XPT_Data.m_strAlarmMessage);
                        }
                        else
                        {
                            // 根据接口返回信息组合生成程序名称
                            string programName = GenerateProgramNameFromResponse(moveInResponse);

                            if (!string.IsNullOrEmpty(programName))
                            {
                                m_Config.m_ErsachooseRecipe = true;
                                XPT_Data.m_strProgram = programName;
                                OnShowMessage(Enum_LogType.Info, $"生成程序名称: {XPT_Data.m_strProgram}");
                            }
                            else
                            {
                                XPT_Data.m_strProgram = "";
                                m_edcLogger.Info($"返回程序名为空");
                                OnShowMessage(Enum_LogType.Error, "返回程序名为空");
                            }
                        }
                        XPT_Data.m_result = true;
                        base.r_edcResult = new Edc_Result
                        {
                            m_enuResultCode = Enum_ResponseCode.ok,
                            m_strText = "进站校验成功"
                        };
                        //XPT_Data.m_strProgram = "2025"/;
                        base.r_strCurrentRecipe = XPT_Data.m_strProgram;
                        // 记录程序名更换成功信息
                        OnShowMessage(Enum_LogType.Info, $"程序名更换成功，程序: {XPT_Data.m_strProgram}");
                        m_edcLogger.Info($"程序名更换成功,程序: {XPT_Data.m_strProgram}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            m_edcLogger.Error($"{MethodBase.GetCurrentMethod().Name} Details: {ex.Message}", ex, "Fun_blnConnectMesPlatform", 92);
            XPT_Data.m_strAlarmMessage = $"处理异常: {ex.Message}";
            XPT_Data.m_result = false;

            base.r_edcResult = new Edc_Result
            {
                m_enuResultCode = Enum_ResponseCode.fehler,
                m_strText = ex.Message
            };
        }

        //await Task.CompletedTask;
    }

    private void HandleMoveInVerify(string barcode)
    {
        // 调用进站校验接口
        var moveInResponse = MoveInVerify(barcode);

        if (moveInResponse.ValueReturn != "0")
        {
            // 进站校验失败
            XPT_Data.m_strAlarmMessage = $"进站校验失败 MES返回信息: {moveInResponse.Message}";
            XPT_Data.m_result = false;
            m_edcLogger.Info($"进站校验失败 MES返回信息: {moveInResponse.Message}");
            OnShowMessage(Enum_LogType.Error, XPT_Data.m_strAlarmMessage);
            base.r_edcResult = new Edc_Result
            {
                m_enuResultCode = Enum_ResponseCode.fehler,
                m_strText = "进站校验失败"
            };
        }
        else
        {
            // 进站校验成功，继续处理
            m_Config.m_strCode = barcode;
            XPT_Data.m_strAlarmMessage = $"当前条码为:{barcode}---{DateTime.Now}";

            bool isChangeOver = bool.TryParse(m_Config.m_checkChangeOver, out bool checkChangeOver) && checkChangeOver;
            if (isChangeOver || !string.IsNullOrEmpty(moveInResponse.Program))
            {
                if (!isChangeOver)
                {
                    // 进站校验失败
                    XPT_Data.m_strAlarmMessage = $"请开启换型功能";
                    XPT_Data.m_result = false;
                    m_edcLogger.Info($"请开启换型功能");
                    OnShowMessage(Enum_LogType.Error, XPT_Data.m_strAlarmMessage);
                }
                else
                {
                    // 根据接口返回信息组合生成程序名称
                    string programName = GenerateProgramNameFromResponse(moveInResponse);

                    if (!string.IsNullOrEmpty(programName))
                    {
                        m_Config.m_ErsachooseRecipe = true;
                        XPT_Data.m_strProgram = programName;
                        OnShowMessage(Enum_LogType.Info, $"生成程序名称: {XPT_Data.m_strProgram}");
                    }
                    else
                    {
                        XPT_Data.m_strProgram = "";
                        m_edcLogger.Info($"返回程序名为空");
                        OnShowMessage(Enum_LogType.Error, "返回程序名为空");
                    }
                    //XPT_Data.m_blnActiveSelectProgram = true;
                    //XPT_Data.m_result = true;

                    XPT_Data.m_result = true;
                    base.r_edcResult = new Edc_Result
                    {
                        m_enuResultCode = Enum_ResponseCode.ok,
                        m_strText = "进站校验成功"
                    };
                    //XPT_Data.m_strProgram = "2025"/;
                    base.r_strCurrentRecipe = XPT_Data.m_strProgram;
                    // 记录程序名更换成功信息
                    OnShowMessage(Enum_LogType.Info, $"程序名更换成功，程序: {XPT_Data.m_strProgram}");
                    m_edcLogger.Info($"程序名更换成功,程序: {XPT_Data.m_strProgram}");
                }
            }
        }
    }

    /// <summary>
    /// 调用进站校验接口
    /// </summary>
    private MoveInVerifyResponse MoveInVerify(string messn)
    {
        string url = m_Config.m_ersaDownLineUrl;

        if (string.IsNullOrEmpty(url))
        {
            m_edcLogger.Error("ERSAMES接口地址未配置");
            return new MoveInVerifyResponse
            {
                ValueReturn = "500",
                Message = "ERSAMES接口地址未配置",
            };
        }

        try
        {
            var request = new MoveInVerifyRequest
            {
                CommandType = "BarcodeCheck",
                LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Line = m_Config.m_Account,
                MachineCode = m_Config.m_RecipeName,
                Barcode = messn,
                Lane = m_Config.m_Lane,
                Layer = m_Config.m_Layer
            };

            string jsonRequest = JsonConvert.SerializeObject(request);
            m_edcLogger.Info($"MoveInVerify Request: {jsonRequest}");
            OnShowMessage(Enum_LogType.Info, $"发送进站请求: {messn}");

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            _httpClient.Timeout = TimeSpan.FromSeconds(5);

            var response = _httpClient.PostAsync(url, content).GetAwaiter().GetResult();
            string responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            m_edcLogger.Info($"MoveInVerify Response: {responseJson}");

            var result = JsonConvert.DeserializeObject<MoveInVerifyResponse>(responseJson);
            return result;
        }
        catch (OperationCanceledException)
        {
            m_edcLogger.Error("进站接口请求超时", null, "MoveInVerify", 0);
            OnShowMessage(Enum_LogType.Error, "进站接口请求超时");
            return new MoveInVerifyResponse
            {
                Message = "请求超时（5000ms）",
                ValueReturn = "408"
            };
        }
        catch (HttpRequestException ex)
        {
            m_edcLogger.Error($"HTTP请求异常: {ex.Message}", ex, "MoveInVerify", 0);
            OnShowMessage(Enum_LogType.Error, $"进站接口调用失败: {ex.Message}");
            return new MoveInVerifyResponse
            {
                Message = $"HTTP请求错误: {ex.Message}",
                ValueReturn = "500"
            };
        }
        catch (Exception ex)
        {
            m_edcLogger.Error($"MoveInVerify Error: {ex.Message}", ex, "MoveInVerify", 0);
            OnShowMessage(Enum_LogType.Error, $"进站接口调用失败: {ex.Message}");
            return new MoveInVerifyResponse
            {
                Message = $"接口调用异常: {ex.Message}",
                ValueReturn = "500"
            };
        }
    }

    /// <summary>
    /// 根据进站接口返回信息生成程序名称
    /// 格式: ProductNo-ProductVersion_PCBSurfaceID
    /// </summary>
    private string GenerateProgramNameFromResponse(MoveInVerifyResponse response)
    {
        try
        {
            var productNo = response.Program;
            if (productNo == null)
            {
                m_edcLogger.Info("进站接口返回数据中缺少Program信息");
                return string.Empty;
            }

            string programName = $"{productNo}";

            m_edcLogger.Info($"成功生成程序名称: {programName}");
            return programName;
        }
        catch (Exception ex)
        {
            m_edcLogger.Error($"生成程序名称时发生错误: {ex.Message}", ex, "GenerateProgramNameFromResponse", 0);
            return null;
        }
    }

    private XPT_ersaOnlineResponse GetResponse(string message)
    {
        return JsonConvert.DeserializeObject<XPT_ersaOnlineResponse>(message);
    }
}