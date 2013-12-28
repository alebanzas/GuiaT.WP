using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GuiaTBAWP.Commons.Extensions;

namespace GuiaTBAWP.ViewModels
{
    public class AirportStatusViewModel : INotifyPropertyChanged
    {
        public AirportStatusViewModel()
        {
            Vuelos = new ObservableCollection<AirportStatusItemViewModel>();
            VuelosFiltrados = new ObservableCollection<AirportStatusItemViewModel>();
            EmptyResults = true;
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
            private set
            {
                _vuelos = value;
                NotifyPropertyChanged("Vuelos");
            }
        }


        private ObservableCollection<AirportStatusItemViewModel> _vuelosFiltrados;
        private bool _emptyResults;

        public ObservableCollection<AirportStatusItemViewModel> VuelosFiltrados
        {
            get { return _vuelosFiltrados; }
            private set
            {
                _vuelosFiltrados = value;
                NotifyPropertyChanged("VuelosFiltrados");
            }
        }

        public string NickName { get; set; }

        public string Tipo { get; set; }

        public bool EmptyResults
        {
            get { return _emptyResults; }
            set
            {
                _emptyResults = value;
                NotifyPropertyChanged("EmptyResults");
            }
        }

        public void AddVuelo(AirportStatusItemViewModel linea)
        {
            Vuelos.Add(linea);
            VuelosFiltrados.Add(linea);
            EmptyResults = false;
        }

        public void FiltrarVuelos(string pattern)
        {
            VuelosFiltrados = string.IsNullOrWhiteSpace(pattern) ? 
                                        new ObservableCollection<AirportStatusItemViewModel>(Vuelos.ToList()) :
                                        new ObservableCollection<AirportStatusItemViewModel>(Vuelos.Where(x => x.Ciudad.Sanitize().Contains(pattern.Sanitize())).ToList());

            EmptyResults = !VuelosFiltrados.Any();
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