namespace WpfApplication1.LogerClass
{
    public class Loger
    {
        static ILoger loger = new TxtLoger();       
        

        public Loger() {
            
        }

        public static void log(string logType, string message) {
            loger.log(logType, message);
        }

    }
}
