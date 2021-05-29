using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApplication1.LogerClass;
using WpfApplication1.LogerConstants;
using System.Windows;
using WpfApp1;

namespace WpfApplication1.ExceptionHandlersNamespace
{
    public static class ExceptionHandlers
    {
        public static void databaseConnectionExceptionHandler() {
            ///popup_window dbConnectionErrorWindow = new popup_window("Ошибка", "Ошибка при подключении к базе данных", "", "Завершить работу", System.Windows.Visibility.Hidden, "no_connection");
          //  dbConnectionErrorWindow.ShowDialog();
            Loger.log(LogType.DATABASE, LogMessage.DATABASE_CONNECTION_ERROR);
            killSession();
        }

        public static void databaseReadExceptionHandler()
        {
           // popup_window dbConnectionErrorWindow = new popup_window("Ошибка", "Ошибка при подключении к базе данных", "", "Завершить работу", System.Windows.Visibility.Hidden, "no_connection");
           // dbConnectionErrorWindow.ShowDialog();
            Loger.log(LogType.DATABASE, LogMessage.DATABASE_READ_ERROR);
            killSession();
        }



        public static void killSession() {
            Thread windowCloser = new Thread(() =>
            {
                Thread.Sleep(15000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (Window w in App.Current.Windows)
                    {
                        if (w != App.Current.MainWindow)
                            w.Close();
                    }
                });

            });
            windowCloser.Start();
        }
    }
}
