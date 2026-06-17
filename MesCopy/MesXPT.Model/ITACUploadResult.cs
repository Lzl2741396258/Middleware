using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class ITACUploadResult
    {
        public string CommandType { get; set; }

        public string LocalTime { get; set; }

        public string Line { get; set; }

        public string MachineCode { get; set; }

        public string Barcode { get; set; }

        public string Program { get; set; }

        public string Lane { get; set; }
        public bool testResult { get; set; }

        public bool reviseResult { get; set; }
        public string remark { get; set; }

        //public string SessionID { get; set; }

        //public string SerialNumber { get; set; }

        //public string SerialNumberType { get; set; }

        //public string WorkNodeID { get; set; }

        //public string WorkNodeType { get; set; }

        //public string DeviceNo { get; set; }

        //public string StartTime { get; set; }

        //public string EndDate { get; set; }

        //public string ProgramName { get; set; }

        //public string Description { get; set; }

        //public string User { get; set; }

        //public List<string> PartSNList { get; set; }

        //public List<string> PanelXList { get; set; }

       // public List<TestDataDetails> TestDataDetails { get; set; }
        public List<TestDataDetailsItem> testDataDetails { get; set; }

    }
}
