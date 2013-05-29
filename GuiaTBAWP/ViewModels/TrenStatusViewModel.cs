using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GuiaTBAWP.ViewModels
{
    public class TrenStatusViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TrenLineaItemViewModel> _lineas;

        public TrenStatusViewModel()
        {
            Lineas = new ObservableCollection<TrenLineaItemViewModel>();
        }

        public ObservableCollection<TrenLineaItemViewModel> Lineas
        {
            get { return _lineas; }
            private set { 
                _lineas = value;
                NotifyPropertyChanged("Lineas");
            }
        }

        public void AddLinea(TrenLineaItemViewModel linea)
        {
            Lineas.Add(linea);
        }

        public string Actualizacion { get; set; }

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

    public class TrenLineaItemViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TrenRamalItemViewModel> _ramales;

        public TrenLineaItemViewModel()
        {
            Ramales = new ObservableCollection<TrenRamalItemViewModel>();
        }

        public ObservableCollection<TrenRamalItemViewModel> Ramales
        {
            get { return _ramales; }
            private set
            {
                _ramales = value;
                NotifyPropertyChanged("Ramales");
            }
        }

        private string _nombre;
        public string Nombre {
            get { return _nombre; }
            set
            {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
            }
        }

        private string _estado;
        public string Estado
        {
            get { return _estado; }
            set
            {
                _estado = value;
                NotifyPropertyChanged("Estado");
            }
        }

        private DateTime _actualizacion;
        public DateTime Actualizacion
        {
            get { return _actualizacion; }
            set
            {
                _actualizacion = value;
                NotifyPropertyChanged("Actualizacion");
            }
        }


        public void AddRamal(TrenRamalItemViewModel linea)
        {
            Ramales.Add(linea);
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