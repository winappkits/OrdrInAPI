using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using APIMASH_OrdrInLib;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class RestaurantPage : PhoneApplicationPage
    {
        public RestaurantPage()
        {
            InitializeComponent();
            this.Loaded += RestaurantPage_Loaded;
            restMap.Center = Globals.Instance.ordrin.position;
            restMap.ZoomLevel = 15;
        }

        void CurrentRestaurant_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                if (Globals.Instance.ordrin.CurrentRestaurant.Position != null)
                    restMap.Center = Globals.Instance.ordrin.CurrentRestaurant.Position;
            }
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Globals.Instance.ordrin.OnError -= ordrin_OnError;
            if (Globals.Instance.ordrin.CurrentRestaurant != null)
                Globals.Instance.ordrin.CurrentRestaurant.PropertyChanged -= CurrentRestaurant_PropertyChanged;
            base.OnNavigatingFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int restaurantId = 0;
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("id", out selectedIndex))
            {
                restaurantId = int.Parse(selectedIndex);
            }
            string decideIndex = "";
            if (NavigationContext.QueryString.TryGetValue("decide", out decideIndex) && e.NavigationMode == NavigationMode.New)
            {
                NavigationService.RemoveBackEntry();
            }
            MenuList.SelectedItem = null;

            Globals.Instance.ordrin.LoadRestaurant(restaurantId);
            Globals.Instance.ordrin.OnError += ordrin_OnError;

            base.OnNavigatedTo(e);
        }

        private void ordrin_OnError(object sender, ErrorEventArgs e)
        {
            switch (e.Code)
            {
                case ErrorCode.RestaurantLoaded:
                    Globals.Instance.ordrin.CurrentRestaurant.PropertyChanged += CurrentRestaurant_PropertyChanged;
                    break;
                default:
                    break;
            }
        }

        void RestaurantPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Globals.Instance.ordrin;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuButton.IsChecked.Value)
            {
                DetailButton.IsChecked = false;
                MenuButton.Style = Application.Current.Resources["SelectedToggleStyle2"] as Style;
                DetailButton.Style = Application.Current.Resources["UnselectedToggleStyle2"] as Style;
                DetailCanvas.Visibility = System.Windows.Visibility.Collapsed;
                MenuList.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailButton.IsChecked.Value)
            {
                MenuButton.IsChecked = false;
                DetailButton.Style = Application.Current.Resources["SelectedToggleStyle2"] as Style;
                MenuButton.Style = Application.Current.Resources["UnselectedToggleStyle2"] as Style;
                DetailCanvas.Visibility = System.Windows.Visibility.Visible;
                MenuList.Visibility = System.Windows.Visibility.Collapsed;
                if(Globals.Instance.ordrin.CurrentRestaurant.Position != null)
                    restMap.Center = Globals.Instance.ordrin.CurrentRestaurant.Position;
            }
        }

        private void HyperlinkButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Microsoft.Phone.Tasks.PhoneCallTask task = new Microsoft.Phone.Tasks.PhoneCallTask();
            task.DisplayName = Globals.Instance.ordrin.CurrentRestaurant.Name;
            task.PhoneNumber = Globals.Instance.ordrin.CurrentRestaurant.Phone;
            task.Show();
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list.SelectedItem != null)
            {
                var menuitem = ((Menu_Bindable)list.SelectedItem);
                if (menuitem.IsOrderable == false)
                {
                    Globals.Instance.ordrin.CurrentMenuItem = menuitem;
                    NavigationService.Navigate(new Uri("/SubMenuPage.xaml", UriKind.Relative));
                }
                else
                {
                    Globals.Instance.ordrin.CurrentItem = menuitem;
                    NavigationService.Navigate(new Uri("/MenuItemPage.xaml", UriKind.Relative));
                }
            }
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Globals.Instance.ordrin.Tray.Count > 0)
                NavigationService.Navigate(new Uri("/TotalPage.xaml", UriKind.Relative));
            else
            {
                string company = Application.Current.Resources["AppName"] as string;
                MessageBox.Show("Please add something to the cart first before checking out.", company, MessageBoxButton.OK);
            }
        }
    }
}