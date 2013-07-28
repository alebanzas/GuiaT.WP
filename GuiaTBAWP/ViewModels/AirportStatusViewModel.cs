using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GuiaTBAWP.ViewModels
{
    public class AirportStatusViewModel : INotifyPropertyChanged
    {
        public AirportStatusViewModel()
        {
            Vuelos = new ObservableCollection<AirportStatusItemViewModel>();
        }

        private string _actualizacion;
        public string Actualizacion
        {
            get { return _actualizacion; }
            set
            {
                _actualizacion = value;
                NotifyPropertyChanged("Actualizacion");
            }
        }

        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value.ToLowerInvariant();
                NotifyPropertyChanged("Titulo");
            }
        }

        private string _aeroestacion;
        public string Aeroestacion
        {
            get { return _aeroestacion; }
            set
            {
                _aeroestacion = value.ToUpperInvariant();
                NotifyPropertyChanged("Aeroestacion");
            }
        }

        private ObservableCollection<AirportStatusItemViewModel> _vuelos;
        public ObservableCollection<AirportStatusItemViewModel> Vuelos
        {
            get { return _vuelos; }
            private set { 
                _vuelos = value;
                NotifyPropertyChanged("Vuelos");
            }
        }

        public string NickName { get; set; }

        public string Tipo { get; set; }

        public void AddVuelo(AirportStatusItemViewModel linea)
        {
            Vuelos.Add(linea);
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