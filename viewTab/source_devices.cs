using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viewTab
{
    static partial class Source
    {
        public static void SaveDevicesToFile()
        {
            List<device> _list;
            using (SqlConnection conn = new SqlConnection(DBConst.ConnectionStringTest))
            {
                string sql = @" select distinct deviceID from MSEventdataTemp";
                _list = conn.Query<device>(sql).ToList<device>();
            }

            //using (FileStream file = new FileStream("by_boards.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
            //using (StreamWriter _writer = new StreamWriter(file, Encoding.UTF8))

            StringBuilder _sb = new StringBuilder();
            _sb.Append("[");
            foreach (device i in _list)
            {
                _sb.Append("{");
                _sb.AppendFormat("deviceID:{0}", i.deviceID);
                _sb.Append("},\n");
            }
            _sb.Append("]");

            using (FileStream _file = new FileStream("Devices.json", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            using (StreamWriter _writer = new StreamWriter(_file, Encoding.UTF8))
            {
                _writer.Write(_sb.ToString());
            }
        }
    }
    class device
    {
        public string deviceID;
    }
}
