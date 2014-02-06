using System;
using System.Windows;
using System.Windows.Navigation;
using GuiaTBAWP;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ProgressBar = GuiaTBAWP.ProgressBar;

namespace BicicletaBAWP
{
    public partial class App : Application
    {
        private const string AppName = "BicicletaBAWP";
        private const string AppVersion = "1.7.1.0";

        public static ApplicationConfiguration Configuration { get; set; }

        // Easy access to the root frame
        public static PhoneApplicationFrame RootFrame { get; private set; }

        // Constructor
        public App()
        {
            Current.Host.Settings.EnableFrameRateCounter = false;
            // Global handler for uncaught exceptions. 
            // Note that exceptions thrown by ApplicationBarItem.Click will not get caught here.
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            ThemeManager.ToDarkTheme();

            LoadingBar.Instance.Initialize(RootFrame);
        }

        #region ApplicationLifetimeObjects

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            Configuration = Config.Get<ApplicationConfiguration>() ?? new ApplicationConfiguration(AppName, AppVersion);
            Configuration.SetInitialConfiguration(AppName, AppVersion);

            PositionService.Initialize();
            ProgressBar.Initialize();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Configuration = Config.Get<ApplicationConfiguration>() ?? new ApplicationConfiguration(AppName, AppVersion);
            Configuration.SetInitialConfiguration(AppName, AppVersion);

            PositionService.Initialize();
            ProgressBar.Initialize();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Config.Set(Configuration);
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Config.Set(Configuration);
            PositionService.Destroy();
        }

        #endregion

        #region exception handling

        // Code to execute if a navigation fails
        void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                //System.Diagnostics.Debugger.Break();
            }

            GeneralError.Exception = e.Exception;
            ((PhoneApplicationFrame)RootVisual).Source = new Uri("/GeneralError.xaml", UriKind.Relative);

            e.Handled = true;
        }

        // Code to execute on Unhandled Exceptions
        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is QuitException)
                return;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                //System.Diagnostics.Debugger.Break();
            }

            //MessageBox.Show(e.ExceptionObject.ToString(), "Unexpected error", MessageBoxButton.OK);
            GeneralError.Exception = e.ExceptionObject;
            ((PhoneApplicationFrame)RootVisual).Source = new Uri("/GeneralError.xaml", UriKind.Relative);

            e.Handled = true;
        }

        private class QuitException : Exception { }

        public static void Quit()
        {
            throw new QuitException();
        }

        #endregion

        #region application initialization

        // Avoid double-initialization
        private bool _phoneApplicationInitialized;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (_phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            _phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            var navigatedPage = (PhoneApplicationPage)e.Content;
            SetNavigatedPage(navigatedPage);

            // Set the root visual to allow the application to render
            RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
            RootFrame.Navigated += RootFrame_Navigated;
        }

        void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var navigatedPage = (PhoneApplicationPage)e.Content;
            SetNavigatedPage(navigatedPage);
        }

        void SetNavigatedPage(PhoneApplicationPage navigatedPage)
        {
            ProgressBar.UIElement = navigatedPage;
        }

        #endregion
    }
}