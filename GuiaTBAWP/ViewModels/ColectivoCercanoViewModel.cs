using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GuiaTBAWP.ViewModels
{
    public class ColectivoCercanoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ColectivoItemViewModel> _items;

        public ColectivoCercanoViewModel()
        {
            Items = new ObservableCollection<ColectivoItemViewModel>();
        }

        public ObservableCollection<ColectivoItemViewModel> Items
        {
            get { return _items; }
            private set { 
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        public void AddLinea(ColectivoItemViewModel linea)
        {
            Items.Add(linea);
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