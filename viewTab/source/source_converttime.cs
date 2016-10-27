using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace viewTab
{
    static partial class Source
    {
        public static string CorrectTime(int ticks)
        {
            Regex regex = new Regex(@",");
            string _dt = time2time.GetDateTimeByEcho(ticks).ToString();
            return regex.Replace(_dt, ".");
        }
        public static string CorrectDouble(double input)
        {
            Regex regex = new Regex(@",");
            string _dt = input.ToString();
            return regex.Replace(_dt, ".");
        }
    }
}
