using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace MesXPT.XPT_MesService
{
    public class Edc_CorsMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // 检查是否是 OPTIONS 预检请求
            if (request.Properties.ContainsKey("httpRequest"))
            {
                var httpRequest = (HttpRequestMessageProperty)request.Properties["httpRequest"];
                if (httpRequest.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
                {
                    return "OPTIONS"; // 返回标记，告诉 BeforeSendReply 这是预检请求
                }
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // 添加 CORS 响应头
            if (reply.Properties.ContainsKey("httpResponse"))
            {
                var httpResponse = (HttpResponseMessageProperty)reply.Properties["httpResponse"];

                // 允许所有来源
                httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                // 允许的 HTTP 方法
                httpResponse.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                // 允许的请求头
                httpResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                // 预检请求缓存时间（秒）
                httpResponse.Headers.Add("Access-Control-Max-Age", "3600");

                // 如果是 OPTIONS 预检请求，直接返回 200 状态码
                if (correlationState is string state && state == "OPTIONS")
                {
                    httpResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    httpResponse.StatusDescription = "OK";

                    // 创建空的回复消息
                    reply = Message.CreateMessage(MessageVersion.None, "");
                    var emptyResponse = new HttpResponseMessageProperty();
                    emptyResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                    emptyResponse.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    emptyResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                    emptyResponse.Headers.Add("Access-Control-Max-Age", "3600");
                    emptyResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    reply.Properties["httpResponse"] = emptyResponse;
                }
            }
        }
    }
}