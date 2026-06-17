using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class XPT_ersaOnlineResponse
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("solution")]
        public string solution { get; set; }

        [JsonProperty("data")]
        public ErsaOnlineData Data { get; set; }

        public XPT_ersaOnlineResponse()
        {
            Data = new ErsaOnlineData();
        }
    }
}
