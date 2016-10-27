using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viewTab
{
    static class DBConst
    {
        public static string ConnectionStringTest = @"Data Source=192.168.0.101;Initial Catalog=docflow3;Persist Security Info=True;User ID=sa;Password=@qwerty123";
        public static string ConnectionStringKurj = @"Data Source=10.253.2.2;Initial Catalog=db;Persist Security Info=True;User ID=sa;Password=123123qwE";

        public static string TableName = "MSEventData0";//"MSEventDataTemp";
        //public static string TableName = "MSEventDataTemp";
    }
    static partial class Source
    {
        public static List<item> getData()
        {                                //      Dates.oct13_day
            List<item> _list = GetTestData(1476374400, Dates.oct13_night, true);

            _list.Sort(delegate (item x, item y)
            {
                if (x.timestamp == 0 && y.timestamp == 0) return 0;
                else if (x.timestamp == 0) return -1;
                else if (y.timestamp == 0) return 1;
                else return x.timestamp.CompareTo(y.timestamp);
            });
            return _list;
        }

        public static List<item> GetTestData(int start, int end, bool longer = false)
        {
            long lstart = (long)start * 10000000;
            long lend = (long)end * 10000000;
            if (longer)
            {
                long _1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                lstart += _1970;
                lend += _1970;
            }
            List<item> _list;
            using (SqlConnection conn = new SqlConnection(DBConst.ConnectionStringTest))
            {
                string sql = string.Format(
                                @" SELECT deviceID, timestamp, statusCode, latitude, longitude, altitude, gpsAge, speedKPH, heading
                                FROM {0} 
                                WHERE deviceID NOT IN @devices   
                                AND timestamp > {1}
                                AND timestamp < {2}
                                ",
                                DBConst.TableName,
                                (!longer) ? start : lstart,
                                (!longer) ? end : lend
                                );
                //_list = conn.Query<item>(sql, new { devices = new[] { "800", "801", "803", "804", "805", "806", "807", "809", "810", "13", "134", "125", "157", "70" } }).ToList<item>();
                _list = conn.Query<item>(sql, new { devices = new[] { "0", "1" } }).ToList<item>();
            }
            return _list;
        }   
    }
    public class item
    {
        public int accountID;
        public string deviceID;
        public long timestamp;
        public int statusCode;
        public double latitude;
        public double longitude;
        public double altitude;
        public double gpsAge;
        public int speedKPH;
        public double heading;
        public double odometerKM;

        public int CompareTo(item compareItem)
        {
            // A null value means that this object is greater.
            if (compareItem == null)
                return 1;

            else
                return this.timestamp.CompareTo(compareItem.timestamp);
        }
    }
}
