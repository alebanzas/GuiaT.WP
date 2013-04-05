using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GuiaTBAWP.Views.Subtes;

namespace GuiaTBAWP.ViewModels
{
    public class SubteStatusViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SubteItemViewModel> _lineas;

        public SubteStatusViewModel()
        {
            Lineas = new ObservableCollection<SubteItemViewModel>();
        }

        public ObservableCollection<SubteItemViewModel> Lineas
        {
            get { return _lineas; }
            private set { 
                _lineas = value;
                NotifyPropertyChanged("SampleProperty");
            }
        }

        public void AddLinea(SubteItemViewModel linea)
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
}