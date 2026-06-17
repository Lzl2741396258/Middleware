using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class ErsaDownlineReturnQualityData
    {

        [JsonProperty("SnCode")]
        public string SnCode { get; set; }

        [JsonProperty("jumpToPosition")]
        public string JumpToPosition { get; set; }
     
    }
}
