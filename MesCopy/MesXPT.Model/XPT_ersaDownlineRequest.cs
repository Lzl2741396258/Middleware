using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class XPT_ersaDownlineRequest
    {
        [JsonProperty("snCode")]
        public string SnCode { get; set; }

        [JsonProperty("processCode")]
        public string ProcessCode { get; set; }

        [JsonProperty("equipmentCode")]
        public string EquipmentCode { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonProperty("adapterPlateCode")]
        public string AdapterPlateCode { get; set; }

        [JsonProperty("productStatus")]
        public int ProductStatus { get; set; }

        [JsonProperty("stationStatus")]
        public string StationStatus { get; set; }

        [JsonProperty("actualCycleTime")]
        public decimal ActualCycleTime { get; set; }

        [JsonProperty("isCheckComplete")]
        public bool IsCheckComplete { get; set; } = true;

        [JsonProperty("qualitDataDtos")]
        public List<ErsaDownlineQualityData> QualitDatas { get; set; }

        public XPT_ersaDownlineRequest()
        {
            QualitDatas = new List<ErsaDownlineQualityData>();
        }

    }



}
