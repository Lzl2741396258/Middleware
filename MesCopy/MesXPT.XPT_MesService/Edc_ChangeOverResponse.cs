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
        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "data")]
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

        public static Edc_ChangeOverResponse NoChangeRequired(string message)
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
