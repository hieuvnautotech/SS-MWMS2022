using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD
{
    public class UTL
    {
        public static string ConvertToString(int value, int lenght)
        {
            return value.ToString("D" + lenght);
        }

        public static string ConvertToDatetimeString(string value)
        {
            return value.Substring(0, 4) + "-" + value.Substring(4, 2) + "-" + value.Substring(6, 2) + " " + value.Substring(8, 2) + ":" + value.Substring(10, 2) + ":" + value.Substring(12, 2);
        }
    }
}