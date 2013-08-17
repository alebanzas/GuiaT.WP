using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using GuiaTBAWP.Models;

namespace GuiaTBAWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel(string bingMapsApiKey)
        {
            PuntosVenta = new ObservableCollection<ItemViewModel>();
            PuntosRecarga = new ObservableCollection<ItemViewModel>();
            _credentialsProvider = new ApplicationIdCredentialsProvider(bingMapsApiKey);
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
            Icon = new Uri("/Resources/Icons/Pushpins/PushpinLocation.png", UriKind.Relative),
            TypeName = "PushpinFuel",
        };
        
        public PushpinModel RecargaPushPin = new PushpinModel
        {
            Icon = new Uri("/Resources/Icons/Pushpins/PushpinFuel.png", UriKind.Relative),
            TypeName = "PushpinShop",
        };

        public PushpinModel VentaPushPin = new PushpinModel
        {
            Icon = new Uri("/Resources/Icons/Pushpins/PushpinShop.png", UriKind.Relative),
            TypeName = "PushpinHouse",
        };

        #endregion

        #region Fields

        /// <value>Provides credentials for the map control.</value>
        private readonly CredentialsProvider _credentialsProvider;
        
        private readonly ObservableCollection<PushpinModel> _pushpins = new ObservableCollection<PushpinModel>();
        
        /// <value>Map zoom level.</value>
        private double _zoom;

        /// <value>Map center coordinate.</value>
        private GeoCoordinate _center;

        private GeoCoordinate _currentPosition;

        #endregion

        #region Properties
        
        public CredentialsProvider CredentialsProvider
        {
            get { return _credentialsProvider; }
        }

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

        public GeoCoordinate CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                CreateNewPushpin(value);
                _currentPosition = value;
            }
        }

        public ObservableCollection<PushpinModel> Pushpins
        {
            get { return _pushpins; }
        }

        #endregion
        
        private void InitializeDefaults()
        {
            Zoom = DefaultZoomLevel;
        }

        public ObservableCollection<ItemViewModel> PuntosVenta { get; private set; }

        public ObservableCollection<ItemViewModel> PuntosRecarga { get; private set; }
        
        public void LoadPuntosRecarga(IEnumerable<ItemViewModel> items)
        {
            Pushpins.Clear();
            PuntosRecarga.Clear();
            var i = 1;
            foreach (var itemViewModel in items)
            {
                itemViewModel.Index = i++;
                CreateNewRecargaPushpin(itemViewModel);
                PuntosRecarga.Add(itemViewModel);
            }
            CreateNewPushpin(CurrentPosition);
        }

        public void LoadPuntosVenta(IEnumerable<ItemViewModel> items)
        {
            Pushpins.Clear();
            PuntosVenta.Clear();
            var i = 1;
            foreach (var itemViewModel in items)
            {
                itemViewModel.Index = i++;
                CreateNewVentaPushpin(itemViewModel);
                PuntosVenta.Add(itemViewModel);
            }
            CreateNewPushpin(CurrentPosition);
        }

        public void CreateNewPushpin(GeoCoordinate location)
        {
            var pushpin = DefaultPushPin.Clone(location);
            CreateNewPushpin(pushpin);
        }

        public void CreateNewRecargaPushpin(ItemViewModel itemViewModel)
        {
            var pushpin = RecargaPushPin.Clone(itemViewModel.Punto);
            pushpin.Title = itemViewModel.Titulo;
            pushpin.Index = itemViewModel.Index;
            CreateNewPushpin(pushpin);
        }

        public void CreateNewVentaPushpin(ItemViewModel itemViewModel)
        {
            var pushpin = VentaPushPin.Clone(itemViewModel.Punto);
            pushpin.Title = itemViewModel.Titulo;
            pushpin.Index = itemViewModel.Index;
            CreateNewPushpin(pushpin);
        }

        private void CreateNewPushpin(PushpinModel pushpin)
        {
            Pushpins.Add(pushpin);
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