using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views.Taxis
{
    public partial class RadioTaxiDetalles : PhoneApplicationPage
    {
        RadioTaxiTable _radioTaxi;
        
        // Constructor
        public RadioTaxiDetalles()
        {
            InitializeComponent();
            Unloaded += Page_UnLoaded;
        }

        private void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateLugar()
        {
            PageTitle.Text = _radioTaxi.Nombre;
            Detalles.Text = _radioTaxi.Detalles;
            Telefono.Text = _radioTaxi.Telefono;
            Url.Text = _radioTaxi.Url;

            
            var applicationBarIconButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = !string.IsNullOrWhiteSpace(_radioTaxi.Url);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Al navegar a la página, busco el lugar en base al id pasado y luego lo muestro.
            var id = Guid.Parse(Uri.EscapeUriString(NavigationContext.QueryString["id"]));
            var query = from miLugar in RadioTaxiDC.Current.Lista
                        where miLugar.Id == id
                        select miLugar;

            _radioTaxi = query.FirstOrDefault();
            UpdateLugar();

            base.OnNavigatedTo(e);
        }
        
        private void Pin(object sender, EventArgs e)
        {
            TileManager.Set(new Uri(string.Format("/Views/Taxis/RadioTaxiDetalles.xaml?id={0}", _radioTaxi.Id), UriKind.Relative), _radioTaxi.Nombre, new Uri("/Images/Home/taxis.png", UriKind.Relative));
        }
        
        private void Share(object sender, EventArgs e)
        {
            ShareTaskBase shareTask;
            if (string.IsNullOrWhiteSpace(_radioTaxi.Url))
            {
                shareTask = new ShareStatusTask
                {
                    Status = string.Format("Radio Taxi {0}. Teléfono: {1}", _radioTaxi.Nombre, _radioTaxi.Telefono),
                };
            }
            else
            {
                shareTask = new ShareLinkTask
                {
                    Title = "Radio Taxi " + _radioTaxi.Nombre,
                    Message = string.Format("Teléfono: {0}, Web: {1}", _radioTaxi.Telefono, _radioTaxi.Url),
                    LinkUri = new Uri(_radioTaxi.Url, UriKind.Absolute)
                };    
            }
            
            shareTask.Show();
        }
        
        private void Browse(object sender, EventArgs e)
        {
            var webBrowserTask = new WebBrowserTask
            {
                Uri = new Uri(_radioTaxi.Url, UriKind.Absolute)
            };
            webBrowserTask.Show();
        }

        private void Call(object sender, EventArgs e)
        {
            var task = new PhoneCallTask
            {
                DisplayName = _radioTaxi.Nombre, 
                PhoneNumber = _radioTaxi.Telefono
            };
            task.Show();
        }

        private void SaveContact(object sender, EventArgs e)
        {
            var saveContactTask = new SaveContactTask();
            //saveContactTask.Completed += saveContactTask_Completed;

            saveContactTask.FirstName = "Radio Taxi";
            saveContactTask.LastName = _radioTaxi.Nombre;
            saveContactTask.MobilePhone = _radioTaxi.Telefono;
            if(!string.IsNullOrWhiteSpace(_radioTaxi.Url))
                saveContactTask.Website = _radioTaxi.Url;

            saveContactTask.Show();
        }
        
        //void saveContactTask_Completed(object sender, SaveContactResult e)
        //{
        //    switch (e.TaskResult)
        //    {
        //        //Logic for when the contact was saved successfully
        //        case TaskResult.OK:
        //            MessageBox.Show("Contact saved.");
        //            break;
        //
        //        //Logic for when the task was cancelled by the user
        //        case TaskResult.Cancel:
        //            MessageBox.Show("Save cancelled.");
        //            break;
        //
        //        //Logic for when the contact could not be saved
        //        case TaskResult.None:
        //            MessageBox.Show("Contact could not be saved.");
        //            break;
        //    }
        //}
    }
}