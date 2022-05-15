using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp20220514.Shared
{
    public class ResponseModel
    {
        public EnumRespCode respCode { get; set; }
        public string respDesp { get; set; }
    }

    public enum EnumRespCode
    {
        success,
        warning,
        information,
        error
    }
}
