using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class XPT_ersaDownlineResponse
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }


        [JsonProperty("message")]
        public string Message { get; set; }


        [JsonProperty("solution")]
        public string solution { get; set; }

        [JsonProperty("data")]
        public ErsaDownlineReturnQualityData Data { get; set; }

        public XPT_ersaDownlineResponse()
        {
            Data = new ErsaDownlineReturnQualityData();
        }
    }
}
