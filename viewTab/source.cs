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

        public static string TableName = "MSEventDataTemp";
    }
    static partial class Source
    {
        public static List<item> GetTestData(int start, int end)
        {
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
                                start,
                                end);
                //_list = conn.Query<item>(sql, new { devices = new[] { "800", "801", "803", "804", "805", "806", "807", "809", "810", "13", "134", "125", "157", "70" } }).ToList<item>();
                _list = conn.Query<item>(sql, new { devices = new[] { "0", "1" } }).ToList<item>();
            }
            return _list;
        }

    }
    class item
    {
        public int accountID;
        public string deviceID;
        public int timestamp;
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
