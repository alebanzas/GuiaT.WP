using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views
{
    public partial class Comments : PhoneApplicationPage
    {
        public string RegisterURL = "http://servicio.abhosting.com.ar/report/error/?type=WP&version=" + App.Configuration.Version;
        public ErrorReportRequestModel RequestModel { get; set; }

        public Comments()
        {
            InitializeComponent();
        }

        public static Exception Exception;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //el back vuelve a la home
            while (NavigationService.BackStack.Count() > 1)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void GoToHome_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Por favor, vuelva a iniciar la aplicación manualmente."));
            }
        }
        
        private void StartRequest()
        {
            Dispatcher.BeginInvoke(() =>
            {
                BtnSend.IsEnabled = false;
            });
        }

        private void EndRequest()
        {
            Dispatcher.BeginInvoke(() =>
            {
                BtnSend.IsEnabled = true;
                DvError.Visibility = Visibility.Collapsed;
                DvThanks.Visibility = Visibility.Visible;
            });
        }

        private void SendErrorReport_OnClick(object sender, RoutedEventArgs e)
        {
            StartRequest();

            RequestModel = new ErrorReportRequestModel
            {
                Date = DateTime.UtcNow,
                InstallationId = App.Configuration.InstallationId.ToString(),
                ErrorDetail = Exception.ToString(),
                UserMessage = TxtComentario.Text,
            };

            var webRequest = WebRequest.CreateHttp(RegisterURL);
            webRequest.Method = "POST";
            webRequest.BeginGetRequestStream(GetRequestStreamCallback, webRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            var postStream = webRequest.EndGetRequestStream(asynchronousResult);

            string postData = GetReportData();

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            webRequest.ContentType = "application/json, charset=utf-8";
            webRequest.BeginGetResponse(ResponseCallback, webRequest);
        }

        private void ResponseCallback(IAsyncResult asyncResult)
        {
            var httpRequest = (HttpWebRequest)asyncResult.AsyncState;
            var response = (HttpWebResponse)httpRequest.EndGetResponse(asyncResult);
            
            try
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        TxtErrorCode.Text = "ID reporte: " + response.StatusDescription;
                    });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        TxtErrorCode.Text = "ID reporte: " + Guid.Empty;
                    });
            }
            finally
            {
                EndRequest();   
            }
        }


        private string GetReportData()
        {
            var ms = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(ErrorReportRequestModel));
            ser.WriteObject(ms, RequestModel);
            var json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}