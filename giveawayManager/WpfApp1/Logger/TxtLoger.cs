using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.LogerClass;

namespace WpfApplication1.LogerClass
{
    public class TxtLoger : ILoger
    {
        public void log(string logType, string message)
        {
            using (StreamWriter file = new StreamWriter("logs.txt", true))
            {
                file.WriteLine(logType + DateTime.Now.ToString() + message);
            }
        }
    }
}
