using Ersa.Mes.Common;
using Ersa.Mes.Common.CommunicationService;
using Ersa.Mes.FileSystem.Extensions;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.MesFunctionFolder;
using MesXPT.CommunicationService;
using MesXPT.Model;
using MesXPT.XPT_MesTask;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MesDBG.DBG_MesFunction;

public class XPT_OutfeedCreateProtocolSelektiv : OutfeedCreateProtocol
{
    public XPT_Config m_Config;

    public static string m_outcreateProtoclCode = "";

    protected new Inf_Logger m_edcLogger;

    public string a_strCode = "";

    private Edc_XPTCommunicationService m_edcService { get; set; }

    public XPT_OutfeedCreateProtocolSelektiv(XPT_Config i_Config, Inf_Logger i_edcLogger)
        : base(i_Config.m_lstMesFunction, i_edcLogger)
    {
        m_Config = i_Config;
        m_edcService = new Edc_XPTCommunicationService(i_Config, i_edcLogger);
        m_edcLogger = i_edcLogger;
    }


    //public override void Fun_Test()
    //{
    //    Task.Run(() => Func_sendResult(new ITACUploadResult
    //    {
    //        CommandType = "ProductinInfo",
    //        LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    //        Line = "Line1",
    //        MachineCode = "M001",
    //        Barcode = "SN123",
    //        Program = "KSMART.2",
    //        Lane = "0",
    //        testResult = true,
    //        reviseResult = false,
    //        remark = null,
    //        testDataDetails = new List<TestDataDetailsItem>
    //        {
    //            new TestDataDetailsItem
    //            {
    //                testIndex = "1",
    //                testItem = "上1温区",
    //                testValue = "200",
    //                testResult = true
    //            },
    //            new TestDataDetailsItem
    //            {
    //                testIndex = "2",
    //                testItem = "上2温区",
    //                testValue = "210",
    //                testResult = true
    //            },
    //            new TestDataDetailsItem
    //            {
    //                testIndex = "3",
    //                testItem = "上3温区",
    //                testValue = "215",
    //                testResult = true
    //            },
    //            new TestDataDetailsItem
    //            {
    //                testIndex = "04",
    //                testItem = "氧气ppm",
    //                testValue = "500",
    //                testResult = true
    //            },
    //            new TestDataDetailsItem
    //            {
    //                testIndex = "04",
    //                testItem = "氮气压力mbar",
    //                testValue = "500",
    //                testResult = true
    //            }
    //        }
    //    }));
    //}

