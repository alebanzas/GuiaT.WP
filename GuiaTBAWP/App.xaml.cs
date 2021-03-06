﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Windows.ApplicationModel.Activation;
using GuiaTBA.Common;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Azure.Engagement;
using Microsoft.Phone.Notification;
using Microsoft.WindowsAzure.Messaging;

namespace GuiaTBAWP
{
    public partial class App
    {
        private const string AppName = "GUIATBAWP";
        private const string AppVersion = "2.3.0.8";

        public static ApplicationConfiguration Configuration { get; set; }

        public static MapViewModel MapViewModel { get; set; }
        
        // Easy access to the root frame
        public static PhoneApplicationFrame RootFrame { get; private set; }

        // Constructor
        public App()
        {
            MapViewModel = new MapViewModel();

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
            Debug.WriteLine(EngagementAgent.Instance.GetDeviceId());
            EngagementAgent.Instance.Init();
            EngagementReach.Instance.Init();

            InitPushNotification();

            Configuration = Config.Get<ApplicationConfiguration>() ?? new ApplicationConfiguration(AppName, AppVersion);
            Configuration.SetInitialConfiguration(AppName, AppVersion);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                PositionService.Initialize();
                ProgressBar.Initialize();
            });
        }

        private void InitPushNotification()
        {
            var channel = HttpNotificationChannel.Find("MyPushChannel");
            if (channel == null)
            {
                channel = new HttpNotificationChannel("MyPushChannel");
            }

            channel.ChannelUriUpdated += async (o, args) =>
            {
                var hub = new NotificationHub("guiatbanh", "Endpoint=sb://guiatbanhns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=7iaqH211eYZCT2AHHueK5y6POSjVsBQnSovubTyIAhg=");
                await hub.RegisterNativeAsync(args.ChannelUri.ToString());
            };
            channel.Open();
            channel.BindToShellToast();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            EngagementAgent.Instance.OnActivated(e);
            EngagementReach.Instance.OnActivated(e);

            Configuration = Config.Get<ApplicationConfiguration>() ?? new ApplicationConfiguration(AppName, AppVersion);
            Configuration.SetInitialConfiguration(AppName, AppVersion);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                PositionService.Initialize();
                ProgressBar.Initialize();
            });
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
            if (navigatedPage != null)
            {
                navigatedPage.Loaded += (sender, args) => SystemTray.SetBackgroundColor(navigatedPage, ColorTranslator.FromHtml("#10283a"));   
            }
        }

        #endregion
    }
}
