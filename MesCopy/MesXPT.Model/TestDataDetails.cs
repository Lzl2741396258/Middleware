using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.Model
{
    public class TestDataDetails
    {
        public string TestIndex { get; set; }

        public string TestItem { get; set; }

        public string MaxValue { get; set; }

        public string MinValue { get; set; }

        public string TestValue { get; set; }

        public string unit { get; set; }

        public bool TestResult { get; set; }

        public bool ReviseResult { get; set; }

        public string Location { get; set; }

        public string PartNumber { get; set; }

        public string ErrorCode { get; set; }

        public string ReviseErrorCode { get; set; }

        public int BlockID { get; set; }

        public string Remark { get; set; }

        public string testExtData { get; set; }
    }

}
