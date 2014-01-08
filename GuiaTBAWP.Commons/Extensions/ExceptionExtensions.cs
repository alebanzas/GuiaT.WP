using System;
using System.Net;
using System.Windows;

namespace GuiaTBAWP.Commons.Extensions
{
    public static class ExceptionExtensions
    {
        public static void Log(this Exception exception, bool showMsj = true)
        {
            Log(exception, null, null, showMsj);
        }

        public static void Log(this Exception exception, Func<int> preAction, bool showMsj = true)
        {
            Log(exception, preAction, null, showMsj);
        }

        public static void Log(this Exception exception, Func<int> preAction, Func<int> postAction, bool showMsj = true)
        {
            if (exception as WebException != null)
            {
                if (((WebException)exception).Status == WebExceptionStatus.RequestCanceled) return;
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (preAction != null)
                {
                    preAction();
                }
#if DEBUG
                MessageBox.Show(exception.ToString());
#endif
                if (showMsj)
                {
                    MessageBox.Show("Ocurrió un error al obtener la informacióna. Verifique su conexión a internet.");
                }
                if (postAction != null)
                {
                    postAction();
                }
            });

        }
    }
}
