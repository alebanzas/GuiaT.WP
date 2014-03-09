using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using GuiaTBAWP.Commons.ViewModels;

namespace GuiaTBA.Common.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel(GeoCoordinate center)
        {
            Puntos = new ObservableCollection<ItemViewModel>();
            Center = center;
            InitializeDefaults();
        }


        #region Consts

        /// <value>Default map zoom level.</value>
        public double DefaultZoomLevel = 15.0;

        /// <value>Maximum map zoom level allowed.</value>
        public double MaxZoomLevel = 21.0;

        /// <value>Minimum map zoom level allowed.</value>
        public double MinZoomLevel = 1.0;
        
        #endregion

        #region Fields

        /// <value>Map zoom level.</value>
        private double _zoom;

        /// <value>Map center coordinate.</value>
        private GeoCoordinate _center;

        #endregion

        #region Properties
        
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
        
        #endregion
        
        private void InitializeDefaults()
        {
            Zoom = DefaultZoomLevel;
        }

        public ObservableCollection<ItemViewModel> Puntos { get; private set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }

        public void LoadPuntos(IEnumerable<ItemViewModel> items)
        {
            Puntos.Clear();
            var i = 1;
            foreach (var itemViewModel in items)
            {
                itemViewModel.Index = i++;
                Puntos.Add(itemViewModel);
            }
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