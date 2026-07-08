using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace MesXPT.XPT_MesService
{
    public class Edc_CorsEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // 添加 CORS 消息检查器
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new Edc_CorsMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}