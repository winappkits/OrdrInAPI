using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class DecidePage : PhoneApplicationPage
    {
        public DecidePage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;

            Globals.Instance.ordrin.CurrentRestaurant = null;

            SelectDelivery();
        }

        public void SelectDelivery()
        {
            var list = Globals.Instance.ordrin.Deliveries;
            if (list.Count > 0)
            {
                var rnd = new Random();
                int selected = rnd.Next(0, list.Count);
                Globals.Instance.ordrin.LoadRestaurant(Globals.Instance.ordrin.Deliveries[selected].Id);
            }
        }

        private void decideButton_Click(object sender, RoutedEventArgs e)
        {
            SelectDelivery();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RestaurantPage.xaml?decide=1&id=" + Globals.Instance.ordrin.CurrentRestaurant.Id, UriKind.Relative));
        }
    }
}