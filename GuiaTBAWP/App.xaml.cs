using System;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GuiaTBAWP.ViewModels;

namespace GuiaTBAWP
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        public GeoCoordinate Ubicacion { get; set; }

        public DateTime UltimaActualizacionBicicletas
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("UltimaActualizacionBicicletas"))
                    IsolatedStorageSettings.ApplicationSettings["UltimaActualizacionBicicletas"] = DateTime.UtcNow;
                return (DateTime)IsolatedStorageSettings.ApplicationSettings["UltimaActualizacionBicicletas"];
            }
            set { IsolatedStorageSettings.ApplicationSettings["UltimaActualizacionBicicletas"] = value; }
        }

        public bool InitialDataBicicletas
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("InitialDataBicicletas"))
                    IsolatedStorageSettings.ApplicationSettings["InitialDataBicicletas"] = false;
                return (bool)IsolatedStorageSettings.ApplicationSettings["InitialDataBicicletas"];
            }
            set { IsolatedStorageSettings.ApplicationSettings["InitialDataBicicletas"] = value; }
        }

        public DateTime UltimaActualizacionTrenes
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("UltimaActualizacionTrenes"))
                    IsolatedStorageSettings.ApplicationSettings["UltimaActualizacionTrenes"] = DateTime.UtcNow;
                return (DateTime)IsolatedStorageSettings.ApplicationSettings["UltimaActualizacionTrenes"];
            }
            set { IsolatedStorageSettings.ApplicationSettings["UltimaActualizacionTrenes"] = value; }
        }

        public bool InitialDataTrenes
        {
            get
            {
                if (!IsolatedStorageSettings.ApplicationSettings.Contains("InitialDataTrenes"))
                    IsolatedStorageSettings.ApplicationSettings["InitialDataTrenes"] = false;
                return (bool)IsolatedStorageSettings.ApplicationSettings["InitialDataTrenes"];
            }
            set { IsolatedStorageSettings.ApplicationSettings["InitialDataTrenes"] = value; }
        }

        /// <value>Registered ID used to access map control and Bing maps service.</value>
        internal const string Id = "AgagZE2Ku0M0iPH8uolBeUSZUgHmGRrqbd-5etCjKym4dmTaH59yeS6Ka_kz_SDp";

        // Easy access to the root frame
        public PhoneApplicationFrame RootFrame { get; private set; }

        public bool TimerUsed { get; set; }

        // Constructor
        public App()
        {
            Current.Host.Settings.EnableFrameRateCounter = true;
            // Global handler for uncaught exceptions. 
            // Note that exceptions thrown by ApplicationBarItem.Click will not get caught here.
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            ThemeManager.ToDarkTheme();
            
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("localizacion"))
                IsolatedStorageSettings.ApplicationSettings["localizacion"] = true;
        }        

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                //System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
