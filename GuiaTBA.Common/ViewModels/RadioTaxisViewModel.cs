using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GuiaTBAWP.Commons.ViewModels
{
    public class RadioTaxisViewModel : INotifyPropertyChanged
    {
        public RadioTaxisViewModel()
        {
            Lista = new ObservableCollection<RadioTaxisItemViewModel>();
        }

        private ObservableCollection<RadioTaxisItemViewModel> _lista;
        public ObservableCollection<RadioTaxisItemViewModel> Lista
        {
            get { return _lista; }
            private set
            {
                _lista = value;
                NotifyPropertyChanged("Lista");
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