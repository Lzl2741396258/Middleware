using Ersa.Mes.Logging;
using MesXPT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesService
{
    public class Edc_ChangeOverServiceHost : IDisposable
    {
        private WebServiceHost _serviceHost;
        private XPT_Config m_Config { get; set; }
        private Inf_Logger m_Logger { get; set; }
        private string m_BaseAddress { get; set; }
        private Edc_ChangeOverService _serviceInstance;

        // 消息显示事件（供外部订阅）
        public event Action<Enum_LogType, string> OnShowMessage;
        public bool IsRunning => _serviceHost?.State == CommunicationState.Opened;

        public Edc_ChangeOverServiceHost(XPT_Config config, Inf_Logger logger)
        {
            m_Config = config;
            m_Logger = logger;
        }

        public bool Start(string ipAddress = "127.0.0.1", int port = 8080)
        {
            try
            {
                if (IsRunning)
                {
                    m_Logger.Info("[WCF] Service is already running");
                    return true;
                }

                m_BaseAddress = $"http://{ipAddress}:{port}/";
                m_Logger.Info($"[WCF] Starting service at {m_BaseAddress}");

                // 创建服务实例
                _serviceInstance = new Edc_ChangeOverService(m_Config, m_Logger);
                // 订阅服务内部消息事件
                _serviceInstance.ShowMessage += (type, message) => OnShowMessage?.Invoke(type, message);

                // 创建 WebServiceHost
                _serviceHost = new WebServiceHost(_serviceInstance, new Uri(m_BaseAddress));

                // 添加端点
                var endpoint = _serviceHost.AddServiceEndpoint(
                    typeof(Inf_IChangeOverService),
                    new WebHttpBinding(),
                    ""
                );

                // 配置行为
                endpoint.Behaviors.Add(new WebHttpBehavior
                {
                    AutomaticFormatSelectionEnabled = true,
                    HelpEnabled = true
                });

                // 启用元数据
                var smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpsGetEnabled = false
                };
                _serviceHost.Description.Behaviors.Add(smb);

                // 启动服务
                _serviceHost.Open();

                m_Logger.Info($"[WCF] Service started successfully at {m_BaseAddress}");
                m_Logger.Info($"[WCF] Endpoint: {m_BaseAddress}api/ChangeOver/AutoChange");
                //m_Logger.Info($"[WCF] Status: {m_BaseAddress}api/ChangeOver/Status");

                return true;
            }
            catch (Exception ex)
            {
                m_Logger.Error($"[WCF] Failed to start service: {ex.Message}", ex, "Start", 0);
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                if (_serviceHost != null && _serviceHost.State != CommunicationState.Closed)
                {
                    m_Logger.Info("[WCF] Stopping service...");
                    _serviceHost.Close();
                    m_Logger.Info("[WCF] Service stopped");
                }
            }
            catch (Exception ex)
            {
                m_Logger.Error($"[WCF] Failed to stop service: {ex.Message}", ex, "Stop", 0);
                _serviceHost?.Abort();
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
