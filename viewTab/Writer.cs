using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viewTab
{
    static class Writer
    {
        public static void Do(string input)
        {
            using (FileStream file = new FileStream("temp.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter _writer = new StreamWriter(file, Encoding.UTF8))
            {
                _writer.Write(input);
            }
        }
    }
}
