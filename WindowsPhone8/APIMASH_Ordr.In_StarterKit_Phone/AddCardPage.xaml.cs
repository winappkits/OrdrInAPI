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
using System.Windows.Input;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class AddCardPage : PhoneApplicationPage
    {
        public AddCardPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;
            Globals.Instance.ordrin.CurrentCard = new Card_Bindable();
            this.Loaded += AddCardPage_Loaded;
            this.Unloaded += AddCardPage_Unloaded;
        }

        void AddCardPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Globals.Instance.ordrin.OnError -= ordrin_OnError;
        }

        void AddCardPage_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.Instance.ordrin.OnError += ordrin_OnError;

            string nick = "";
            if (NavigationContext.QueryString.TryGetValue("nick", out nick))
            {
                var card = Globals.Instance.ordrin.Cards.FirstOrDefault(a => a.Nick == nick);
                if (card != null)
                {
                    Globals.Instance.ordrin.CurrentCard.Nick = card.Nick;
                    ((MonthClass)this.expireMonth.SelectedItem).Value = card.CardMonth;
                    ((MonthClass)this.expireYear.SelectedItem).Value = card.CardYear;
                    ((MonthClass)this.cardType.SelectedItem).Value = card.CardType;
                    Globals.Instance.ordrin.CurrentCard.Address = card.Address;
                    Globals.Instance.ordrin.CurrentCard.Address2 = card.Address2;
                    Globals.Instance.ordrin.CurrentCard.City = card.City;
                    Globals.Instance.ordrin.CurrentCard.State = card.State;
                    Globals.Instance.ordrin.CurrentCard.Zip = card.Zip;
                    Globals.Instance.ordrin.CurrentCard.Phone = card.Phone;
                }
            }
        }

        void ordrin_OnError(object sender, ErrorEventArgs e)
        {
            switch (e.Code)
            {
                case ErrorCode.CardSaveError:
                    string company = Application.Current.Resources["AppName"] as string;
                    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                    break;
                case ErrorCode.CardSaved:
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
                MessageBox.Show("The card entered needs a name.  Please enter a name and submit it again.", company, MessageBoxButton.OK);
                return;
            }
            if (cardNumberText.Text == null || cardNumberText.Text == "")
            {
                MessageBox.Show("The card needs a number.  Please enter a credit card number and submit it again.", company, MessageBoxButton.OK);
                return;
            }
            if (securityText.Text == null || securityText.Text == "" || securityText.Text.Length < 3 || securityText.Text.Length > 4)
            {
                MessageBox.Show("The card needs a valid security code.  Please enter a security code and submit it again.", company, MessageBoxButton.OK);
                return;
            }

            if (UseDeliveryAddress.IsChecked == true)
            {
                if(Globals.Instance.ordrin.CurrentAddress == null)
                {
                    MessageBox.Show("The card needs a valid address.  Please enter an address and submit it again.", company, MessageBoxButton.OK);
                    return;
                }

                Globals.Instance.ordrin.CurrentCard.Zip = Globals.Instance.ordrin.CurrentAddress.Zip;
                Globals.Instance.ordrin.CurrentCard.State = Globals.Instance.ordrin.CurrentAddress.State;
                Globals.Instance.ordrin.CurrentCard.City = Globals.Instance.ordrin.CurrentAddress.City;
                Globals.Instance.ordrin.CurrentCard.Address = Globals.Instance.ordrin.CurrentAddress.Address;
                Globals.Instance.ordrin.CurrentCard.Address2 = Globals.Instance.ordrin.CurrentAddress.Address2;
            }

            Globals.Instance.ordrin.AddCard(Globals.Instance.ordrin.CurrentCard.Nick,
                Globals.Instance.ordrin.Account.first_name + " " + Globals.Instance.ordrin.Account.last_name,
                this.cardNumberText.Text,
                this.securityText.Text,
                ((MonthClass)this.expireMonth.SelectedItem).Value,
                ((MonthClass)this.expireYear.SelectedItem).Value,
                ((MonthClass)this.cardType.SelectedItem).Value,
                Globals.Instance.ordrin.CurrentCard.Address,
                Globals.Instance.ordrin.CurrentCard.Address2,
                Globals.Instance.ordrin.CurrentCard.City,
                Globals.Instance.ordrin.CurrentCard.State,
                Globals.Instance.ordrin.CurrentCard.Zip,
                Globals.Instance.ordrin.CurrentCard.Phone
                );

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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb.IsChecked == true)
            {
                if (this.zipText != null && Globals.Instance.ordrin.CurrentAddress != null)
                    this.zipText.Text = Globals.Instance.ordrin.CurrentAddress.Zip;
                if (ccAddress != null)
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

    }
}