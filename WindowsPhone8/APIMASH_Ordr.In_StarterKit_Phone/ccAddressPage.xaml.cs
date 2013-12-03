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
    public partial class ccAddressPage : PhoneApplicationPage
    {
        public ccAddressPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string company = Application.Current.Resources["AppName"] as string;
            //if (nickText.Text == null || nickText.Text == "")
            //{
            //    MessageBox.Show("The address entered needs a name.  Please enter a name and submit it again.", company, MessageBoxButton.OK);
            //    return;
            //}
//            Globals.Instance.ordrin.AddAddress(nickText.Text, addressText.Text, address2Text.Text, cityText.Text, stateText.Text, zipText.Text, phoneText.Text);
            NavigationService.GoBack();
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