using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP
{
    public partial class PerdidaRoboDanio : PhoneApplicationPage
    {
        public PerdidaRoboDanio()
        {
            InitializeComponent();
        }

        private void SUBETel_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask();
            task.PhoneNumber = "0800-777-7823";
            task.Show();
        }
    }
}