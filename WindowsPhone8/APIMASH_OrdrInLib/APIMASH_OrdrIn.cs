using APIMASHLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Devices.Geolocation;

/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/

namespace APIMASH_OrdrInLib
{
    public delegate void NetworkingEventHandler(object sender, EventArgs e);
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    public enum ErrorCode
    {
        LoginFailed,
        RestaurantClosed,
        RestaurantLoaded,
        ListLoading,
        ListLoaded,
        OrdersLoaded,
        AccountCreated,
        PasswordReset,
        AddressesLoaded,
        OrderPlaced,
        OrderError,
        CardSaveError,
        CardSaved,
        AddressSaveError,
        AddressSaved
    }

    enum Mode
    {
        Idle,
        LoggingIn,
        LoadingAddresses,
        LoadingOrders,
        LoadingCards,
        LoadingDeliveries,
        CreateAccount,
        AddingAddress,
        DeleteAddress,
        AddingCard,
        DeleteCard,
        ResetPassword
    }

    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string Secondary { get; set; }
        public ErrorCode Code { get; set; }
    }

    public class TipClass
    {
        public string Display { get; set; }
        public float Multiplier { get; set; }
    }

    public class QuantityClass
    {
        public string Display { get; set; }
        public int Value { get; set; }
    }

    public class MonthClass
    {
        public string Display { get; set; }
        public string Value { get; set; }
    }

    public sealed class APIMASH_OrdrIn : INotifyPropertyChanged
    {
        #region Member Variables
#error Change the Ordr.In Developer Key and then remove this error
        const string APIKEY = "<ordr.in api key>";

#error Change the Bing Maps Developer Key and then remove this error
        const string BINGMAPSKEY = "<bing map key>";

        readonly APIMASHInvoke apiInvoke = new APIMASHInvoke();
        const bool useSandbox = true;
        const string testingRestaurantAddress = @"https://r-test.ordr.in/";
        const string productionRestaurantAddress = @"https://r.ordr.in/";
        const string RestaurantAddress = testingRestaurantAddress;

        const string testingOrderAddress = @"https://o-test.ordr.in/";
        const string productionOrderAddress = @"https://o.ordr.in/";
        const string OrderAddress = testingOrderAddress;

        const string testingUserAddress = @"https://u-test.ordr.in/";
        const string productionUserAddress = @"https://u.ordr.in/";
        const string UserAddress = testingUserAddress;


        #endregion Member Variables
        private float _subTotal;
        public float SubTotal 
        {
            get { return _subTotal; }
            set
            {
                _subTotal = value;
                this.OnPropertyChanged("SubTotal");
                this.OnPropertyChanged("CurrentTip");
                this.OnPropertyChanged("GrandTotal");
            }
        }
        private float _currentTip;
        public float CurrentTip
        {
            get
            {
                return _currentTip;
            }
            set
            {
                _currentTip = value;
                this.OnPropertyChanged("CurrentTip");
                this.OnPropertyChanged("GrandTotal");
            }
        }
        private float _grandTotal;
        public float GrandTotal
        {
            get
            {
                return _grandTotal;
            }
            set
            {
                _grandTotal = value;
                this.OnPropertyChanged("GrandTotal");
            }
        }

        private int _trayCount;
        public int TrayCount
        {
            get { return _trayCount; }
            set
            {
                _trayCount = value; 
                this.OnPropertyChanged("TrayCount");
            }
        }
        

        private DeliveryCheck_Bindable _deliveryCheck = new DeliveryCheck_Bindable();
        public DeliveryCheck_Bindable DeliveryCheck
        {
            get { return _deliveryCheck; }
            set { _deliveryCheck = value; }
        }


        public Restaurant_Bindable _currentRestaurant = new Restaurant_Bindable();
        public Restaurant_Bindable CurrentRestaurant
        {
            get { return _currentRestaurant; }
            set
            {
                _currentRestaurant = value;
                this.OnPropertyChanged("CurrentRestaurant");
                this.OnPropertyChanged("RestaurantSubItem");
                this.OnPropertyChanged("RestaurantCheckout");
            }
        }
        public string RestaurantSubItem
        {
            get
            {

                return CurrentRestaurant.Name.ToUpper() + " > " + CurrentMenuItem.Name.ToUpper();
            }
        }
        public string RestaurantCheckout
        {
            get
            {

                return CurrentRestaurant.Name.ToUpper() + " > CHECKOUT";
            }
        }

        private Menu_Bindable _currentMenuItem;

        public Menu_Bindable CurrentMenuItem
        {
            get { return _currentMenuItem; }
            set 
            { 
                _currentMenuItem = value;
                this.OnPropertyChanged("CurrentMenuItem");
                this.OnPropertyChanged("RestaurantSubItem");
                this.OnPropertyChanged("RestaurantCheckout");
            }
        }
        
        public Menu SelectedCategory { get; set; }

        public List<int> _countList = new List<int>();
        public List<int> CountList
        { get { return _countList; } set { _countList = value; } }

        public List<TipClass> _tipList = new List<TipClass>();
        public List<TipClass> TipList
        { get { return _tipList; } set { _tipList = value; } }

        public List<QuantityClass> _quantityList = new List<QuantityClass>();
        public List<QuantityClass> QuantityList
        { get { return _quantityList; } set { _quantityList = value; } }

        public List<MonthClass> _monthList = new List<MonthClass>();
        public List<MonthClass> MonthList
        { get { return _monthList; } set { _monthList = value; } }

        public List<MonthClass> _yearList = new List<MonthClass>();
        public List<MonthClass> YearList
        { get { return _yearList; } set { _yearList = value; } }

        public List<MonthClass> _cardTypeList = new List<MonthClass>();
        public List<MonthClass> CardTypeList
        { get { return _cardTypeList; } set { _cardTypeList = value; } }

        public GeoCoordinate position { get; set; }
        public string createError { get; set; }
        public ObservableCollection<Delivery_Bindable> Deliveries = new ObservableCollection<Delivery_Bindable>();
        public ObservableCollection<Delivery_Bindable> Pickups = new ObservableCollection<Delivery_Bindable>();

        private Delivery_Bindable _currentDelivery;

        public Delivery_Bindable CurrentDelivery
        {
            get { return _currentDelivery; }
            set 
            { 
                _currentDelivery = value;
                this.OnPropertyChanged("CurrentDelivery");
            }
        }
        public int DeliveryCount 
        { 
            get
            {
                return Deliveries.Count + Pickups.Count;
            }
        }

        private ObservableCollection<TrayItem_Bindable> _tray = new ObservableCollection<TrayItem_Bindable>();
        public ObservableCollection<TrayItem_Bindable> Tray
        {
            get { return _tray; }
            set { _tray = value; }
        }
        private ObservableCollection<Address_Bindable> _addresses = new ObservableCollection<Address_Bindable>();
        public ObservableCollection<Address_Bindable> Addresses
        {
            get { return _addresses; }
            set
            {
                _addresses = value;
                this.OnPropertyChanged("Addresses");
            }
        }

        private OrderStatus_Bindable _confirmation = new OrderStatus_Bindable();
        public OrderStatus_Bindable Confirmation
        {
            get { return _confirmation; }
            set 
            { 
                _confirmation = value;
                this.OnPropertyChanged("Confirmation");
            }
        }
        private Card_Bindable _currentCard;
        public Card_Bindable CurrentCard
        {
            get { return _currentCard; }
            set 
            { 
                _currentCard = value;
                this.OnPropertyChanged("CurrentCard");
            }
        }
        

        Card_Bindable blankCard = new Card_Bindable();
        public Card_Bindable DefaultCard 
        { 
            get
            {
                if (_cards.Count == 0)
                {
                    return blankCard;
                }
                if (_cards.Count(c => c.IsDefault == true) > 0)
                {
                    return _cards.First(c => c.IsDefault == true);
                }
                else
                {
                    return _cards.First();
                }
            }
        }
        private ObservableCollection<Card_Bindable> _cards = new ObservableCollection<Card_Bindable>();
        public ObservableCollection<Card_Bindable> Cards
        {
            get { return _cards; }
            set
            {
                _cards = value;
                this.OnPropertyChanged("Cards");
            }
        }

        private Address_Bindable _currentAddress;
        public Address_Bindable CurrentAddress
        {
            get { return _currentAddress; }
            set
            {
                _currentAddress = value;
                this.OnPropertyChanged("CurrentAddress");
            }
        }

        private Fee_Bindable _currentFee = new Fee_Bindable();
        public Fee_Bindable CurrentFee
        {
            get { return _currentFee; }
            set
            {
                _currentFee = value;
                this.OnPropertyChanged("CurrentFee");
                //GrandTotal = SubTotal + CurrentTip + float.Parse(CurrentFee.Fee) + float.Parse(CurrentFee.Tax);
            }
        }

        private ObservableCollection<Order_Bindable> _orders = new ObservableCollection<Order_Bindable>();
        public ObservableCollection<Order_Bindable> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }

        private ObservableCollection<OrderHistory_Bindable> _orderHistory = new ObservableCollection<OrderHistory_Bindable>();
        public ObservableCollection<OrderHistory_Bindable> OrderHistories
        {
            get { return _orderHistory; }
            set { _orderHistory = value; }
        }

        private ObservableCollection<Favorite_Bindable> _favorites = new ObservableCollection<Favorite_Bindable>();
        public ObservableCollection<Favorite_Bindable> Favorites
        {
            get { return _favorites; }
            set { _favorites = value; }
        }

        private ObservableCollection<Places_Bindable> _places = new ObservableCollection<Places_Bindable>();
        public ObservableCollection<Places_Bindable> Places
        {
            get { return _places; }
            set { _places = value; }
        }

        public Menu_Bindable CurrentItem { get; set; }

        public AccountObject Account { get; set; }

        public event NetworkingEventHandler OnNetworkError;
        public event ErrorEventHandler OnError;
        Mode currentMode = Mode.Idle;

        private OrderHistory_Bindable _currentOrderHistory;
        public OrderHistory_Bindable CurrentOrderHistory
        {
            get { return _currentOrderHistory; }
            set { _currentOrderHistory = value; }
        }
    
        public APIMASH_OrdrIn()
        {
            apiInvoke.OnResponse += apiInvoke_OnResponse;    

            position = new GeoCoordinate(47.6097, -122.3331);
            for (int x = 1; x <= 15; x++)
            {
                CountList.Add(x);
            }

            TipList.Add(new TipClass() { Display = "5%", Multiplier = 0.05f });
            TipList.Add(new TipClass() { Display = "10%", Multiplier = 0.10f });
            TipList.Add(new TipClass() { Display = "15%", Multiplier = 0.15f });
            TipList.Add(new TipClass() { Display = "20%", Multiplier = 0.20f });

            QuantityList.Add(new QuantityClass() { Display = "1", Value = 1 });
            QuantityList.Add(new QuantityClass() { Display = "2", Value = 2 });
            QuantityList.Add(new QuantityClass() { Display = "3", Value = 3 });
            QuantityList.Add(new QuantityClass() { Display = "4", Value = 4 });
            QuantityList.Add(new QuantityClass() { Display = "5", Value = 5 });
            QuantityList.Add(new QuantityClass() { Display = "6", Value = 6 });
            QuantityList.Add(new QuantityClass() { Display = "7", Value = 7 });
            QuantityList.Add(new QuantityClass() { Display = "8", Value = 8 });
            QuantityList.Add(new QuantityClass() { Display = "9", Value = 9 });
            QuantityList.Add(new QuantityClass() { Display = "10", Value = 10 });

            MonthList.Add(new MonthClass() { Display = "Expire Month", Value = "00" });
            MonthList.Add(new MonthClass() { Display = "January", Value = "01" });
            MonthList.Add(new MonthClass() { Display = "February", Value = "02" });
            MonthList.Add(new MonthClass() { Display = "March", Value = "03" });
            MonthList.Add(new MonthClass() { Display = "April", Value = "04" });
            MonthList.Add(new MonthClass() { Display = "May", Value = "05" });
            MonthList.Add(new MonthClass() { Display = "June", Value = "06" });
            MonthList.Add(new MonthClass() { Display = "July", Value = "07" });
            MonthList.Add(new MonthClass() { Display = "August", Value = "08" });
            MonthList.Add(new MonthClass() { Display = "September", Value = "09" });
            MonthList.Add(new MonthClass() { Display = "October", Value = "10" });
            MonthList.Add(new MonthClass() { Display = "November", Value = "11" });
            MonthList.Add(new MonthClass() { Display = "December", Value = "12" });

            YearList.Add(new MonthClass() { Display = "Expire Year", Value = "00" });
            DateTime now = DateTime.Now;
            for (int i = 0; i < 20; i++)
            {
                YearList.Add(new MonthClass() { Display = now.AddYears(i).Year.ToString(), Value = now.AddYears(i).Year.ToString() });                
            }

            CardTypeList.Add(new MonthClass() { Display = "Visa", Value = "Visa" });
            CardTypeList.Add(new MonthClass() { Display = "Mastercard", Value = "Mastercard" });
            CardTypeList.Add(new MonthClass() { Display = "American Express", Value = "American Express" });

        }

        public async void LoadPosition()
        {
            try
            {
                Geolocator geolocator = new Geolocator();
                Geoposition geoposition = await geolocator.GetGeopositionAsync();
                if(geoposition != null && geoposition.Coordinate != null)
                    position = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
            }
            catch (Exception ex)
            {
                position = null;
            }
        }

        void apiInvoke_OnResponse(object sender, APIMASHEvent e)
        {
            try
            {
                if (e.Status != APIMASHStatus.SUCCESS)
                {
                    switch(currentMode)
                    {
                        case Mode.LoggingIn:
                            SendLoginError();
                            break;
                        case Mode.LoadingAddresses:
                            break;
                        case Mode.LoadingOrders:
                            break;
                        case Mode.LoadingDeliveries:
                            break;
                        case Mode.CreateAccount:
                            break;
                    }
                }
                currentMode = Mode.Idle;
                if (e.Object as RestaurantObject != null)
                {
                    var rest = e.Object as RestaurantObject;
                    if (CurrentRestaurant == null || rest.restaurant_id != CurrentRestaurant.Id)
                    {
                        if (CurrentRestaurant == null)
                            CurrentRestaurant = new Restaurant_Bindable();
                        rest.Copy(CurrentRestaurant);
                        Tray.Clear();
                        TrayCount = 0;
                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.RestaurantLoaded, Message = "" });
                        LoadDeliveryCheck(int.Parse(CurrentRestaurant.Id));
                    }
                }
                else if (e.Object as DeliveryCheckObject != null)
                {
                    var check = e.Object as DeliveryCheckObject;
                    check.Copy(DeliveryCheck);

                    string location = string.Format("http://dev.virtualearth.net/REST/v1/Locations/US/{0}/{1}/{2}/{3}?o=json&key={4}", CurrentRestaurant.State, CurrentRestaurant.Zip, CurrentRestaurant.City, CurrentRestaurant.Address, BINGMAPSKEY);
                    apiInvoke.Invoke<LocationObject>(location);
                }
                else if (e.Object as DeleteObject != null)
                {
                    var delMessage = e.Object as DeleteObject;
                    Addresses.Clear();
                    LoadAddresses();
                }
                else if (e.Object as DeleteCardObject != null)
                {
                    var delMessage = e.Object as DeleteCardObject;
                    Cards.Clear();
                    LoadCards();
                }
                else if (e.Object as FeeObject != null)
                {
                    var fee = e.Object as FeeObject;
                    fee.Copy(CurrentFee);
                    if (CurrentFee.CanService == 0)
                    {
                        SendRestaurantClosedError(CurrentFee.Message);
                    }
                    else
                    {
                        GrandTotal = SubTotal + CurrentTip + float.Parse(CurrentFee.Fee) + float.Parse(CurrentFee.Tax);
                        this.OnPropertyChanged("GrandTotal");
                    }
                }
                else if (e.Object as CreateCardObject != null)
                {
                    var createCard = e.Object as CreateCardObject;
                    if (createCard._err == "0")
                    {
                        LoadCards();
                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.CardSaved, Message = "" });
                    }
                    else
                    {
                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.CardSaveError, Message = "There was an error saving your credit card information.  Check the data and try again." });
                    }
                }
                else if (e.Object as AccountObject != null)
                {
                    Account = e.Object as AccountObject;
                    if (Account.em != null)
                        LoadAddresses();
                    else
                    {
                        SendLoginError();
                    }
                }
                else if (e.Object as PasswordResetObject != null)
                {
                    var reset = e.Object as PasswordResetObject;
                    SendResetMessage();
                }
                else if (e.Object as List<OrderHistory> != null)
                {
                    var history = e.Object as List<OrderHistory>;

                    OrderHistories.Clear();
                    foreach (var item in history)
                    {
                        var newHistory = new OrderHistory_Bindable();
                        item.Copy(newHistory);
                        OrderHistories.Add(newHistory);
                    }

                    if (OnError != null)
                        OnError(null, new ErrorEventArgs() { Code = ErrorCode.OrdersLoaded, Message = "" });
                }
                else if (e.Object as CreateAddressObject != null)
                {
                    var address = e.Object as CreateAddressObject;
                    if (address._error != "1")
                    {
                        LoadAddresses();
                        
                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.AddressSaved, Message = "" });
                    }
                    else
                    {
                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.AddressSaveError, Message = "There was an error saving your address information.  Check the data and try again." });
                    }
                }
                else if (e.Object as List<OrdrAddress> != null)
                {
                    var list = e.Object as List<OrdrAddress>;
                    if (Account.em != null)
                    {
                        foreach (var item in list)
                        {
                            var work = new Address_Bindable();
                            item.Copy(work);
                            if (Addresses.FirstOrDefault(a => a.Nick == work.Nick) == null)
                                Addresses.Add(work);
                        }

                        var address = Addresses.First(a => a.IsDefault == 1);
                        CurrentAddress = address;

                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.AddressesLoaded });

                        LoadDeliveryList(CurrentAddress.Zip, CurrentAddress.City, CurrentAddress.Address, "ASAP");
                        LoadCards();
                    }
                }
                else if (e.Object as List<CardObject> != null)
                {
                    var list = e.Object as List<CardObject>;
                    Cards.Clear();
                    foreach (var item in list)
                    {
                        var work = new Card_Bindable();
                        item.Copy(work);
                        Cards.Add(work);
                    }
                    if (Cards.Count > 0)
                    {
                        Cards.First().IsDefault = true;
                        ChangeDefaultCard();
                    }

                    LoadOrders();
                }
                else if (e.Object as LocationObject != null)
                {
                    if (CurrentRestaurant != null)
                    {
                        var location = e.Object as LocationObject;
                        var coord = location.resourceSets.First().resources.First().geocodePoints.First().coordinates;
                        CurrentRestaurant.Position = new GeoCoordinate(coord[0], coord[1]);
                    }
                    if (loadingDeliveryList == true)
                    {
                        var location = e.Object as LocationObject;
                        var ad = location.resourceSets.First().resources.First().address;

                        var work = new Address_Bindable()
                        {
                            Zip = ad.postalCode,
                            City = ad.locality,
                            State = ad.adminDistrict,
                            Address = ad.addressLine
                        };
                        CurrentAddress = work;

                        string apiCall = RestaurantAddress + @"dl/" + "ASAP" + "/" + ad.postalCode + "/" + ad.locality + "/" + ad.addressLine + "?_auth=1," + APIKEY;
                        currentMode = Mode.LoadingDeliveries;
                        apiInvoke.Invoke<List<DeliveryObject>>(apiCall);
                    }
                    loadingDeliveryList = false;
                }
                else if (e.Object as UserCreateObject != null)
                {
                    var userCreate = e.Object as UserCreateObject;
                    if (userCreate._err != 0)
                    {
                        createError = userCreate.msg;
                    }
                    else
                    {
                        Addresses.Clear();
                        LoadAccount(tempemail, temppass);

                        if (OnError != null)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.AccountCreated, Message = tempemail, Secondary = temppass });
                    }
                }
                else if (e.Object as OrderStatusObject != null)
                {
                    var confirm = e.Object as OrderStatusObject;
                    confirm.Copy(Confirmation);

                    if (OnError != null)
                    {
                        if(confirm._err == 0 && confirm._error == 0)
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.OrderPlaced, Message = "" });
                        else
                            OnError(null, new ErrorEventArgs() { Code = ErrorCode.OrderError, Message = Confirmation.Message });
                    }
                }
                else if (e.Object as List<DeliveryObject> != null)
                {
                    Deliveries.Clear();
                    Pickups.Clear();

                    if (Account == null || Account.em == null)
                        return;

                    var list = e.Object as List<DeliveryObject>;

                    foreach (var item in list)
                    {
                        var del = new Delivery_Bindable();
                        item.Copy(del);
                        if (del.CanDeliver)
                        {
                            if(Deliveries.Contains(del) == false)
                                Deliveries.Add(del);
                        }
                        if (del.CanPickup)
                        {
                            if(Pickups.Contains(del) == false)
                                Pickups.Add(del);
                        }
                    }
                    OnPropertyChanged("DeliveryCount");
                    
                    if (OnError != null)
                        OnError(null, new ErrorEventArgs() { Code = ErrorCode.ListLoaded, Message = "" });
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(null, new ErrorEventArgs() { Code = ErrorCode.ListLoaded, Message = "" });

            }
        }
        private void ChangeDefaultCard()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("card.default", FileMode.Open))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        var name = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);
                        var card = Cards.FirstOrDefault(a => a.Nick == name);
                        if (card != null)
                        {
                            foreach (var item in this.Cards)
                            {
                                item.IsDefault = false;
                            }
                            card.IsDefault = true;
                        }
                    }
                }
            }
            catch
            {
                //add some code here
            }
        }

        void SendRestaurantClosedError(string message)
        {
            if (OnError != null)
                OnError(null, new ErrorEventArgs() { Code = ErrorCode.RestaurantClosed, Message = message });
        }

        void SendLoginError()
        {
            if (OnError != null)
                OnError(null, new ErrorEventArgs() { Code = ErrorCode.LoginFailed, Message = "Account could not be logged in.  Please check your email address and password as well as your internet connection." });
        }

        void SendResetMessage()
        {
            if (OnError != null)
                OnError(null, new ErrorEventArgs() { Code = ErrorCode.PasswordReset, Message = "Account password reset email has been sent.  Please check your email for the link to reset your password." });
        }

        async private void LoadCards()
        {
            string email = Account.em;
            string password = Account.pw;
            string hexpass = password + email + "/u/" + email + "/ccs";
            SHA256Managed sh = new SHA256Managed();

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            var rnd = new Random();
            string apiCall = UserAddress + @"u/" + email + "/ccs?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY + "&param=" + rnd.Next();

            currentMode = Mode.LoadingCards;
            apiInvoke.Invoke<List<CardObject>>(apiCall);
        }


        async private void LoadAddresses()
        {
            string email = Account.em;
            string password = Account.pw;
            string hexpass = password + email + "/u/" + email + "/addrs";
            SHA256Managed sh = new SHA256Managed();

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            var rnd = new Random();
            string apiCall = UserAddress + @"u/" + email + "/addrs?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY + "&param=" + rnd.Next();

            currentMode = Mode.LoadingAddresses;
            apiInvoke.Invoke<List<OrdrAddress>>(apiCall);
        }

        public void AddMenuItem(Menu_Bindable item, int quantity, List<Menu_Bindable> subitems)
        {
            var search = Tray.Where(t => t.ItemId == item.Id);
            bool match = true;
            if (search.Count() != 0)
            {
                foreach (var sitem in search)
                {
                    match = true;
                    if (sitem.SubItems.Count != subitems.Count)
                        match = false;
                    foreach (var subItem in sitem.SubItems)
                    {
                        if (subitems.Count(o => o.Id == subItem.ItemId) == 0)
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match == true)
                    {
                        sitem.Quantity+=quantity;
                        break;
                    }
                }
            }
            else
            {
                match = false;
            }
            if(match == false)
            {
                var trayItem = new TrayItem_Bindable()
                {
                    ItemId = item.Id,
                    Quantity = quantity,
                    Name = item.Name,
                    Price = item.Price,
                    MenuItem = item,
                    QuantityList = QuantityList
                };
                foreach (var subitem in subitems)
                {
                    var option = new TrayItem_Bindable() 
                    {
                        ItemId = subitem.Id,
                        Quantity = 1,
                        Name = subitem.Name,
                        Price = subitem.Price,
                        MenuItem = subitem,
                        QuantityList = QuantityList
                    };
                    trayItem.SubItems.Add(option);
                }
                Tray.Add(trayItem);
            }
            TrayCount += quantity;
        }

        public void UpdateTrayCount()
        {
            TrayCount = 0;
            foreach (var item in Tray)
            {
                TrayCount += item.Quantity;
            }
        }

        public void Signout()
        {
            Addresses.Clear();
            tempemail = string.Empty;
            temppass = string.Empty;
            this.CurrentRestaurant = null;
            Deliveries.Clear();
            Pickups.Clear();
            OrderHistories.Clear();

            if (OnError != null)
                OnError(null, new ErrorEventArgs() { Code = ErrorCode.ListLoaded, Message = "" });
        }

        string tempemail;
        string temppass;

        public void ResetPassword(string email)
        {
            //string email = Account.em;
            //string password = Account.pw;

            string apiCall = UserAddress + @"u/" + email + "/password_reset" + "?_auth=1," + APIKEY;

            currentMode = Mode.ResetPassword;
            apiInvoke.Invoke<CreateAddressObject>(apiCall, new[]
                {
                    new KeyValuePair<string, string>("email", email)}
                    , "POST");
        }

        public void ChangePassword(string email, string oldPassword,  string newPassword)
        {
        }


        public void LoadOrders()
        {
            string email = tempemail;
            string password = temppass;

            SHA256Managed sh = new SHA256Managed();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] result = sh.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append(item.ToString("x2"));
            }

            string hexpass = sb.ToString() + email + "/u/" + Uri.EscapeUriString(email) + "/orders";

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();

            currentMode = Mode.LoadingOrders;

            string apiCall = UserAddress + @"u/" + Uri.EscapeUriString(email) + "/orders" + "?_uauth=1," + Uri.EscapeUriString(email) + "," + hashkey + "&_auth=1," + APIKEY;
            apiInvoke.Invoke<List<OrderHistory>>(apiCall);
        }


        public void LoadAccount(string email, string password)
        {
            tempemail = email;
            temppass = password;

            SHA256Managed sh = new SHA256Managed();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] result = sh.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append(item.ToString("x2"));
            }
            string hexpass = sb.ToString() + email + "/u/" + Uri.EscapeUriString(email);

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            var rnd = new Random();

            string apiCall = UserAddress + @"u/" + Uri.EscapeUriString(email) + "?_uauth=1," + Uri.EscapeUriString(email) + "," + hashkey + "&_auth=1," + APIKEY + "&param=" + rnd.Next();
            currentMode = Mode.LoggingIn;
            apiInvoke.Invoke<AccountObject>(apiCall);
        }

        public void CreateAccount(string fname, string lname, string email, string password)
        {
            tempemail = email;
            temppass = password;

            string apiCall = UserAddress + @"u/" + Uri.EscapeUriString(email) + "?_auth=1," + APIKEY;

            SHA256Managed sh = new SHA256Managed();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] result = sh.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append(item.ToString("x2"));
            }
            string hexpass = sb.ToString();

            currentMode = Mode.CreateAccount;
            apiInvoke.Invoke<UserCreateObject>(apiCall, new[]
                {
                    new KeyValuePair<string, string>("pw", hexpass),
                    new KeyValuePair<string, string>("first_name", fname),
                    new KeyValuePair<string, string>("last_name", lname)
                });
        }


        public void AddCard(string nick, string name, string number, string cvc, string month, string year, string cctype, string addr1, string addr2, string city, string state, string zip, string phone)
        {
            string email = Account.em;
            string password = Account.pw;

            SHA256Managed sh = new SHA256Managed();
            string hexpass = password + email + "/u/" + email + "/ccs/" + nick;

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            string apiCall = UserAddress + @"u/" + email + "/ccs/" + nick + "?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY;

            currentMode = Mode.AddingCard;
            apiInvoke.Invoke<CreateCardObject>(apiCall, new[]
                {
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("number", number),
                    new KeyValuePair<string, string>("cvc", cvc),
                    new KeyValuePair<string, string>("expiry_month", month),
                    new KeyValuePair<string, string>("expiry_year", year),
                    new KeyValuePair<string, string>("type", cctype),
                    new KeyValuePair<string, string>("bill_addr", addr1),
                    new KeyValuePair<string, string>("bill_addr2", addr2),
                    new KeyValuePair<string, string>("bill_city", city),
                    new KeyValuePair<string, string>("bill_state", state),
                    new KeyValuePair<string, string>("bill_zip", zip),
                    new KeyValuePair<string, string>("bill_phone", phone)
                }, "PUT");
        }

        public void DeleteCard(string nick)
        {
            string email = Account.em;
            string password = Account.pw;

            SHA256Managed sh = new SHA256Managed();
            string hexpass = password + email + "/u/" + email + "/ccs/" + nick;

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            string apiCall = UserAddress + @"u/" + email + "/ccs/" + nick + "?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY;

            currentMode = Mode.LoggingIn;
            apiInvoke.Invoke<DeleteCardObject>(apiCall, null, "DELETE");
        }

        public void DeleteAddress(string nick)
        {
            string email = Account.em;
            string password = Account.pw;

            SHA256Managed sh = new SHA256Managed();
            string hexpass = password + email + "/u/" + email + "/addrs/" + nick;

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            string apiCall = UserAddress + @"u/" + email + "/addrs/" + nick + "?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY;

            currentMode = Mode.LoggingIn;
            apiInvoke.Invoke<DeleteObject>(apiCall, null, "DELETE");
        }

        public void AddAddress(string nick, string addr1, string addr2, string city, string state, string zip, string phone)
        {
            try
            {
                string email = Account.em;
                string password = Account.pw;

                SHA256Managed sh = new SHA256Managed();
                string hexpass = password + email + "/u/" + email + "/addrs/" + nick;

                var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
                byte[] hashresult = sh.ComputeHash(hashbytes);
                StringBuilder sb2 = new StringBuilder();
                foreach (var item in hashresult)
                {
                    sb2.Append(item.ToString("x2"));
                }
                string hashkey = sb2.ToString();
                string apiCall = UserAddress + @"u/" + email + "/addrs/" + nick + "?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY;

                currentMode = Mode.LoggingIn;
                apiInvoke.Invoke<CreateAddressObject>(apiCall, new[]
                {
                    new KeyValuePair<string, string>("addr", addr1),
                    new KeyValuePair<string, string>("addr2", addr2),
                    new KeyValuePair<string, string>("city", city),
                    new KeyValuePair<string, string>("state", state),
                    new KeyValuePair<string, string>("zip", zip),
                    new KeyValuePair<string, string>("phone", phone)
                }, "PUT");

                var oldAddress = Addresses.FirstOrDefault(a => a.Nick == nick);
                if (oldAddress == null)
                    Addresses.Add(new Address_Bindable() { Address = addr1, Address2 = addr2, City = city, State = state, Phone = phone, Zip = zip, IsDefault = 1, Nick = nick });
                else
                {
                    oldAddress.Address = addr1;
                    oldAddress.Address2 = addr2;
                    oldAddress.City = city;
                    oldAddress.State = state;
                    oldAddress.Phone = phone;
                    oldAddress.Zip = zip;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool loadingDeliveryList = false;
        public void LoadDeliveryList(string zip = null, string city = null, string address = null, string deliveryTime = "ASAP", double longitude=0.0, double latitude=0.0)
        {
            loadingDeliveryList = true;
            
            Deliveries.Clear();
            Pickups.Clear();

            if (OnError != null)
                OnError(null, new ErrorEventArgs() { Code = ErrorCode.ListLoading, Message = "" });

            string apiCall;
            if (zip == null)
            {
                string location = string.Format("http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?o=json&key={2}", latitude, longitude, BINGMAPSKEY);
                apiInvoke.Invoke<LocationObject>(location);
                //apiCall = RestaurantAddress + @"dl/ASAP/77840/College Station/1 Main Street";
            }
            else
            {
                apiCall = RestaurantAddress + @"dl/" + deliveryTime + "/" + zip + "/" + Uri.EscapeUriString(city) + "/" + Uri.EscapeUriString(address) + "?_auth=1," + APIKEY;
                apiInvoke.Invoke<List<DeliveryObject>>(apiCall);
            }
        }

        public void GetFullDeliveryAddress(string zip)
        {
                string location = string.Format("http://dev.virtualearth.net/REST/v1/Locations?postalCode={0}&o=json&key={1}", zip, BINGMAPSKEY);
                apiInvoke.Invoke<LocationObject>(location);
        }

        public void LoadRestaurantPosition(int id)
        {
        }

        private void LoadDeliveryCheck(int id)
        {
            loadingDeliveryList = false;
            string apiCall = RestaurantAddress + @"dc/" + id + "/ASAP/" + CurrentAddress.Zip + "/" + Uri.EscapeUriString(CurrentAddress.City) + "/" + Uri.EscapeUriString(CurrentAddress.Address) + "?_auth=1," + APIKEY;
            apiInvoke.Invoke<DeliveryCheckObject>(apiCall);
        }

        public void LoadDeliveryFee(int id, float subtotal, float tip)
        {
            GrandTotal = subtotal + tip;
            string apiCall = RestaurantAddress + @"fee/" + id + "/" + subtotal.ToString("00.00") + "/" + tip.ToString("00.00") + "/ASAP/" + CurrentAddress.Zip + "/" + Uri.EscapeUriString(CurrentAddress.City) + "/" + Uri.EscapeUriString(CurrentAddress.Address) + "?_auth=1," + APIKEY;
            apiInvoke.Invoke<FeeObject>(apiCall);
        }

        public void PlaceOrder()
        {
            StringBuilder tray = new StringBuilder();
            foreach (var item in Tray)
            {
                if (item != Tray.First())
                    tray.Append("+");
                tray.Append(item.ItemId + "/" + item.Quantity);
                foreach (var subitem in item.SubItems)
                {
                    tray.Append("," + subitem.ItemId);
                }
            }

            string email = Account.em;
            string password = Account.pw;

            SHA256Managed sh = new SHA256Managed();

            string hexpass = password + email + "/o/" + CurrentRestaurant.Id;

            var hashbytes = System.Text.Encoding.UTF8.GetBytes(hexpass);
            byte[] hashresult = sh.ComputeHash(hashbytes);
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in hashresult)
            {
                sb2.Append(item.ToString("x2"));
            }
            string hashkey = sb2.ToString();
            string apiCall = OrderAddress + @"o/" + CurrentRestaurant.Id + "?_uauth=1," + email + "," + hashkey + "&_auth=1," + APIKEY;

            if (CurrentCard.FullLast5.Contains("****") == true)
            {
                apiInvoke.Invoke<OrderStatusObject>(apiCall, new[]
                {
                    new KeyValuePair<string, string>("rid", CurrentRestaurant.Id),
                    new KeyValuePair<string, string>("tray", tray.ToString()),
                    new KeyValuePair<string, string>("tip", CurrentTip.ToString()),
                    new KeyValuePair<string, string>("delivery_date", "ASAP"),
                    new KeyValuePair<string, string>("delivery_time", "ASAP"),
                    new KeyValuePair<string, string>("first_name", Account.first_name),
                    new KeyValuePair<string, string>("last_name", Account.last_name),
                    new KeyValuePair<string, string>("addr", CurrentAddress.Address),
                    new KeyValuePair<string, string>("addr2", CurrentAddress.Address2),
                    new KeyValuePair<string, string>("city", CurrentAddress.City),
                    new KeyValuePair<string, string>("state", CurrentAddress.State),
                    new KeyValuePair<string, string>("zip", CurrentAddress.Zip),
                    new KeyValuePair<string, string>("phone", CurrentAddress.Phone),
                    new KeyValuePair<string, string>("em", Account.em),
                    new KeyValuePair<string, string>("password", Account.pw),
                    new KeyValuePair<string, string>("card_nick", CurrentCard.Nick),
                });
            }
            else
            {
                apiInvoke.Invoke<OrderStatusObject>(apiCall, new[]
                {
                    new KeyValuePair<string, string>("rid", CurrentRestaurant.Id),
                    new KeyValuePair<string, string>("tray", tray.ToString()),
                    new KeyValuePair<string, string>("tip", CurrentTip.ToString()),
                    new KeyValuePair<string, string>("delivery_date", "ASAP"),
                    new KeyValuePair<string, string>("delivery_time", "ASAP"),
                    new KeyValuePair<string, string>("first_name", Account.first_name),
                    new KeyValuePair<string, string>("last_name", Account.last_name),
                    new KeyValuePair<string, string>("addr", CurrentAddress.Address),
                    new KeyValuePair<string, string>("addr2", CurrentAddress.Address2),
                    new KeyValuePair<string, string>("city", CurrentAddress.City),
                    new KeyValuePair<string, string>("state", CurrentAddress.State),
                    new KeyValuePair<string, string>("zip", CurrentAddress.Zip),
                    new KeyValuePair<string, string>("phone", CurrentAddress.Phone),
                    new KeyValuePair<string, string>("em", Account.em),
                    new KeyValuePair<string, string>("password", Account.pw),
                    new KeyValuePair<string, string>("card_name", CurrentCard.Nick),
                    new KeyValuePair<string, string>("card_number", CurrentCard.FullLast5),
                    new KeyValuePair<string, string>("card_cvc", CurrentCard.Security),
                    new KeyValuePair<string, string>("card_expiry", CurrentCard.Expire),
                    new KeyValuePair<string, string>("card_bill_addr", CurrentCard.Address),
                    new KeyValuePair<string, string>("card_bill_addr2", ""),
                    new KeyValuePair<string, string>("card_bill_city", CurrentCard.City),
                    new KeyValuePair<string, string>("card_bill_state", CurrentCard.State),
                    new KeyValuePair<string, string>("card_bill_zip", CurrentCard.Zip),
                    new KeyValuePair<string, string>("card_bill_phone", CurrentAddress.Phone),
                });
            }
        }

        public void LoadRestaurant(int id)
        {
            CurrentDelivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if(CurrentDelivery == null)
                CurrentDelivery = Pickups.FirstOrDefault(d => d.Id == id);

            string apiCall = RestaurantAddress + @"rd/" + id + "?_auth=1," + APIKEY;
            apiInvoke.Invoke<RestaurantObject>(apiCall);

        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
