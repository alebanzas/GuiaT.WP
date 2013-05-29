using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using GuiaTBAWP.Bing.Route;
using GuiaTBAWP.Models;

namespace GuiaTBAWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.PuntosVenta = new ObservableCollection<ItemViewModel>();
            this.PuntosRecarga = new ObservableCollection<ItemViewModel>();
            InitializeDefaults();
        }


        #region Consts

        /// <value>Default map zoom level.</value>
        public double DefaultZoomLevel = 15.0;

        /// <value>Maximum map zoom level allowed.</value>
        public double MaxZoomLevel = 21.0;

        /// <value>Minimum map zoom level allowed.</value>
        public double MinZoomLevel = 1.0;

        public PushpinModel DefaultPushPin = new PushpinModel
        {
            Location = DefaultLocation,
            Icon = new Uri("/Resources/Icons/Pushpins/PushpinLocation.png", UriKind.Relative),
            TypeName = "PushpinFuel",
        };
        
        public PushpinModel RecargaPushPin = new PushpinModel
        {
            Location = DefaultLocation,
            Icon = new Uri("/Resources/Icons/Pushpins/PushpinFuel.png", UriKind.Relative),
            TypeName = "PushpinShop",
        };

        public PushpinModel VentaPushPin = new PushpinModel
        {
            Location = DefaultLocation,
            Icon = new Uri("/Resources/Icons/Pushpins/PushpinShop.png", UriKind.Relative),
            TypeName = "PushpinHouse",
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
        /// To: Name of Hotel
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// To: Name of Hotel
        /// </summary>
        public string ToItinerary { get; set; }

        /// <summary>
        /// Current And To: Location in lat long
        /// </summary>
        public GeoCoordinate CurrentLocation { get; set; }
        public GeoCoordinate ToLocation { get; set; }

        public GeoCoordinateWatcher watcher;

        #endregion
        
        private void InitializeDefaults()
        {
            CurrentLocation = DefaultLocation;
            ToLocation = DefaultLocation;
            Zoom = DefaultZoomLevel;
            Center = DefaultLocation;
            To = "Buenos Aires, Argentina";
            //Pushpins.Add(DefaultPushPin);
        }



        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> PuntosVenta { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> PuntosRecarga { get; private set; }
        
        public bool IsPuntosRecargaLoaded
        {
            get;
            private set;
        }

        public bool IsPuntosVentaLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the PuntosRecarga collection.
        /// </summary>
        public void LoadPuntosRecarga(IEnumerable<ItemViewModel> items)
        {
            this.PuntosRecarga.Clear();
            foreach (var itemViewModel in items)
            {
                this.PuntosRecarga.Add(itemViewModel);
            }
            this.IsPuntosRecargaLoaded = true;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the PuntosVenta collection.
        /// </summary>
        public void LoadPuntosVenta(IEnumerable<ItemViewModel> items)
        {
            this.PuntosVenta.Clear();
            foreach (var itemViewModel in items)
            {
                this.PuntosVenta.Add(itemViewModel);
            }
            this.IsPuntosVentaLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}