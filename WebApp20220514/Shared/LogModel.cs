using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp20220514.Shared
{
    public class LogModel : BaseApiResModel
    {
        public DateTime reqDateTime { get; set; }
        public object reqData { get; set; }
        public DateTime respDateTime { get; set; }
        public object respData { get; set; }
        public string reqPath { get; set; }
    }
}
