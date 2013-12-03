using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using APIMASH_OrdrInLib;
using System.Windows.Input;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class PaymentPage : PhoneApplicationPage
    {
        public PaymentPage()
        {
            Globals.Instance.ordrin.CurrentCard = Globals.Instance.ordrin.DefaultCard;
            if (Globals.Instance.ordrin.CurrentCard.Zip == null)
            {
                Globals.Instance.ordrin.CurrentCard.Zip = Globals.Instance.ordrin.CurrentAddress.Zip;
                Globals.Instance.ordrin.CurrentCard.State = Globals.Instance.ordrin.CurrentAddress.State;
                Globals.Instance.ordrin.CurrentCard.City = Globals.Instance.ordrin.CurrentAddress.City;
                Globals.Instance.ordrin.CurrentCard.Address = Globals.Instance.ordrin.CurrentAddress.Address;
            }


            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;
            this.ZipCode.Text = Globals.Instance.ordrin.CurrentAddress.Zip;
            orderWait.IsIndeterminate = false;

            if (Globals.Instance.ordrin.CurrentCard.FullLast5.Contains('*') == true && Globals.Instance.ordrin.CurrentCard.Nick != "")
            {
                // hide the other controls
                expireMonth.Visibility = System.Windows.Visibility.Collapsed;
                expireYear.Visibility = System.Windows.Visibility.Collapsed;
                SecurityCode.Visibility = System.Windows.Visibility.Collapsed;
                ZipCode.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (Globals.Instance.ordrin.CurrentCard.CardMonth != "")
                expireMonth.SelectedItem = Globals.Instance.ordrin.MonthList.First(m => m.Value == Globals.Instance.ordrin.CurrentCard.CardMonth);
            if (Globals.Instance.ordrin.CurrentCard.CardYear != "")
                expireYear.SelectedItem = Globals.Instance.ordrin.YearList.First(m => m.Value == Globals.Instance.ordrin.CurrentCard.CardYear);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Globals.Instance.ordrin.OnError += ordrin_OnError;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Globals.Instance.ordrin.OnError -= ordrin_OnError;
            base.OnNavigatingFrom(e);
        }
        void ordrin_OnError(object sender, APIMASH_OrdrInLib.ErrorEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;
            orderWait.IsIndeterminate = false;

            switch (e.Code)
            {
                case ErrorCode.OrderPlaced:
                    NavigationService.Navigate(new Uri("/ConfirmPage.xaml", UriKind.Relative));
                    break;
                case ErrorCode.OrderError:
                    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;

            if (Globals.Instance.ordrin.CurrentCard.FullLast5 == null || Globals.Instance.ordrin.CurrentCard.FullLast5.Length == 0)
            {
                MessageBox.Show("Please fill in your credit card information to place an order.", company, MessageBoxButton.OK);
                return;
            }
            if (Globals.Instance.ordrin.CurrentCard.FullLast5.Contains('*') == false && (Globals.Instance.ordrin.CurrentCard.Security == null || Globals.Instance.ordrin.CurrentCard.Security.Length == 0))
            {
                MessageBox.Show("Please fill in your credit card security code to place an order.", company, MessageBoxButton.OK);
                return;
            }
            if (Globals.Instance.ordrin.CurrentCard.FullLast5.Contains('*') == false && (Globals.Instance.ordrin.CurrentCard.Security.Length < 3 || Globals.Instance.ordrin.CurrentCard.Security.Length > 4))
            {
                MessageBox.Show("Your security code is incorrect. Please check and submit again.", company, MessageBoxButton.OK);
                return;
            }
            if (Globals.Instance.ordrin.CurrentCard.FullLast5.Contains('*') == false && Globals.Instance.ordrin.CurrentCard.Security.Contains('.'))
            {
                MessageBox.Show("Your security code is incorrect. Please check and submit again.", company, MessageBoxButton.OK);
                return;
            }
            if (Globals.Instance.ordrin.CurrentCard.FullLast5.Contains('*') == false && (Globals.Instance.ordrin.CurrentCard.Zip == null || Globals.Instance.ordrin.CurrentCard.Zip.Length == 0))
            {
                MessageBox.Show("Please fill in your credit card zipcode to place an order.", company, MessageBoxButton.OK);
                return;
            }
            if (((MonthClass)this.expireMonth.SelectedItem).Value == "00" || ((MonthClass)this.expireYear.SelectedItem).Value == "00")
            {
                MessageBox.Show("Please fill in your credit card expiration date to place an order.", company, MessageBoxButton.OK);
                return;
            }

            Globals.Instance.ordrin.CurrentCard.Expire = ((MonthClass)this.expireMonth.SelectedItem).Value + "/" + ((MonthClass)this.expireYear.SelectedItem).Value;
            if (int.Parse(((MonthClass)this.expireMonth.SelectedItem).Value) < DateTime.Now.Month && int.Parse(((MonthClass)this.expireYear.SelectedItem).Value) == DateTime.Now.Year)
            {
                MessageBox.Show("Your credit card has expired.  Please update the expiration date to place an order.", company, MessageBoxButton.OK);
                return;
            }
            
            //if (UseDeliveryAddress.IsChecked == true)
            //{
            //    Globals.Instance.ordrin.CurrentCard.Zip = Globals.Instance.ordrin.CurrentAddress.Zip;
            //    Globals.Instance.ordrin.CurrentCard.State = Globals.Instance.ordrin.CurrentAddress.State;
            //    Globals.Instance.ordrin.CurrentCard.City = Globals.Instance.ordrin.CurrentAddress.City;
            //    Globals.Instance.ordrin.CurrentCard.Address = Globals.Instance.ordrin.CurrentAddress.Address;
            //}
            if (Globals.Instance.ordrin.CurrentCard.Nick == null || Globals.Instance.ordrin.CurrentCard.Nick.Length == 0)
                Globals.Instance.ordrin.CurrentCard.Nick = "Temp Card";

            orderWait.IsIndeterminate = true;
            Globals.Instance.ordrin.PlaceOrder();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb.IsChecked == true)
            {
                if (this.ZipCode != null)
                    this.ZipCode.Text = Globals.Instance.ordrin.CurrentAddress.Zip;
                if(ccAddress != null)
                    ccAddress.IsEnabled = false;
            }
            else
            {
                if (ccAddress != null)
                    ccAddress.IsEnabled = true;
            }
        }

        private void ccAddress_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ccAddressPage.xaml", UriKind.Relative));
        }

        private void CreditCardNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;
            s.Foreground = Application.Current.Resources["BlackColor"] as SolidColorBrush;
            s.Select(0, s.Text.Length);
        }

        private void CreditCardNumber_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // if the enter key is pressed
            if (e.Key == Key.Enter)
            {
                // focus the page in order to remove focus from the text box
                // and hide the soft keyboard
                this.Focus();
            }
        }

        private void CreditCardNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (s.Text.Contains('*') == true)
            {
                // hide the other controls
                expireMonth.Visibility = System.Windows.Visibility.Collapsed;
                expireYear.Visibility = System.Windows.Visibility.Collapsed;
                SecurityCode.Visibility = System.Windows.Visibility.Collapsed;
                ZipCode.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                // show the other controls
                expireMonth.Visibility = System.Windows.Visibility.Visible;
                expireYear.Visibility = System.Windows.Visibility.Visible;
                SecurityCode.Visibility = System.Windows.Visibility.Visible;
                ZipCode.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}