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
    public partial class SubMenuPage : PhoneApplicationPage
    {
        public SubMenuPage()
        {
            InitializeComponent();

            this.DataContext = Globals.Instance.ordrin;
            this.Loaded += SubMenuPage_Loaded;

//            restMap.Center = Globals.Instance.ordrin.position;
//            restMap.ZoomLevel = 15;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MenuList.SelectedItem = null;
            base.OnNavigatedTo(e);
        }

        void SubMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            MenuList.Height = this.LayoutRoot.RowDefinitions[1].ActualHeight - 25;
            //            MenuList.ItemsSource = .Children;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list.SelectedItem != null)
            {
                var menuitem = ((Menu_Bindable)list.SelectedItem);
                if (menuitem.IsOrderable == false)
                    MenuList.ItemsSource = menuitem.Children;
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