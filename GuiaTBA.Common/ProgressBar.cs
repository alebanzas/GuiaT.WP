using System.Windows;
using GuiaTBAWP.Commons.Helpers;
using Microsoft.Phone.Shell;

namespace GuiaTBA.Common
{
    public static class ProgressBar
    {
        private static readonly ProgressIndicator Progress = new ProgressIndicator();
        public static UIElement UIElement;
        public static void Initialize()
        {
            Progress.IsVisible = true;
            Progress.IsIndeterminate = true;
        }

        public static void Show(string msj, bool showProgress = true)
        {
            Progress.Text = msj;
            Progress.IsIndeterminate = showProgress;
            SystemTray.SetIsVisible(UIElement, true);
            SystemTray.SetProgressIndicator(UIElement, Progress);
            SystemTray.SetBackgroundColor(UIElement, ColorTranslator.FromHtml("#10283a"));
            SystemTray.SetForegroundColor(UIElement, ColorTranslator.FromHtml("#FFFFFF"));
        }

        public static void Hide()
        {
            SystemTray.SetProgressIndicator(UIElement, null);
        }
    }
}
