using System;
using System.ComponentModel;
using System.Device.Location;

namespace GuiaTBAWP.Commons.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _titulo;
        public string Titulo
        {
            get
            {
                return string.Format("{0} - {1}", Index, _titulo);
            }
            set
            {
                if (value != _titulo)
                {
                    _titulo = value;
                    NotifyPropertyChanged("Titulo");
                }
            }
        }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (value != _index)
                {
                    _index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

        private GeoCoordinate _punto;
        public GeoCoordinate Punto
        {
            get
            {
                return _punto;
            }
            set
            {
                if (value != _punto)
                {
                    _punto = value;
                    NotifyPropertyChanged("Punto");
                }
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