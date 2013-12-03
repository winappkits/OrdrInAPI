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
using System.Windows.Media;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class TotalPage : PhoneApplicationPage
    {
        public TotalPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;

            CalcTotal();
            Globals.Instance.ordrin.OnError += ordrin_OnError;

        }

        private void ordrin_OnError(object sender, ErrorEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;

            switch (e.Code)
            {
                case ErrorCode.RestaurantClosed:
                    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                    checkoutButton.IsEnabled = false;
                    checkoutButton.Opacity = 0.30f;
                    //int cnt = NavigationService.BackStack.Count();
                    //for (int i = 0; i < cnt - 1; i++)
                    //{
                    //    NavigationService.RemoveBackEntry();
                    //}
                    //NavigationService.GoBack();
                    break;
                //case ErrorCode.ListLoaded:
                //    DeliveriesLoaded();
                //    break;
                //case ErrorCode.AccountCreated:
                //    defaultLogin = new LoginInfo()
                //    {
                //        username = e.Message,
                //        password = e.Secondary
                //    };
                //    SaveLoginInfo();

                //    HideLoginInfo();
                //    NavigationService.Navigate(new Uri("/AddressPage.xaml", UriKind.Relative));
                //    break;
            }
        }

        void CalcTotal()
        {
            float total = 0.0f;
            foreach (var item in Globals.Instance.ordrin.Tray)
            {
                total += item.SubTotal;
            }
            Globals.Instance.ordrin.SubTotal = total;

            if (this.tipAmount.ItemsSource == null)
            {
                this.tipAmount.ItemsSource = Globals.Instance.ordrin.TipList;
                this.tipAmount.SelectedItem = Globals.Instance.ordrin.TipList[3];
            }
            CalcTip();
            Globals.Instance.ordrin.LoadDeliveryFee(int.Parse(Globals.Instance.ordrin.CurrentRestaurant.Id), Globals.Instance.ordrin.SubTotal, Globals.Instance.ordrin.CurrentTip);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            float minOrder = Globals.Instance.ordrin.CurrentDelivery.MinOrder;
            if (Globals.Instance.ordrin.SubTotal < minOrder)
            {
                string company = Application.Current.Resources["AppName"] as string;
                var result = MessageBox.Show("The minimum order for " + Globals.Instance.ordrin.CurrentRestaurant.Name + " is $" + minOrder + ".  Please add more to your cart to place the order.", company, MessageBoxButton.OK);
                return;
            }
            NavigationService.Navigate(new Uri("/DeliveryPage.xaml", UriKind.Relative));
        }

        void CalcTip()
        {
            var tip = tipAmount.SelectedItem as TipClass;
            Globals.Instance.ordrin.CurrentTip = (Globals.Instance.ordrin.SubTotal * tip.Multiplier);
            float taxes;
            float fees;
            if (float.TryParse(Globals.Instance.ordrin.CurrentFee.Tax, out taxes) != true)
                return;
            if (float.TryParse(Globals.Instance.ordrin.CurrentFee.Fee, out fees) != true)
                return;
            Globals.Instance.ordrin.GrandTotal = Globals.Instance.ordrin.CurrentTip + Globals.Instance.ordrin.SubTotal + fees + taxes;
        }

        private void tipAmount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
                CalcTip();
        }

        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dc = (TrayItem_Bindable)((ListPicker)sender).DataContext;
            if (e.AddedItems.Count > 0)
            {
                dc.Quantity = ((QuantityClass)e.AddedItems[0]).Value;
                Globals.Instance.ordrin.UpdateTrayCount();
                CalcTotal();
            }
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;

            var result = MessageBox.Show("Do you want to remove this item from your cart?", company, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                var dc = ((TextBlock)sender).DataContext;
                Globals.Instance.ordrin.Tray.Remove((TrayItem_Bindable)dc);
                Globals.Instance.ordrin.UpdateTrayCount();
                CalcTotal();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (int.Parse((sender as TextBox).Text) == 0)
            //{
            //    var dc = ((TextBox)sender).DataContext;
            //    Globals.Instance.ordrin.Tray.Remove((TrayItem_Bindable)dc);
            //    Globals.Instance.ordrin.UpdateTrayCount();
            //}
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;
//            s.Foreground = Application.Current.Resources["WhiteColor"] as SolidColorBrush;

            int quantity = 1;
            if (int.TryParse(s.Text, out quantity) == false)
            {
                s.Text = "1";
            }

            var dc = (TrayItem_Bindable)s.DataContext;
            dc.Quantity = quantity;
            Globals.Instance.ordrin.UpdateTrayCount();
            CalcTotal();

            if (int.Parse(s.Text) == 0)
            {
                Globals.Instance.ordrin.Tray.Remove((TrayItem_Bindable)dc);
                Globals.Instance.ordrin.UpdateTrayCount();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;
//            s.Foreground = Application.Current.Resources["BlackColor"] as SolidColorBrush;
            s.Select(0, s.Text.Length);
        }

    }
}