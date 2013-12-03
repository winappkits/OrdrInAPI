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
    public partial class DeliveryPage : PhoneApplicationPage
    {
        public DeliveryPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;
            //if (firstNameText.Text == null || firstNameText.Text == "")
            //{
            //    MessageBox.Show("First name is required to continue.", company, MessageBoxButton.OK);
            //    return;
            //}
            //if (lastNameText.Text == null || lastNameText.Text == "")
            //{
            //    MessageBox.Show("First name is required to continue.", company, MessageBoxButton.OK);
            //    return;
            //}
            //if (emailText.Text == null || emailText.Text == "")
            //{
            //    MessageBox.Show("Email address is required to continue.", company, MessageBoxButton.OK);
            //    return;
            //}
            //if (phoneText.Text == null || phoneText.Text == "")
            //{
            //    MessageBox.Show("Phone number is required to continue.", company, MessageBoxButton.OK);
            //    return;
            //}
            NavigationService.Navigate(new Uri("/PaymentPage.xaml", UriKind.Relative));
        }

        private void firstNameText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
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