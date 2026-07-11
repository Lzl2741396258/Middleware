using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Helpers
{
    public class WebServer
    {
        private static Lazy<WebServer> _lazy = new Lazy<WebServer>(() => new WebServer());
        private IDisposable _disposed;
        public event EventHandler<string> LogMessaged;
        public WebServer()
        {

        }
        public static WebServer Instance => _lazy.Value;
        public string BaseAddress { get; private set; } = "http://127.0.0.1:9000";
        public void SetBaseAddress(string url = "http://127.0.0.1:9000")
        {
            Instance.BaseAddress = url;
        }

        public void StartWebServer()
        {
            if (string.IsNullOrEmpty(BaseAddress))
            {
                LogMessaged?.Invoke(this, "WebServer Start Ex:BaseAddress is empty!");
                return;
            }
            try
            {
                StopWebServer();
                _disposed = WebApp.Start<Startup>(url: BaseAddress);
                LogMessaged?.Invoke(this, "WebServer Start Successful!");
            }
            catch (Exception ex)
            {
                LogMessaged?.Invoke(this, $"WebServer Start Failed!\n{ex.Message}");
            }

        }
        public void StopWebServer()
        {
            if (_disposed != null)
            {
                _disposed.Dispose();
                LogMessaged?.Invoke(this, "WebServer Stop!");
            }
        }

    }


    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {

            appBuilder.UseCors(CorsOptions.AllowAll);

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            // 配置JSON序列化设置
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // 配置路由
            config.Routes.Clear();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
