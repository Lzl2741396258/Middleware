using Newtonsoft.Json;
using System;

namespace MesXPT.Model
{
    public class ErsaDownlineQualityData
    {
        /*        [JsonProperty("library")]
                public string Library { get; set; }

                [JsonProperty("program")]
                public string Program { get; set; }

                [JsonProperty("outfeedTime")]
                public DateTime OutfeedTime { get; set; }

                [JsonProperty("infeedTime")]
                public DateTime InfeedTime { get; set; }

                [JsonProperty("cycleCode")]
                public string CycleCode { get; set; }*/

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }


/*        [JsonProperty("preheatunit11Temperature")]
        public float PreheatUnit11Temperature { get; set; }

        [JsonProperty("flux")]
        public DateTime Flux { get; set; }

        [JsonProperty("timePreheatunit11")]
        public DateTime TimePreheatunit11 { get; set; }

        [JsonProperty("timeSolderingunit1")]
        public DateTime TimeSolderingunit1 { get; set; }

        [JsonProperty("soldertemperature")]
        public float SolderTemperature { get; set; }

        [JsonProperty("offsetSolderpot")]
        public float OffsetSolderpot { get; set; }*/
    }
}
