using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class ErsaOnlineData
    {
        [JsonProperty("snCode")]
        public string SnCode { get; set; }

        [JsonProperty("orderNo")]
        public string PlanNo { get; set; }

        [JsonProperty("productTypeCode")]
        public string ProductTypeCode { get; set; }

        [JsonProperty("productTypeCodeWithVer")]
        public string ProductTypeCodeWithVer { get; set; }
    }
}