    protected override async Task Fun_blnConnectMesPlatform()
    {
        try
        {
            OnShowMessage(Enum_LogType.Info, "开始过站");
            List<Struct_ProtocolElement> list2 = base.m_Request.ma_sttProtocolElement.ToList();
            int a_i32Index2 = 1;
            List<TestDataDetailsItem> a_lstItem2 = new List<TestDataDetailsItem>();
            string a_strProgram = list2.Fun_strGetActualValue("Soldering program");
            if (m_Config.m_strITACMESType != "ITAC")
            {
                for (int l = 1; l <= 10; l++)
                {
                    TestDataDetailsItem item4 = new TestDataDetailsItem
                    {
                        testIndex = a_i32Index2++.ToString(),
                        testItem = $"上{l}温区",
                        testResult = true,
                        testValue = list2.Fun_strGetActualValue($"Temperature Top heating zone {l}", "0")
                    };
                    a_lstItem2.Add(item4);
                }
                for (int k = 1; k <= 10; k++)
                {
                    TestDataDetailsItem item3 = new TestDataDetailsItem
                    {
                        testIndex = a_i32Index2++.ToString(),
                        testItem = $"下{k}温区",
                        testResult = true,
                        testValue = list2.Fun_strGetActualValue($"Temperature Bottom heating zone {k}", "0")
                    };
                    a_lstItem2.Add(item3);
                }
                a_lstItem2.Add(new TestDataDetailsItem
                {
                    testIndex = a_i32Index2++.ToString(),
                    testItem = "轨道速度",
                    testResult = true,
                    testValue = list2.Fun_strGetActualValue("Conveyor speed")
                });
                a_lstItem2.Add(new TestDataDetailsItem
                {
                    testIndex = a_i32Index2++.ToString(),
                    testItem = "氧气ppm",
                    testResult = true,
                    testValue = list2.Fun_strGetActualValue("Residual oxygen ")
                });
                TestDataDetailsItem testDataDetailsItem = new TestDataDetailsItem();
                int num = a_i32Index2;
                testDataDetailsItem.testIndex = num.ToString();
                testDataDetailsItem.testItem = "氮气压力mbar";
                testDataDetailsItem.testResult = true;
                testDataDetailsItem.testValue = list2.Fun_strGetActualValue("N2 pressure ");
                a_lstItem2.Add(testDataDetailsItem);
                string a_strOutfeddTime = list2.Where((Struct_ProtocolElement s) => s.m_strName.Equals("Time")).FirstOrDefault()?.m_strActualValue;
                DateTime a_dtmOutfeed = new DateTime(2021, 1, 1);
                DateTime.TryParse(a_strOutfeddTime, out a_dtmOutfeed);
                string a_strProcessTime = list2.Where((Struct_ProtocolElement s) => s.m_strName.Equals("Process time ")).FirstOrDefault()?.m_strActualValue;
                int a_i32ProcessTime = 0;
                int.TryParse(a_strProcessTime, out a_i32ProcessTime);
                DateTime a_dtmInfeed = a_dtmOutfeed.AddSeconds(-a_i32ProcessTime);
                string a_strUserName = list2.Where((Struct_ProtocolElement s) => s.m_strName.Equals("User name")).FirstOrDefault()?.m_strActualValue;
                //Edc_RequestResultUpload
                ITACUploadResult request = new ITACUploadResult
                {
                    CommandType = "ProductinInfo",
                    LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Line = m_Config.m_Line,
                    MachineCode = m_Config.m_MachineCode,
                    Barcode = base.m_Request.ma_sttIdentifier[0].m_strValue,
                    //Barcode = base.m_Request.m_sttIdentifier.m_strValue,
                    Program = a_strProgram,
                    Lane = m_Config.m_Lane,
                    testResult = true,
                    reviseResult = true,
                    remark = null,
                    testDataDetails = a_lstItem2
                };

                Func_sendResult(request);
            }
            else
            {
                List<Struct_ProtocolElement> list = base.m_Request.ma_sttProtocolElement.ToList();
                int a_i32Index = 1;
                List<TestDataDetailsItem> a_lstItem = new List<TestDataDetailsItem>();
                for (int i = 1; i <= 10; i++)
                {
                    TestDataDetailsItem item = new TestDataDetailsItem
                    {
                        testIndex = a_i32Index++.ToString(),
                        testItem = $"上{i}温区",
                        testResult = true,
                        testValue = list.Fun_strGetActualValue($"Temperature Top heating zone {i}", "0"),
                        //ReviseResult = true,
                        //Location = null,
                        //PartNumber = null,
                        //ErrorCode = null,
                        //ReviseErrorCode = null,
                        //BlockID = 0,
                        //Remark = null,
                        //testExtData = null
                    };
                    a_lstItem.Add(item);
                }
                for (int j = 1; j <= 10; j++)
                {


                    TestDataDetailsItem item2 = new TestDataDetailsItem
                    {
                        testIndex = a_i32Index++.ToString(),
                        testItem = $"下{j}温区",
                        testResult = true,
                        testValue = list.Fun_strGetActualValue($"Temperature Bottom heating zone {j}", "0"),
                        //ReviseResult = true,
                        //Location = null,
                        //PartNumber = null,
                        //ErrorCode = null,
                        //ReviseErrorCode = null,
                        //BlockID = 0,
                        //Remark = null,
                        //testExtData = null
                    };
                    a_lstItem.Add(item2);
                }
                a_lstItem.Add(new TestDataDetailsItem
                {
                    testIndex = a_i32Index++.ToString(),
                    testItem = "轨道速度",
                    testResult = true,
                    testValue = list.Fun_strGetActualValue("Conveyor speed"),
                    //ReviseResult = true,
                    //Location = null,
                    //PartNumber = null,
                    //ErrorCode = null,
                    //ReviseErrorCode = null,
                    //BlockID = 0,
                    //Remark = null,
                    //testExtData = null
                });
                a_lstItem.Add(new TestDataDetailsItem
                {
                    testIndex = a_i32Index++.ToString(),
                    testItem = "氧气ppm",
                    testResult = true,
                    testValue = list.Fun_strGetActualValue("Residual oxygen "),
                    //ReviseResult = true,
                    //Location = null,
                    //PartNumber = null,
                    //ErrorCode = null,
                    //ReviseErrorCode = null,
                    //BlockID = 0,
                    //Remark = null,
                    //testExtData = null
                });
                TestDataDetailsItem testDataDetails = new TestDataDetailsItem();
                int num = a_i32Index;
                testDataDetails.testIndex = num.ToString();
                testDataDetails.testItem = "氮气压力mbar";
                testDataDetails.testResult = true;
                testDataDetails.testValue = list.Fun_strGetActualValue("N2 pressure ");
                //testDataDetails.ReviseResult = true;
                //testDataDetails.Location = null;
                //testDataDetails.PartNumber = null;
                //testDataDetails.ErrorCode = null;
                //testDataDetails.ReviseErrorCode = null;
                //testDataDetails.BlockID = 0;
                //testDataDetails.Remark = null;
                //testDataDetails.testExtData = null;
                a_lstItem.Add(testDataDetails);
                string a_strOutfeddTime2 = list.Where((Struct_ProtocolElement s) => s.m_strName.Equals("Time")).FirstOrDefault()?.m_strActualValue;
                DateTime a_dtmOutfeed2 = new DateTime(2021, 1, 1);
                DateTime.TryParse(a_strOutfeddTime2, out a_dtmOutfeed2);
                string a_strProcessTime2 = list.Where((Struct_ProtocolElement s) => s.m_strName.Equals("Process time ")).FirstOrDefault()?.m_strActualValue;
                int a_i32ProcessTime2 = 0;
                int.TryParse(a_strProcessTime2, out a_i32ProcessTime2);
                DateTime a_dtmInfeed2 = a_dtmOutfeed2.AddSeconds(-a_i32ProcessTime2);
                List<string> strings = new List<string>();
                string guid = Guid.NewGuid().ToString();
                string a_StartTime = a_dtmInfeed2.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
                string a_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
                ITACUploadResult iTACUploadResult = new ITACUploadResult
                {
                    CommandType = "ProductinInfo",
                    LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Line = m_Config.m_Line,
                    MachineCode = m_Config.m_MachineCode,
                    Barcode = base.m_Request.ma_sttIdentifier[0].m_strValue,
                    //Barcode = base.m_Request.m_sttIdentifier.m_strValue,
                    Program = a_strProgram,
                    Lane = m_Config.m_Lane,
                    testResult = true,
                    reviseResult = true,
                    remark = null,
                    //SessionID = guid,
                    //SerialNumber = base.m_strCodes,
                    //SerialNumberType = "",
                    //WorkNodeID = m_Config.m_strITACWorkNodeID,
                    //WorkNodeType = null,
                    //DeviceNo = null,
                    //StartTime = a_StartTime,
                    //EndDate = a_time,
                    //ProgramName = "",
                    //Description = null,
                    //User = m_Config.m_strITACUserID,
                    //PartSNList = strings,
                    //PanelXList = strings,
                    testDataDetails = a_lstItem,
                    //InsPanel = true
                };

                Func_sendResult(iTACUploadResult);

            }
            OnShowMessage(Enum_LogType.Info, "Platform -> Outfeed successed ...");
        }
        catch (Exception ex2)
        {
            Exception ex = ex2;
            string a_strMessage = "Connection Mes Plstform Error...Method:'Outfeed Create Protocol'  Details:'" + ex.Message + "'";
            OnShowMessage(Enum_LogType.Error, a_strMessage);
        }

    }


