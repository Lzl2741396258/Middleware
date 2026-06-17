using Newtonsoft.Json;

namespace MesXPT.Model
{
    public class XPT_ersaOnlineRequest
    {
        [JsonProperty("snCode")]
        public string SnCode { get; set; }

        [JsonProperty("processCode")]
        public string ProcessCode { get; set; }

        [JsonProperty("equipmentCode")]
        public string EquipmentCode { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }
    }
}
