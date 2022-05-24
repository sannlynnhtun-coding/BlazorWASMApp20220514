using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp20220514
{
    public static class ApiUrl
    {
        public static string Sale { get; } = "api/sale";
        public static void ToConsole(this object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
