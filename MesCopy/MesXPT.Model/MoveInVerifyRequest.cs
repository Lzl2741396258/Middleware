using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MesXPT.Model
{
    public class MoveInVerifyRequest
    {
        [JsonProperty("CommandType")]
        public string CommandType { get; set; } = "BarcodeCheck";

        [JsonProperty("LocalTime")]
        public string LocalTime { get; set; }

        [JsonProperty("Line")]
        public string Line { get; set; }

        [JsonProperty("MachineCode")]
        public string MachineCode { get; set; }

        [JsonProperty("Barcode")]
        public string Barcode { get; set; }

        [JsonProperty("Lane")]
        public string Lane { get; set; }

        [JsonProperty("Layer")]
        public string Layer { get; set; }
    }


    public class MoveInVerifyResponse
    {
        [JsonProperty("ValueReturn")]
        public string ValueReturn { get; set; } // 0=正常，非0=错误码

        [JsonProperty("Message")]
        public string Message { get; set; } 

        [JsonProperty("Program")]
        public string Program { get; set; } 
    }

    
}