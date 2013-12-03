using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Device.Location;
using Windows.Devices.Geolocation;
using APIMASH_OrdrInLib;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.ApplicationModel;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;
using Microsoft.Phone.Globalization;
using ReviewNotifier.Apollo;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool switchTileColors = false;

        // Constructor
        public MainPage()
        {
            DataContext = Globals.Instance.ordrin;

            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            Globals.Instance.ordrin.OnError += ordrin_OnError;
//            Globals.Instance.ordrin.Deliveries.CollectionChanged += Deliveries_CollectionChanged;
            Globals.Instance.ordrin.OrderHistories.CollectionChanged += OrderHistories_CollectionChanged;


            //string Version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            var parts = Assembly.GetExecutingAssembly().FullName.Split(',');
            var lcVersionStr = parts[1].Split('=')[1];
            versionText.Text = lcVersionStr;
            this.Loaded += MainPage_Loaded;
//            progressOverlay.Hide();
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LittleWatson.CheckForPreviousException();
            await ReviewNotification.TriggerAsync("Would you like to rate and review the Windows Phone Ordr.In app right now?", "Thanks for using Ordr.In.", 5);
        }

        void OrderHistories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }
        List<AlphaKeyGroup<Delivery_Bindable>> DataSource;

        void DeliveriesLoaded()
        {
            //var list = Globals.Instance.ordrin.Deliveries.ToList();
            DataSource = AlphaKeyGroup<Delivery_Bindable>.CreateGroups(Globals.Instance.ordrin.Deliveries,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (Delivery_Bindable s) => { return s.Name; }, true);
            deliveryList.ItemsSource = DataSource; // Globals.Instance.ordrin.Deliveries;
            //wrap.Children.Clear();
            //bool switchColors = false;
            //Random rand = new Random();
            //int switchCnt = rand.Next() % 2;
            //SolidColorBrush darkRedBrush = Application.Current.Resources["DarkRedColor"] as SolidColorBrush;
            //SolidColorBrush whiteBrush = Application.Current.Resources["WhiteColor"] as SolidColorBrush;
            //foreach (var item in Globals.Instance.ordrin.Deliveries)
            //{
            //    Border b = new Border()
            //    {
            //        Width = 200,
            //        Height = 200,
            //        Background = darkRedBrush,
            //        BorderThickness = new Thickness(2),
            //        Margin = new Thickness(8),
            //        Tag = item.Id

            //    };
            //    b.Tap += b_Tap;

            //    if (switchColors == false)
            //    {
            //        switchColors = rand.Next() % 5 == 0;
            //    }
            //    if (switchColors == false && switchTileColors == true)
            //        b.Background = darkRedBrush;
            //    else
            //    {
            //        b.Background = Application.Current.Resources["LightRedColor"] as SolidColorBrush;
            //        switchCnt++;
            //        if (switchCnt >= 2)
            //        {
            //            switchColors = false;
            //            switchCnt = rand.Next() % 2;
            //        }
            //    }

            //    var t = new TextBlock()
            //    {
            //        Text = item.Name,
            //        TextWrapping = TextWrapping.Wrap,
            //        FontFamily = new FontFamily("Segeo WP"),
            //        FontSize = 36,
            //        Foreground = whiteBrush
            //    };
            //    b.Child = t;

            //    wrap.Children.Add(b);
            //}
            progressLoad.IsIndeterminate = false;
            progressLoad.Visibility = System.Windows.Visibility.Collapsed;
            panControl.SlideToPage(0);
        }

        void Deliveries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //wrap.Children.Clear();
            //bool switchColors = false;
            //Random rand = new Random();
            //int switchCnt = rand.Next() % 2;
            //foreach (var item in Globals.Instance.ordrin.Deliveries)
            //{
            //    Border b = new Border()
            //    {
            //        Width = 200,
            //        Height = 200,
            //        Background = Application.Current.Resources["DarkRedColor"] as SolidColorBrush,
            //        BorderThickness = new Thickness(2),
            //        Margin = new Thickness(8),
            //        Tag = item.Id
                    
            //    };
            //    b.Tap += b_Tap;

            //    if (switchColors == false)
            //    {
            //        switchColors = rand.Next() % 5 == 0;
            //    }
            //    if (switchColors == false && switchTileColors == true)
            //        b.Background = Application.Current.Resources["DarkRedColor"] as SolidColorBrush;
            //    else
            //    {
            //        b.Background = Application.Current.Resources["LightRedColor"] as SolidColorBrush;
            //        switchCnt++;
            //        if (switchCnt >= 2)
            //        {
            //            switchColors = false;
            //            switchCnt = rand.Next() % 2;
            //        }
            //    }

            //    var t = new TextBlock()
            //    {
            //        Text = item.Name,
            //        TextWrapping = TextWrapping.Wrap,
            //        FontFamily = new FontFamily("Segeo WP"),
            //        FontSize = 36,
            //        Foreground = Application.Current.Resources["WhiteColor"] as SolidColorBrush
            //    };
            //    b.Child = t;

            //    wrap.Children.Add(b);
            //}
         //   listScroller.Height = eatGrid.RowDefinitions[1].ActualHeight;
            deliveryList.Height = eatGrid.RowDefinitions[1].ActualHeight;
            progressLoad.IsIndeterminate = false;
            progressLoad.Visibility = System.Windows.Visibility.Collapsed;
            //            progressOverlay.Hide();

        }

        private async void LoadDeliveryList()
        {
            Globals.Instance.ordrin.Deliveries.Clear();

            Geolocator geolocator = new Geolocator();

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync();
//                progressOverlay.Show();
                Globals.Instance.ordrin.LoadDeliveryList(longitude: geoposition.Coordinate.Longitude, latitude: geoposition.Coordinate.Latitude);
            }
            catch (Exception ex)
            {
            }
        }

        // Load data for the ViewModel Items
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                LoadLoginInfo();
                if (defaultLogin == null)
                {
                    panControl.SlideToPage(5);
                    //                    progressOverlay.Show();
                    //LoadDeliveryList();
                }
                else
                {
                    panControl.SlideToPage(0);
                    progressLoad.IsIndeterminate = true;
                    progressLoad.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        void b_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var border = sender as Border;

            if (Globals.Instance.ordrin.CurrentRestaurant != null && Globals.Instance.ordrin.CurrentRestaurant.Id != border.Tag.ToString())
                Globals.Instance.ordrin.CurrentRestaurant = null;

            NavigationService.Navigate(new Uri("/RestaurantPage.xaml?id=" + border.Tag, UriKind.Relative));
        }

        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            //if (LocationButton.IsChecked.Value)
            //{
            //    PriceButton.IsChecked = false;
            //    LocationButton.Style = Application.Current.Resources["SelectedToggleStyle"] as Style;
            //    PriceButton.Style = Application.Current.Resources["UnselectedToggleStyle"] as Style;
            //    pickupBorder.Background = Application.Current.Resources["WhiteColor"] as Brush;
            //    deliveryBorder.Background = Application.Current.Resources["LightRedColor"] as Brush;
            //    deliveryList.ItemsSource = Globals.Instance.ordrin.Deliveries;

            //    //wrap.Children.Clear();
            //    //bool switchColors = false;
            //    //Random rand = new Random();
            //    //int switchCnt = rand.Next() % 2;
            //    //foreach (var item in Globals.Instance.ordrin.Deliveries)
            //    //{
            //    //    Border b = new Border()
            //    //    {
            //    //        Width = 200,
            //    //        Height = 200,
            //    //        Background = Application.Current.Resources["DarkRedColor"] as SolidColorBrush,
            //    //        BorderThickness = new Thickness(2),
            //    //        Margin = new Thickness(8),
            //    //        Tag = item.Id

            //    //    };
            //    //    b.Tap += b_Tap;

            //    //    if (switchColors == false)
            //    //    {
            //    //        switchColors = rand.Next() % 5 == 0;
            //    //    }
            //    //    if (switchColors == false && switchTileColors == true)
            //    //        b.Background = Application.Current.Resources["DarkRedColor"] as SolidColorBrush;
            //    //    else
            //    //    {
            //    //        b.Background = Application.Current.Resources["LightRedColor"] as SolidColorBrush;
            //    //        switchCnt++;
            //    //        if (switchCnt >= 2)
            //    //        {
            //    //            switchColors = false;
            //    //            switchCnt = rand.Next() % 2;
            //    //        }
            //    //    }

            //    //    var t = new TextBlock()
            //    //    {
            //    //        Text = item.Name,
            //    //        TextWrapping = TextWrapping.Wrap,
            //    //        FontFamily = new FontFamily("Segeo WP"),
            //    //        FontSize = 36,
            //    //        Foreground = Application.Current.Resources["WhiteColor"] as SolidColorBrush
            //    //    };
            //    //    b.Child = t;

            //    //    wrap.Children.Add(b);
            //    //}

            //}
        }

        private void PriceButton_Click(object sender, RoutedEventArgs e)
        {
            //if (PriceButton.IsChecked.Value)
            //{
            //    LocationButton.IsChecked = false;
            //    PriceButton.Style = Application.Current.Resources["SelectedToggleStyle"] as Style;
            //    LocationButton.Style = Application.Current.Resources["UnselectedToggleStyle"] as Style;
            //    deliveryBorder.Background = Application.Current.Resources["WhiteColor"] as Brush;
            //    pickupBorder.Background = Application.Current.Resources["LightRedColor"] as Brush;
            //    deliveryList.ItemsSource = Globals.Instance.ordrin.Pickups;

            //    //wrap.Children.Clear();
            //    //bool switchColors = false;
            //    //Random rand = new Random();
            //    //int switchCnt = rand.Next() % 2;
            //    //foreach (var item in Globals.Instance.ordrin.Pickups)
            //    //{
            //    //    Border b = new Border()
            //    //    {
            //    //        Width = 200,
            //    //        Height = 200,
            //    //        Background = Application.Current.Resources["DarkRedColor"] as SolidColorBrush,
            //    //        BorderThickness = new Thickness(2),
            //    //        Margin = new Thickness(8),
            //    //        Tag = item.Id

            //    //    };
            //    //    b.Tap += b_Tap;

            //    //    if (switchColors == false)
            //    //    {
            //    //        switchColors = rand.Next() % 5 == 0;
            //    //    }
            //    //    if (switchColors == false && switchTileColors == true)
            //    //        b.Background = Application.Current.Resources["DarkRedColor"] as SolidColorBrush;
            //    //    else
            //    //    {
            //    //        b.Background = Application.Current.Resources["LightRedColor"] as SolidColorBrush;
            //    //        switchCnt++;
            //    //        if (switchCnt >= 2)
            //    //        {
            //    //            switchColors = false;
            //    //            switchCnt = rand.Next() % 2;
            //    //        }
            //    //    }

            //    //    var t = new TextBlock()
            //    //    {
            //    //        Text = item.Name,
            //    //        TextWrapping = TextWrapping.Wrap,
            //    //        FontFamily = new FontFamily("Segeo WP"),
            //    //        FontSize = 36,
            //    //        Foreground = Application.Current.Resources["WhiteColor"] as SolidColorBrush
            //    //    };
            //    //    b.Child = t;

            //    //    wrap.Children.Add(b);
            //    //}


            //}
        }

        private void AddAddressButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddressPage.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(Globals.Instance.ordrin.CurrentRestaurant != null && Globals.Instance.ordrin.Tray.Count > 0)
                NavigationService.Navigate(new Uri("/TotalPage.xaml", UriKind.Relative));
            else
            {
                string company = Application.Current.Resources["AppName"] as string;
                MessageBox.Show("Please add something to the cart first before checking out.", company, MessageBoxButton.OK);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
//            NavigationService.Navigate(new Uri("/DeliveryPage.xaml", UriKind.Relative));
            NavigationService.Navigate(new Uri("/DecidePage.xaml", UriKind.Relative));
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {            
            // perform login
            Globals.Instance.ordrin.LoadAccount(loginEmail.Text, loginPassword.Password);

            if (saveLoginCheck.IsChecked == true)
            {
                defaultLogin = new LoginInfo()
                {
                    username = loginEmail.Text,
                    password = loginPassword.Password
                };
                SaveLoginInfo();
            }
            HideLoginInfo();
        }

        private void createAccountButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountPage.xaml", UriKind.Relative));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb.IsChecked == true)
            {
                cb.Background = Application.Current.Resources["BrownColor"] as SolidColorBrush;
            }
            else
            {
                cb.Background = null;
            }
        }

        private void signOutButton_Click(object sender, RoutedEventArgs e)
        {
            addressPanItem.Visibility = System.Windows.Visibility.Collapsed;

            accountControls.Visibility = System.Windows.Visibility.Collapsed;
            loginButton.Visibility = System.Windows.Visibility.Visible;
            resetButton.Visibility = System.Windows.Visibility.Visible;

            loginEmail.Text = string.Empty;
            loginPassword.Password = string.Empty;
            CheckPasswordWatermark();
            CheckEmailWatermark();

            saveLoginCheck.IsChecked = true;

            loginEmail.Visibility = System.Windows.Visibility.Visible;
            loginEmailw.Visibility = System.Windows.Visibility.Visible;
            loginPassword.Visibility = System.Windows.Visibility.Visible;
            loginPasswordWatermark.Visibility = System.Windows.Visibility.Visible;
            saveLoginCheck.Visibility = System.Windows.Visibility.Visible;
            createAccountButton.Visibility = System.Windows.Visibility.Visible;

            Globals.Instance.ordrin.Signout();
            DeleteDefaultAddress();
            DeleteLoginInfo();
            LoadDeliveryList();
        }

        private void addressList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var address = addressList.SelectedItem as Address_Bindable;
            progressLoad.IsIndeterminate = true;
            progressLoad.Visibility = System.Windows.Visibility.Visible;
            Globals.Instance.ordrin.CurrentAddress = address;
            foreach (var item in Globals.Instance.ordrin.Addresses)
            {
                item.IsDefault = 0;
            }
            if (Globals.Instance.ordrin.CurrentAddress != null)
            {
                Globals.Instance.ordrin.CurrentAddress.IsDefault = 1;
                SaveDefaultAddress();
            }
            if (address != null)
            {
                panControl.SlideToPage(0);
                Globals.Instance.ordrin.LoadDeliveryList(address.Zip, address.City, address.Address, "ASAP");
            }
        }

        private void DeleteDefaultAddress()
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                myIsolatedStorage.DeleteFile("address.default");
            }
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
        private void ChangeDefaultAddress()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("address.default", FileMode.Open))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        var name = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);
                        var address = Globals.Instance.ordrin.Addresses.FirstOrDefault(a => a.Nick == name);
                        if (address != null)
                        {
                            foreach (var item in Globals.Instance.ordrin.Addresses)
                            {
                                item.IsDefault = 0;
                            }
                            Globals.Instance.ordrin.CurrentAddress = address;
                            Globals.Instance.ordrin.CurrentAddress.IsDefault = 1;
                        }
                        else
                        {
                            if (Globals.Instance.ordrin.Addresses.Count > 0)
                            {
                                var add = Globals.Instance.ordrin.Addresses.First();
                                Globals.Instance.ordrin.CurrentAddress = add;
                                Globals.Instance.ordrin.CurrentAddress.IsDefault = 1;
                            }
                        }
                    }
                }
            }
            catch
            {
                //add some code here
            }
        }

        private void loginPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            loginPasswordWatermark.Opacity = 0;
            loginPassword.Opacity = 100;
        }

        private void loginPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckPasswordWatermark();
        }

        public void CheckPasswordWatermark()
        {
            var passwordEmpty = string.IsNullOrEmpty(loginPassword.Password);
            loginPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            loginPassword.Opacity = passwordEmpty ? 0 : 100;
        }
        public void CheckEmailWatermark()
        {
            var passwordEmpty = string.IsNullOrEmpty(loginEmail.Text);
            loginEmailw.Opacity = passwordEmpty ? 100 : 0;
            loginEmail.Opacity = passwordEmpty ? 0 : 100;
        }

        #region login info
        private void DeleteLoginInfo()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    myIsolatedStorage.DeleteFile("login.xml");
                }
            }
            catch
            {    //add some code here
            }
        }

        LoginInfo defaultLogin = null;

        public void SaveLoginInfo()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("login.xml", FileMode.OpenOrCreate))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LoginInfo));
                        serializer.Serialize(stream, defaultLogin);
                    }
                }
            }
            catch
            {    //add some code here
            }
        }


        public void LoadLoginInfo()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("login.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(LoginInfo));
                        defaultLogin = (LoginInfo)serializer.Deserialize(stream);
                        
                        Globals.Instance.ordrin.LoadAccount(defaultLogin.username, defaultLogin.password);
                        
                        HideLoginInfo();
                    }
                }
            }
            catch
            {    //add some code here
            }
        }

        private void HideLoginInfo()
        {

            accountControls.Visibility = System.Windows.Visibility.Visible;
            loginButton.Visibility = System.Windows.Visibility.Collapsed;
            resetButton.Visibility = System.Windows.Visibility.Collapsed;

            loginEmail.Visibility = System.Windows.Visibility.Collapsed;
            loginEmailw.Visibility = System.Windows.Visibility.Collapsed;
            loginPassword.Visibility = System.Windows.Visibility.Collapsed;
            loginPasswordWatermark.Visibility = System.Windows.Visibility.Collapsed;
            saveLoginCheck.Visibility = System.Windows.Visibility.Collapsed;
            createAccountButton.Visibility = System.Windows.Visibility.Collapsed;

            panControl.SlideToPage(0);
        }

        #endregion login info

        #region error handling
        void ordrin_OnError(object sender, ErrorEventArgs e)
        {
            string company = Application.Current.Resources["AppName"] as string;

            switch(e.Code)
            {
                case ErrorCode.LoginFailed:
                    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                    signOutButton_Click(sender, null);
                    break;
                case ErrorCode.PasswordReset:
                    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                    break;
                //case ErrorCode.RestaurantClosed:
                //    MessageBox.Show(e.Message, company, MessageBoxButton.OK);
                //    int cnt = NavigationService.BackStack.Count();
                //    for (int i = 0; i < cnt-1; i++)
                //    {
                //        NavigationService.RemoveBackEntry();
                //    }
                //    NavigationService.GoBack();
                //    break;
                case ErrorCode.ListLoading:
                    DeliveriesLoaded();
                    Globals.Instance.ordrin.LoadPosition();
                    progressLoad.IsIndeterminate = true;
                    progressLoad.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ErrorCode.ListLoaded:
                    DeliveriesLoaded();
                    break;
                case ErrorCode.OrdersLoaded:
                    OrdersLoaded();
                    break;
                case ErrorCode.AddressesLoaded:
                    addressPanItem.Visibility = System.Windows.Visibility.Visible;
                    //addressList.Height = addressStack.RowDefinitions[0].ActualHeight - 40;
                    ChangeDefaultAddress();
                    panControl.SlideToPage(0);
                    break;
                case ErrorCode.AccountCreated:
                    defaultLogin = new LoginInfo()
                    {
                        username = e.Message,
                        password = e.Secondary
                    };
                    SaveLoginInfo();
                    HideLoginInfo();
                    NavigationService.Navigate(new Uri("/AddressPage.xaml", UriKind.Relative));
                    break;
            }
        }

        private void OrdersLoaded()
        {
            //if(Globals.Instance.ordrin.OrderHistories.Count > 0)
            //{
            //    ordersPanItem.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        #endregion

        private void addressesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/YourAddressesPage.xaml", UriKind.Relative));
        }

        private void cardsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/YourCardsPage.xaml", UriKind.Relative));
        }

        private void loginEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            loginEmailw.Opacity = 0;
            loginEmail.Opacity = 100;
        }

        private void loginEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckEmailWatermark();
        }

        private void passwordButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Instance.ordrin.ResetPassword(loginEmail.Text);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Instance.ordrin.ResetPassword(loginEmail.Text);
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // selected a previous order
            if (e.AddedItems.Count > 0)
            {
                Globals.Instance.ordrin.CurrentOrderHistory = e.AddedItems[0] as OrderHistory_Bindable;

                NavigationService.Navigate(new Uri("/OldCartPage.xaml", UriKind.Relative));
            }

        }

        private void loginEmail_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // if the enter key is pressed
            if (e.Key == Key.Enter)
            {
                // focus the page in order to remove focus from the text box
                // and hide the soft keyboard
                this.Focus();
            }
        }

        private void pickupBorder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PriceButton_Click(sender, null);
        }

        private void deliveryBorder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LocationButton_Click(sender, null);
        }

        private void deliveryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as LongListSelector;
            if(e.AddedItems.Count > 0)
            {
                var border = e.AddedItems[0] as Delivery_Bindable;

                if (Globals.Instance.ordrin.CurrentRestaurant != null && Globals.Instance.ordrin.CurrentRestaurant.Id != border.Id.ToString())
                    Globals.Instance.ordrin.CurrentRestaurant = null;

                NavigationService.Navigate(new Uri("/RestaurantPage.xaml?id=" + border.Id, UriKind.Relative));
            }
        }

        private void addressPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            addressesButton_Click(sender, null);
        }

        private void signoutPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            signOutButton_Click(sender, null);
        }

        private void cardPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cardsButton_Click(sender, null);
        }

        private void deliveryList_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (e.ItemKind == LongListSelectorItemKind.GroupHeader)
            {
//                deliveryList..ScrollTo(e.Container.Content);
            }
        }


    }

    public class LoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public static class PanoramaExtensions
    {
        public static void SlideToPage(this Panorama self, int item)
        {

            var slide_transition = new SlideTransition() { };
            slide_transition.Mode = SlideTransitionMode.SlideLeftFadeIn;
            ITransition transition = slide_transition.GetTransition(self);
            transition.Completed += delegate
            {
                self.DefaultItem = self.Items[item];
                transition.Stop();
            };
            transition.Begin();
        }
    }


    public class AlphaKeyGroup<T> : List<T>
    {
        const string GlobeGroupKey = "\uD83C\uDF10";

        /// <summary>
        /// The Key of this group.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Public ctor.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        public AlphaKeyGroup(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="slg">The </param>
        /// <returns>Theitems source for a LongListSelector</returns>
        private static List<AlphaKeyGroup<T>> CreateDefaultGroups(SortedLocaleGrouping slg)
        {
            List<AlphaKeyGroup<T>> list = new List<AlphaKeyGroup<T>>();

            foreach (string key in slg.GroupDisplayNames)
            {
                if (key == "...")
                {
                    list.Add(new AlphaKeyGroup<T>(GlobeGroupKey));
                }
                else
                {
                    list.Add(new AlphaKeyGroup<T>(key));
                }
            }

            return list;
        }

        /// <summary>
        /// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping 
        /// using the current threads culture to determine which alpha keys to
        /// include.
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <param name="sort">Will sort the data if true.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, Func<T, string> keySelector, bool sort)
        {
            return CreateGroups(
                items,
                System.Threading.Thread.CurrentThread.CurrentCulture,
                keySelector,
                sort);
        }

        /// <summary>
        /// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="ci">The CultureInfo to group and sort by.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <param name="sort">Will sort the data if true.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, Func<T, string> keySelector, bool sort)
        {
            SortedLocaleGrouping slg = new SortedLocaleGrouping(ci);
            List<AlphaKeyGroup<T>> list = CreateDefaultGroups(slg);

            foreach (T item in items)
            {
                int index = 0;
                //if (slg.SupportsPhonetics)
                //{
                //check if your database has yomi string for item
                //if it does not, then do you want to generate Yomi or ask the user for this item.
                //index = slg.GetGroupIndex(getKey(Yomiof(item)));
                //}
                //else
                {
                    index = slg.GetGroupIndex(keySelector(item));
                }

                if (index >= 0 && index < list.Count)
                {
                    list[index].Add(item);
                }
            }

            if (sort)
            {
                foreach (AlphaKeyGroup<T> group in list)
                {
                    group.Sort((c0, c1) => { return ci.CompareInfo.Compare(keySelector(c0), keySelector(c1)); });
                }
            }

            return list;
        }
    }
}