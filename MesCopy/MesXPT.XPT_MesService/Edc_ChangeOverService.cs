using Ersa.Mes.Logging;
using MesXPT.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Edc_ChangeOverService : Inf_IChangeOverService
    {
        private static readonly object _jobLock = new object();
        private static bool _isJobChanging = false;
        private static string _currentProgram = string.Empty;

        private XPT_Config m_Config { get; set; }
        private Inf_Logger m_Logger { get; set; }

        // 消息显示事件（用于通知界面）
        public event Action<Enum_LogType, string> ShowMessage;

        public Edc_ChangeOverService(XPT_Config config, Inf_Logger logger)
        {
            m_Config = config;
            m_Logger = logger;
        }

        /// <summary>
        /// 显示消息到界面
        /// </summary>
        private void OnShowMessage(Enum_LogType i_enuLogType, string i_strMessage)
        {
            ShowMessage?.Invoke(i_enuLogType, i_strMessage);
        }

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

                // 互斥锁检查
                lock (_jobLock)
                {
                    if (_isJobChanging)
                    {
                        return LogAndReturn(Edc_ChangeOverResponse.Busy());
                    }

                    // 检查当前程序是否相同
                    if (_currentProgram == request.ProgramName)
                    {
                        return LogAndReturn(Edc_ChangeOverResponse.NoChangeRequired("Consistent With Current Program"));
                    }

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

                // 设置全局程序名（触发换型）
                XPT_Data.m_strProgram = request.ProgramName;
                m_Config.m_ErsachooseRecipe = true;
                XPT_Data.m_blnActiveSelectProgram = true;
                // 更新当前程序记录
                _currentProgram = request.ProgramName;

                m_Logger.Info($"[WCF] JobChange Success: Program={request.ProgramName}");
                OnShowMessage(Enum_LogType.Info, "换型成功： " + request.ProgramName);
                result.Message = "Job Change Success";
                result.Data = 0;
            }
            catch (Exception ex)
            {
                m_Logger.Error($"[WCF] JobChange Exception: {ex.Message}", ex, "ExecuteChangeOver", 0);
                OnShowMessage(Enum_LogType.Error, "换型失败： " + ex.Message);
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
