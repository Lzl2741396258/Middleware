using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesService
{
    [DataContract]
    public class Edc_ChangeOverRequest
    {
        [DataMember(Name = "LineNo")]
        public string LineNo { get; set; }

        [DataMember(Name = "MachineNo")]
        public string MachineNo { get; set; }

        [DataMember(Name = "LaneNo")]
        public string LaneNo { get; set; }

        [DataMember(Name = "ProgramName")]
        public string ProgramName { get; set; }
    }
}
