using System;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using GuiaTBAWP.Bing.Geocode;
using GuiaTBAWP.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GuiaTBAWP.ViewModels;

namespace GuiaTBAWP
{
    public partial class App : Application
    {
        public static ApplicationConfiguration Configuration { get; set; }
        
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

        public static GeoCoordinateWatcher Ubicacion { get; set; }
        
        /// <value>Registered ID used to access map control and Bing maps service.</value>
        internal const string Id = "AgagZE2Ku0M0iPH8uolBeUSZUgHmGRrqbd-5etCjKym4dmTaH59yeS6Ka_kz_SDp";

        // Easy access to the root frame
        public PhoneApplicationFrame RootFrame { get; private set; }

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
        }        

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            Configuration = Config.Get<ApplicationConfiguration>() ?? new ApplicationConfiguration();
            Configuration.SetInitialConfiguration();

            if (Configuration.IsLocationEnabled)
            {
                Ubicacion = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                Ubicacion.StatusChanged += Ubicacion_StatusChanged;
                Ubicacion.PositionChanged += Ubicacion_PositionChanged;
                Ubicacion.MovementThreshold = 100;
                Ubicacion.Start();
            }
            else
            {
                MessageBox.Show("El servicio de localización se encuentra deshabilitado. Por favor asegúrese de habilitarlo en las Opciones de la aplicación para utilizar las caracteristicas que lo requeran.");
            }
        }

        private void Ubicacion_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Configuration.Ubicacion = e.Position;
            Ubicacion.Stop();
        }

        private void Ubicacion_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show(Ubicacion.Permission == GeoPositionPermission.Denied
                                        ? "El servicio de localización se encuentra deshabilitado. Por favor asegúrese de habilitarlo en las Opciones del dispositivo para utilizar las caracteristicas que lo requeran."
                                        : "El servicio de localización se encuentra sin funcionamiento.");
                    Ubicacion.Stop();
                    break;

                case GeoPositionStatus.Initializing: //Estado: Inicializando
                    Ubicacion.Stop();
                    break;

                case GeoPositionStatus.NoData: //Estado: Datos no disponibles
                    Ubicacion.Stop();
                    break;

                case GeoPositionStatus.Ready: //Estado: Servicio de localización disponible
                    Ubicacion.Start();
                    break;
            }
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
            Config.Set(Configuration);
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Config.Set(Configuration);
            Ubicacion.Dispose();
        }

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

    public enum ApplicationConfigurationKeys
    {
        IsInitialized,
        Location,
    }

    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            Ubicacion = new GeoPosition<GeoCoordinate>();
        }

        public void SetInitialConfiguration()
        {
            if (IsInitialized) return;

            InstallationId = Guid.NewGuid();
            IsLocationEnabled = true;
            IsInitialized = true;

            Config.Set(this);
        }

        public bool IsInitialized { get; set; }

        public bool IsLocationEnabled { get; set; }

        public Guid InstallationId { get; set; }

        public GeoPosition<GeoCoordinate> Ubicacion { get; set; }

        public double MinDiffGeography = 0.0001;

        public DateTime UltimaActualizacionBicicletas { get; set; }

        public bool InitialDataBicicletas { get; set; }

        public DateTime UltimaActualizacionTrenes { get; set; }

        public bool InitialDataTrenes { get; set; }

        public string Version
        {
            get
            {
                var v = "1.4.0.0";
#if DEBUG
                v += "d";
#endif
                return v;
            }
        }

        public string Name
        {
            get
            {
                return "GUIATBAWP";
            }
        }
    }
}