    // add send to Mes result
    private void  Func_sendResult(ITACUploadResult request)
    {
        try
        {
            OnShowMessage(Enum_LogType.Info, "SEND TO  MES SERVER");
            m_edcLogger.Info(" 上传 MES 条码 : " + base.m_Request.ma_sttIdentifier[0].m_strValue);
            int iTimtout = 5000;
            XPT_Data.m_outCreateProtocol = true;
            string strAlarmMessage = XPT_Data.m_strAlarmMessage;
            string i_strMessage = JsonConvert.SerializeObject(request);
            string url = m_Config.m_completedUrl;
            string sReval =  HTTPComm.Post(url, i_strMessage, iTimtout);
            m_edcLogger.Info(" 上传 MES 内容 : " + sReval);
            Edc_ResponseResultUpload resultResponse = GetResponse(sReval);
            OnShowMessage(Enum_LogType.Info, "Send mes return " + base.m_Request.ma_sttIdentifier[0].m_strValue + " successed...");


            if (resultResponse.ValueReturn == "0")
            {
                XPT_Data.m_strAlarmMessage = "send mes return sucess : " + resultResponse.Message;
                base.r_edcResult = new Edc_Result
                {
                    m_enuResultCode = Enum_ResponseCode.ok,
                    m_strText = XPT_Data.m_strAlarmMessage
                };
                OnShowMessage(Enum_LogType.Info, XPT_Data.m_strAlarmMessage);
                m_edcLogger.Info(" 上传 MES 结果 : " + XPT_Data.m_strAlarmMessage);
            }
            else
            {
                XPT_Data.m_strAlarmMessage = " mes return NG: " + resultResponse.Message;
                base.r_edcResult = new Edc_Result
                {
                    m_enuResultCode = Enum_ResponseCode.fehler,
                    m_strText = XPT_Data.m_strAlarmMessage
                };

                OnShowMessage(Enum_LogType.Info, XPT_Data.m_strAlarmMessage);
                m_edcLogger.Info(" 上传 MES 结果 : " + XPT_Data.m_strAlarmMessage);
            }

        }
        catch (Exception ex2)
        {
            Exception ex = ex2;
            string a_strMessage = "Connection Mes Plstform Error...Method:'Outfeed Create Protocol'  Details:'" + ex.Message + "'";
            m_edcLogger.Error("Connection Mes Plstform Error...Method:'Outfeed Create Protocol'  Details:'" + ex.Message + "'", null, "Fun_blnConnectMesPlatform", 130);
            OnShowMessage(Enum_LogType.Error, a_strMessage);
        }
    }


    private Edc_ResponseResultUpload GetResponse(string message)
    {
        return JsonConvert.DeserializeObject<Edc_ResponseResultUpload>(message);
    }
}
