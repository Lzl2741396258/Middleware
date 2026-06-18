using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MesXPT.XPT_MesService
{
    [DataContract]
    public class Edc_ChangeOverResponse
    {
        [DataMember(Name = "Code")]
        public int Code { get; set; }

        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Data")]
        public int Data { get; set; }

        public static Edc_ChangeOverResponse Success()
        {
            return new Edc_ChangeOverResponse
            {
                Code = 0,
                Message = "Success",
                Data = 0
            };
        }

        public static Edc_ChangeOverResponse Fail(string message)
        {
            return new Edc_ChangeOverResponse
            {
                Code = 1,
                Message = message,
                Data = 1
            };
        }

        public static Edc_ChangeOverResponse Busy()
        {
            return new Edc_ChangeOverResponse
            {
                Code = 0,
                Message = "Job Changing",
                Data = 2
            };
        }

        public static Edc_ChangeOverResponse NoChangeRequired(string message = "No Changeover Required")
        {
            return new Edc_ChangeOverResponse
            {
                Code = 0,
                Message = message,
                Data = 2
            };
        }
    }
}
