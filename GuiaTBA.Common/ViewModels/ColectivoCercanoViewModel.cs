﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GuiaTBAWP.Commons.ViewModels
{
    public class ColectivoCercanoViewModel : INotifyPropertyChanged
    {
        public ColectivoCercanoViewModel()
        {
            Items = new ObservableCollection<ColectivoItemViewModel>();
        }

        private ObservableCollection<ColectivoItemViewModel> _items;
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