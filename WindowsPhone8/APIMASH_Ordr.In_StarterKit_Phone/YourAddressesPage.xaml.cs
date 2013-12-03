using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using APIMASH_OrdrInLib;
using System.IO.IsolatedStorage;
using System.IO;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class YourAddressesPage : PhoneApplicationPage
    {
        Address_Bindable selectedAddress=null;

        public YourAddressesPage()
        {
            InitializeComponent();

            this.DataContext = Globals.Instance.ordrin;

            Globals.Instance.ordrin.Addresses.CollectionChanged += Addresses_CollectionChanged;

            this.Loaded += YourAddressesPage_Loaded;
        
        }

        void YourAddressesPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        void Addresses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        private void addressList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var addrs = Globals.Instance.ordrin.Addresses;
            foreach (var item in addrs)
            {
                item.IsSelected = false;
            }
            ApplicationBarIconButton editAddressButton = this.ApplicationBar.Buttons[3] as ApplicationBarIconButton;
            ApplicationBarIconButton deleteAddressButton = this.ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            ApplicationBarIconButton selectAddressButton = this.ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (e.AddedItems.Count > 0)
            {
                selectedAddress = e.AddedItems[0] as Address_Bindable;
                selectedAddress.IsSelected = true;
                deleteAddressButton.IsEnabled = true;
                selectAddressButton.IsEnabled = true;
                editAddressButton.IsEnabled = true;
            }
            else
            {
                deleteAddressButton.IsEnabled = false;
                selectAddressButton.IsEnabled = false;
                editAddressButton.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddressPage.xaml", UriKind.Relative));
        }

        private void AddAddress_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddressPage.xaml", UriKind.Relative));
        }

        private void selectAddress_Click(object sender, EventArgs e)
        {
            var addrs = Globals.Instance.ordrin.Addresses;
            foreach (var item in addrs)
            {
                item.IsDefault = 0;
            }
            selectedAddress.IsDefault = 1;
            Globals.Instance.ordrin.CurrentAddress = selectedAddress;
            SaveDefaultAddress();
            Globals.Instance.ordrin.LoadDeliveryList(selectedAddress.Zip, selectedAddress.City, selectedAddress.Address, "ASAP");
        }

        private void SaveDefaultAddress()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("address.default", FileMode.Create))
                {
                    byte[] buffer = System.Text.Encoding.Unicode.GetBytes(Globals.Instance.ordrin.CurrentAddress.Nick);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private void deleteAddress_Click(object sender, EventArgs e)
        {
            string nick = selectedAddress.Nick;
            Globals.Instance.ordrin.DeleteAddress(nick);
        }

        private void editAddress_Click(object sender, EventArgs e)
        {
            string nick = selectedAddress.Nick;
            NavigationService.Navigate(new Uri("/AddressPage.xaml?nick="+nick, UriKind.Relative));
        }
    }
}