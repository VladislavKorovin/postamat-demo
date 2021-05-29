using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.LogerConstants
{
    public class LogerConstants
    {
       // public LogType logType = new LogType();
    }

    public class LogType 
    {
        public static String AD = "#AD# ";
        public static String VAULT = "#VAULT# ";
        public static String DATABASE = "#DATABASE# ";
        public static String INC_CREATION = "#INC CREATION# ";
        public static String SYSTEM = "#SYSTEM# ";
        public static String EXCEPTION = "#!!!EXCEPTION!!!# ";
    }

    public class LogMessage {
        public static String DATABASE_CONNECTION_ERROR = "Database connection attempt failed";
        public static String DATABASE_READ_ERROR = "Database read attempt failed";
    }

   
}
