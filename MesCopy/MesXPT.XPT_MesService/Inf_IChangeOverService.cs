using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;

namespace MesXPT.XPT_MesService
{
    [ServiceContract]
    public interface Inf_IChangeOverService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
                   UriTemplate = "/api/ChangeOver/AutoChange",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        Edc_ChangeOverResponse AutoChange(Edc_ChangeOverRequest request);

        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //           UriTemplate = "/api/ChangeOver/Status",
        //           ResponseFormat = WebMessageFormat.Json)]
        //Edc_ChangeOverResponse GetStatus();
    }
}
