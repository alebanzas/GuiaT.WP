using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP
{
    public partial class GeneralError : PhoneApplicationPage
    {
        public GeneralError()
        {
            InitializeComponent();
        }

        public static Exception Exception;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ErrorText.Text = Exception.ToString();
        }
    }
}