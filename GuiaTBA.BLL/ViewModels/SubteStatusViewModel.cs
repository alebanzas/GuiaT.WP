using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GuiaTBA.BLL.ViewModels
{
    public class SubteStatusViewModel : INotifyPropertyChanged
    {
        public SubteStatusViewModel()
        {
            Lineas = new ObservableCollection<SubteItemViewModel>();
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

        private ObservableCollection<SubteItemViewModel> _lineas;
        public ObservableCollection<SubteItemViewModel> Lineas
        {
            get { return _lineas; }
            private set { 
                _lineas = value;
                NotifyPropertyChanged("Lineas");
            }
        }

        public void AddLinea(SubteItemViewModel linea)
        {
            Lineas.Add(linea);
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