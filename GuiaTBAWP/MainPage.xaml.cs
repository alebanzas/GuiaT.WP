using System;
using System.Windows;
using System.Device.Location;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using GuiaTBAWP.Bing.Route;
using GuiaTBAWP.Converters;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using GuiaTBAWP.Models;
using GuiaTBAWP.Helpers;

namespace GuiaTBAWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Consts

        /// <value>Default map zoom level.</value>
        private const double DefaultZoomLevel = 18.0;

        /// <value>Maximum map zoom level allowed.</value>
        private const double MaxZoomLevel = 21.0;

        /// <value>Minimum map zoom level allowed.</value>
        private const double MinZoomLevel = 1.0;

        public PushpinModel DefaultPushPin = new PushpinModel
                                                 {
                                                     Location = DefaultLocation,
                                                     Icon = new Uri("/Resources/Icons/Pushpins/PushpinLocation.png", UriKind.Relative)
                                                 };

        #endregion

        #region Fields

        /// <value>Provides credentials for the map control.</value>
        private readonly CredentialsProvider _credentialsProvider = new ApplicationIdCredentialsProvider(App.Id);

        /// <value>Default location coordinate (Buenos Aires).</value>
        private static readonly GeoCoordinate DefaultLocation = new GeoCoordinate(-34.6085, -58.3736);

        /// <value>Collection of pushpins available on map.</value>
        private readonly ObservableCollection<PushpinModel> _pushpins = new ObservableCollection<PushpinModel>();

        /// <value>Collection of calculated map routes.</value>
        private readonly ObservableCollection<RouteModel> _routes = new ObservableCollection<RouteModel>();

        /// <value>Collection of calculated map routes itineraries.</value>
        private readonly ObservableCollection<ItineraryItem> _itineraries = new ObservableCollection<ItineraryItem>();

        /// <value>Map zoom level.</value>
        private double _zoom;

        /// <value>Map center coordinate.</value>
        private GeoCoordinate _center;

        #endregion

        #region Properties

        public bool HasDirections
        {
            get { return Itineraries.Count > 0; }
        }

        /// <summary>
        /// Gets the credentials provider for the map control.
        /// </summary>
        public CredentialsProvider CredentialsProvider
        {
            get { return _credentialsProvider; }
        }

        /// <summary>
        /// Gets or sets the map zoom level.
        /// </summary>
        public double Zoom
        {
            get { return _zoom; }
            set
            {
                var coercedZoom = Math.Max(MinZoomLevel, Math.Min(MaxZoomLevel, value));
                if (_zoom != coercedZoom)
                {
                    _zoom = value;
                    NotifyPropertyChanged("Zoom");
                }
            }
        }

        /// <summary>
        /// Gets or sets the map center location coordinate.
        /// </summary>
        public GeoCoordinate Center
        {
            get { return _center; }
            set
            {
                if (_center != value)
                {
                    _center = value;
                    NotifyPropertyChanged("Center");
                }
            }
        }

        /// <summary>
        /// Gets a collection of pushpins.
        /// </summary>
        public ObservableCollection<PushpinModel> Pushpins
        {
            get { return _pushpins; }
        }

        /// <summary>
        /// Gets a collection of routes.
        /// </summary>
        public ObservableCollection<RouteModel> Routes
        {
            get { return _routes; }
        }

        /// <summary>
        /// Gets a collection of route itineraries.
        /// </summary>
        public ObservableCollection<ItineraryItem> Itineraries
        {
            get { return _itineraries; }
        }

        /// <summary>
        /// To: Name of GuiaTBAWP
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Current And To: Location in lat long
        /// </summary>
        protected GeoCoordinate CurrentLocation { get; set; }
        protected GeoCoordinate ToLocation { get; set; }

        GeoCoordinateWatcher watcher;

        #endregion

        private void InitializeDefaults()
        {
            CurrentLocation = DefaultLocation;
            ToLocation = DefaultLocation;
            Zoom = DefaultZoomLevel;
            Center = DefaultLocation;
            To = "Local Mas Cercano";
            Pushpins.Add(DefaultPushPin);
        }

        private void SwapMapMode()
        {
            if (Map.Mode is AerialMode)
            {
                Map.Mode = new RoadMode();
            }
            else
            {
                Map.Mode = new AerialMode(true);
            }
        }

        private void SetLocation(GeoCoordinate location)
        {
            // Center map to default location.
            Center = location;

            // Reset zoom default level.
            Zoom = DefaultZoomLevel;
        }

        private void CenterPushpinsPopup(Point touchPoint)
        {
            // Reposition the pushpins popup to the center of the touch point.
            Canvas.SetTop(PushpinPopup, touchPoint.Y - ListBoxPushpinCatalog.Height / 2);
        }

        private void CreateNewPushpin(Point point)
        {
            // Translate the map viewport touch point to a geo coordinate.
            GeoCoordinate location;
            Map.TryViewportPointToLocation(point, out location);
            CreateNewPushpin(location);
        }

        private void CreateNewPushpin(GeoCoordinate location)
        {
            var pushpin = DefaultPushPin.Clone(location);
            Pushpins.Add(pushpin);
        }

        private void CreateNewPushpin(object selectedItem, Point point)
        {
            // Use the geo coordinate calculated to add a new pushpin,
            // based on the selected pushpin prototype,
            var pushpinPrototype = selectedItem as PushpinModel;
            if (pushpinPrototype == null) return;

            CreateNewPushpin(point);
        }

        private void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pushpin = sender as Pushpin;

            // Center the map on a pushpin when touched.
            if (pushpin != null) Center = pushpin.Location;
        }

        //TODO: Extract to class
        private void CalculateRoute()
        {
            try
            {
                var routeCalculator = new RouteCalculator(
                    CredentialsProvider,
                    //To,
                    TextConverter.ToLocationString(ToLocation),
                    TextConverter.ToLocationString(CurrentLocation),
                    Dispatcher,
                    result =>
                    {
                        // Clear the route collection to have only one route at a time.
                        Routes.Clear();

                        // Clear previous route related itineraries.
                        Itineraries.Clear();

                        // Create a new route based on route calculator result,
                        // and add the new route to the route collection.
                        var routeModel = new RouteModel(result.Result.RoutePath.Points);
                        Routes.Add(routeModel);

                        // Add new route itineraries to the itineraries collection.
                        foreach (var itineraryItem in result.Result.Legs[0].Itinerary)
                        {
                            itineraryItem.Summary.Distance = NumberConverter.SetSigFigs(itineraryItem.Summary.Distance * 1.609344, 2);
                            Itineraries.Add(itineraryItem);
                        }

                        // Set the map to center on the new route.
                        var viewRect = LocationRect.CreateLocationRect(routeModel.Locations);
                        Map.SetView(viewRect);

                        ShowDirectionsView();
                    });

                // Display an error message in case of fault.
                routeCalculator.Error += r => MessageBox.Show(r.Reason);

                // Start the route calculation asynchronously.
                routeCalculator.CalculateAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //TODO: Extract to class
        #region Location Service

        /// <summary>
        /// Helper method to start up the location data acquisition
        /// </summary>
        /// <param name="accuracy">The accuracy level </param>
        private void SetLocationService(GeoPositionAccuracy accuracy)
        {
            // Reinitialize the GeoCoordinateWatcher
            //StatusTextBlock.Text = "starting, " + accuracyText;
            watcher = new GeoCoordinateWatcher(accuracy) {MovementThreshold = 20};

            // Add event handlers for StatusChanged and PositionChanged events
            watcher.StatusChanged += watcher_StatusChanged;
            watcher.PositionChanged += watcher_PositionChanged;
        }

        /// <summary>
        /// Helper method to start up the location data acquisition
        /// </summary>
        private void StartLocationService()
        {
            // Start data acquisition
            watcher.Start();
        }

        /// <summary>
        /// Helper method to stop up the location data acquisition
        /// </summary>
        private void StopLocationService()
        {
            // Start data acquisition
            watcher.Stop();
        }

        /// <summary>
        /// Handler for the StatusChanged event. This invokes MyStatusChanged on the UI thread and
        /// passes the GeoPositionStatusChangedEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(() => MyStatusChanged(e));

        }

        /// <summary>
        /// Handler for the PositionChanged event. This invokes MyStatusChanged on the UI thread and
        /// passes the GeoPositionStatusChangedEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyPositionChanged(e));
        }

        /// <summary>
        /// Custom method called from the PositionChanged event handler
        /// </summary>
        /// <param name="e"></param>
        void MyPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var location = new GeocodeLocation
            {
                Latitude = e.Position.Location.Latitude,
                Longitude = e.Position.Location.Longitude
            };
            
            if (location.Latitude == CurrentLocation.Latitude && location.Longitude == CurrentLocation.Longitude)
            {
                return;
            }

            CurrentLocation = e.Position.Location;
            
            SetLocation(location);
            CreateNewPushpin(location);
            StopLocationService();
        }

        #endregion
    }
}
