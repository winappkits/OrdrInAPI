using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class AddressPage : PhoneApplicationPage
    {
        public AddressPage()
        {
            InitializeComponent();
            this.Loaded += AddressPage_Loaded;
            this.Unloaded += AddressPage_Unloaded;
        }

        void AddressPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.Instance.ordrin.OnError -= ordrin_OnError;
        }

        void AddressPage_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.Instance.ordrin.OnError += ordrin_OnError;

            string nick = "";
            if (NavigationContext.QueryString.TryGetValue("nick", out nick))
            {
                pageTitle.Text = "Edit Address";
                var address = Globals.Instance.ordrin.Addresses.FirstOrDefault(a => a.Nick == nick);
                if (address != null)
                {
                    nickText.Text = address.Nick;
                    addressText.Text = address.Address;
                    address2Text.Text = address.Address2;
                    cityText.Text = address.City;
                    stateText.Text = address.State;
                    zipText.Text = address.Zip;
                    phoneText.Text = address.Phone;
                }
            }

        }

        private void ordrin_OnError(object sender, APIMASH_OrdrInLib.ErrorEventArgs e)
        {
            switch (e.Code)
            {
                case APIMASH_OrdrInLib.ErrorCode.AddressSaveError:
                    string company = Application.Current.Resources["AppName"] as string;
                    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                    break;
                case APIMASH_OrdrInLib.ErrorCode.AddressSaved:
                    NavigationService.GoBack();
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;
            if (nickText.Text == null || nickText.Text == "")
            {
                MessageBox.Show("The address entered needs a name.  Please enter a name and submit it again.", company, MessageBoxButton.OK);
                return;
            }
            Globals.Instance.ordrin.AddAddress(nickText.Text, addressText.Text, address2Text.Text, cityText.Text, stateText.Text, zipText.Text, phoneText.Text);
        }

        private void nickText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // if the enter key is pressed
            if (e.Key == Key.Enter)
            {
                // focus the page in order to remove focus from the text box
                // and hide the soft keyboard
                this.Focus();
            }
        }
    }
}