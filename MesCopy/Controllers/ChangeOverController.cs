using Autofac;
using Ersa.Mes.Logging;
using Helpers;
using MesXPT.Model;
using MesXPT.XPT_MesService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Forms;

namespace Controllers
{
    public class ExcetecChangeOversController : ApiController
    {
        private static readonly object _jobLock = new object();
        private static bool _isJobChanging = false;
        private static string _currentProgram = string.Empty;

        private XPT_Config m_Config { get; set; }
        private Inf_Logger m_Logger { get; set; }


        // 消息显示事件（用于通知界面）
        public event Action<Enum_LogType, string> ShowMessage;
        public ExcetecChangeOversController()
        {
            m_Config = ContainerManager.Instance.Default.Resolve<XPT_Config>();
            m_Logger = ContainerManager.Instance.Default.Resolve<Inf_Logger>();
           
        }


        [HttpGet]
        public string Status()
        {
            return "GOOD";
        }

       

        [HttpPost]
        public Edc_ChangeOverResponse AutoChange(Edc_ChangeOverRequest request)
        {
            try
            {
                m_Logger.Info($"[WCF] AutoChange Request: {JsonConvert.SerializeObject(request)}");

                if (request == null)
                    return LogAndReturn(Edc_ChangeOverResponse.Fail("Request body is null"));

                if (string.IsNullOrEmpty(request.ProgramName))
                    return LogAndReturn(Edc_ChangeOverResponse.Fail("ProgramName is required"));

                // 文件存在性校验
                //string jobRoot = m_Config.m_programFilePath;
                //if (!string.IsNullOrEmpty(jobRoot) && !Directory.Exists(jobRoot))
                //{
                //    return LogAndReturn(Edc_ChangeOverResponse.Fail("Program file path not found: " + jobRoot));
                //}

                //bool jobExists = CheckProgramExists(jobRoot, request.ProgramName);
                //if (!jobExists)
                //{
                //    return LogAndReturn(Edc_ChangeOverResponse.Fail("Could Not Find Job File: " + request.ProgramName));
                //}
                //if (m_Config.m_RecipeName != request.MachineNo)
                //{
                //    return LogAndReturn(Edc_ChangeOverResponse.Fail("MahcineCode mismatch,current: " + m_Config.m_RecipeName));
                //}

                // 互斥锁检查
                lock (_jobLock)
                {
                    if (_isJobChanging)
                    {
                        return LogAndReturn(Edc_ChangeOverResponse.Busy());
                    }

                    // 检查当前程序是否相同
                    //m_Logger.Info($"Current Program: {XPT_Data.m_strCurrentDeviceProgram}");
                    //XPT_Data.ShowMessage(Enum_LogType.Info, "当前程序名： " + XPT_Data.m_strCurrentDeviceProgram);
                    //if (XPT_Data.m_strCurrentDeviceProgram == request.ProgramName)
                    //{
                    //    string path = m_Config.m_txtFilePath;
                    //    bool fileExists = Directory.GetFiles(path, "rel0*").Any();
                    //    if (!fileExists)
                    //    {
                    //        // 没有该文件，则执行写入操作
                    //        SendProgram(request.ProgramName);
                    //    }

                    //    return LogAndReturn(Edc_ChangeOverResponse.NoChangeRequired("Consistent With Current Program No Changeover Required"));
                    //}

                    _isJobChanging = true;
                }

                // 后台线程执行换型
                var result = ExecuteChangeOver(request).GetAwaiter().GetResult();
                return LogAndReturn(result);
            }
            catch (Exception ex)
            {
                m_Logger.Error($"[WCF] AutoChange Exception: {ex.Message}", ex, "AutoChange", 0);
                lock (_jobLock) { _isJobChanging = false; }
                return LogAndReturn(Edc_ChangeOverResponse.Fail(ex.Message));
            }
        }

        public Edc_ChangeOverResponse GetStatus()
        {
            return new Edc_ChangeOverResponse
            {
                Code = 0,
                Message = _isJobChanging ? "Job Changing" : "Idle",
                Data = _isJobChanging ? 2 : 0
            };
        }

        private async Task<Edc_ChangeOverResponse> ExecuteChangeOver(Edc_ChangeOverRequest request)
        {
            var result = new Edc_ChangeOverResponse
            {
                Code = 0,
                Message = "Start JobChange",
                Data = 0
            };

            try
            {
                m_Logger.Info($"[WCF] JobChange Start: Lane={request.LaneNo}, Program={request.ProgramName}");

                XPT_Data.m_blnActiveSelectProgram = true;

                XPT_Data.m_strLibrary = m_Config.m_txtLibrary;
                XPT_Data.m_strProgram = request.ProgramName;


                m_Logger.Info($"[WCF] JobChange Success: Program={request.ProgramName}");
                XPT_Data.ShowMessage(Enum_LogType.Info, "下发程序名： " + request.ProgramName);
                result.Message = "Job Change Success";
                result.Data = 0;
                //SendProgram(request.ProgramName);
            }
            catch (Exception ex)
            {
                m_Logger.Error($"[WCF] JobChange Exception: {ex.Message}", ex, "ExecuteChangeOver", 0);
                XPT_Data.ShowMessage(Enum_LogType.Error, "换型失败： " + ex.Message);
                result.Code = 1000;
                result.Message = "JobChange Exception: " + ex.Message;
            }
            finally
            {
                lock (_jobLock)
                {
                    _isJobChanging = false;
                }
            }

            return result;
        }


        private void SendProgram(string program)
        {

            try
            {

                MessageBox.Show("PROGRAM： " + program);


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

                    string type = string.Empty;

                    string source = string.IsNullOrEmpty(m_Config.m_Layer) ? "TOP" : m_Config.m_Layer;

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
        private bool CheckProgramExists(string jobRoot, string programName)
        {
            if (string.IsNullOrEmpty(jobRoot))
                return true; // 如果没有配置路径，跳过文件检查

            return Directory.Exists(Path.Combine(jobRoot, programName)) ||
                   Directory.GetFiles(jobRoot, programName + ".*", SearchOption.TopDirectoryOnly).Any();
        }

        private Edc_ChangeOverResponse LogAndReturn(Edc_ChangeOverResponse response)
        {
            m_Logger.Info($"[WCF] AutoChange Response: {JsonConvert.SerializeObject(response)}");
            return response;
        }
    }
}
