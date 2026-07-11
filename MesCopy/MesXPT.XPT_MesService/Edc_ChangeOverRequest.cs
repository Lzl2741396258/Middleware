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
        [DataMember(Name = "lineNo")]
        public string LineNo { get; set; }

        [DataMember(Name = "machineNo")]
        public string MachineNo { get; set; }

        [DataMember(Name = "laneNo")]
        public string LaneNo { get; set; }

        [DataMember(Name = "programName")]
        public string ProgramName { get; set; }
    }
}
