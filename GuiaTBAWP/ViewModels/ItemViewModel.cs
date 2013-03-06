using System;
using System.ComponentModel;

namespace GuiaTBAWP.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _titulo;
        public string Titulo
        {
            get
            {
                return _titulo;
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