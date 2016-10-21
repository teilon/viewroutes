using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viewTab
{
    static class time2time
    {
        public static DateTime GetDateTimeByEcho(int ticks)
        {
            long _1970 = ticks;
            long _0001 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_1970).Ticks;
            return new DateTime(_0001);
        }
    }
}
