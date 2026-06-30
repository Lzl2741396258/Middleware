using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class ReadBarcode
    {
        [JsonProperty("lb_code")]
        public string Ib_Code { get; set; }

        [JsonProperty("dev_code")]
        public string Dev_Code { get; set; }
    }

    public class Response
    {
        [JsonProperty("code")]
        public string Code { get; set; } // 0=正常，非0=错误码

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
