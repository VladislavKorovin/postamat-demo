using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.LogerClass
{
    public interface ILoger
    {
        void log(String logType, String Message);
    }
}
