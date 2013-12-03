using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/

namespace APIMASH_OrdrInLib
{



    #region Bindable Base Class
    [Windows.Foundation.Metadata.WebHostHidden]
    public class BindableBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            if (propertyName != null) this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    #endregion Bindable Base Class


    public class OrderHistory_Bindable : BindableBase
    {
        private List<Item_Bindable> _trayItems;
        public List<Item_Bindable> TrayItems
        {
            get { return _trayItems; }
            set { SetProperty<List<Item_Bindable>>(ref _trayItems, value); }
        }

        private object _orderId;
        public object OrderId
        {
            get { return _orderId; }
            set { SetProperty<object>(ref _orderId, value); }
        }
        private string _total;
        public string Total
        {
            get { return _total; }
            set { SetProperty<string>(ref _total, value); }
        }
        private string _tip;
        public string Tip
        {
            get { return _tip; }
            set { SetProperty<string>(ref _tip, value); }
        }
        private string _tax;
        public string Tax
        {
            get { return _tax; }
            set { SetProperty<string>(ref _tax, value); }
        }
        private string _fee;
        public string Fee
        {
            get { return _fee; }
            set { SetProperty<string>(ref _fee, value); }
        }
        private string _subtotal;
        public string SubTotal
        {
            get { return _subtotal; }
            set { SetProperty<string>(ref _subtotal, value); }
        }
        private DateTime _orderTime;
        public DateTime OrderTime
        {
            get { return _orderTime; }
            set { SetProperty<DateTime>(ref _orderTime, value); }
        }
        private string _restaurantId;
        public string RestaurantId
        {
            get { return _restaurantId; }
            set { SetProperty<string>(ref _restaurantId, value); }
        }
        private string _restaurantName;
        public string RestaurantName
        {
            get { return _restaurantName; }
            set { SetProperty<string>(ref _restaurantName, value); }
        }
    }

    public class Item_Bindable : BindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty<int>(ref _id, value); }
        }
        private string _price;
        public string Price
        {
            get { return _price; }
            set { SetProperty<string>(ref _price, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value); }
        }
        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set { SetProperty<string>(ref _quantity, value); }
        }
        public Opt[] opts { get; set; }
        private List<Option_Bindable> _options;
        public List<Option_Bindable> Options
        {
            get { return _options; }
            set { SetProperty<List<Option_Bindable>>(ref _options, value); }
        }

    }

    public class Option_Bindable : BindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty<int>(ref _id, value); }
        }
        private string _price;
        public string Price
        {
            get { return _price; }
            set { SetProperty<string>(ref _price, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value); }
        }
        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set { SetProperty<string>(ref _quantity, value); }
        }
    }


    public class Card_Bindable : BindableBase
    {
        private string _nick;
        public string Nick
        {
            get { return _nick; }
            set { SetProperty<string>(ref _nick, value); }
        }

        private string _addr;
        public string Address
        {
            get { return _addr; }
            set { SetProperty<string>(ref _addr, value); }
        }
        private string _addr2;
        public string Address2
        {
            get { return _addr2; }
            set { SetProperty<string>(ref _addr2, value); }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty<string>(ref _phone, value); }
        }
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                SetProperty<string>(ref _city, value);
                OnPropertyChanged("CityState");
                OnPropertyChanged("CityStateZip");
            }
        }
        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                SetProperty<string>(ref _state, value);
                OnPropertyChanged("CityState");
                OnPropertyChanged("CityStateZip");
            }
        }
        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set
            {
                SetProperty<string>(ref _zip, value);
                OnPropertyChanged("CityStateZip");
            }
        }
        public string CityState
        {
            get
            {
                return City + ", " + State;
            }
        }
        public string CityStateZip
        {
            get
            {
                return City + ", " + State + " " + Zip;
            }
        }
        private string _fullLast5="";
        public string FullLast5
        {
            get 
            {
                if (_fullLast5 != null && _fullLast5.Length <= 5 && _last5 != null && _last5 != "")
                    _fullLast5 = "****-****-****-" + _last5;
                return _fullLast5; 
            }
            set
            {
                SetProperty<string>(ref _fullLast5, value);
            }
        }

        private string _security;
        public string Security
        {
            get { return _security; }
            set
            {
                SetProperty<string>(ref _security, value);
            }
        }

        private string _last5;
        public string Last5
        {
            get { return _last5; }
            set
            {
                SetProperty<string>(ref _last5, value);
            }
        }
        private string _type;
        public string CardType
        {
            get { return _type; }
            set
            {
                SetProperty<string>(ref _type, value);
            }
        }
        private string _token;
        public string CardToken
        {
            get { return _token; }
            set
            {
                SetProperty<string>(ref _token, value);
            }
        }

        private string _expire="";
        public string Expire
        {
            get 
            {
                if (_expire.Length == 0 && _month.Length != 0 && _year.Length != 0)
                {
                    _expire = _month + "-" + _year;
                }
                return _expire; 
            }
            set
            {
                SetProperty<string>(ref _expire, value);
            }
        }

        private string _year="";
        public string CardYear
        {
            get { return _year; }
            set
            {
                SetProperty<string>(ref _year, value);
            }
        }
        private string _month="";
        public string CardMonth
        {
            get { return _month; }
            set
            {
                SetProperty<string>(ref _month, value);
            }
        }
        private bool _selected;
        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                SetProperty<bool>(ref _selected, value);
            }
        }
        private bool _default;
        public bool IsDefault
        {
            get { return _default; }
            set
            {
                SetProperty<bool>(ref _default, value);
            }
        }
    }




    public class Address_Bindable : BindableBase
    {
        private string _nick;
        public string Nick
        {
            get { return _nick; }
            set { SetProperty<string>(ref _nick, value); }
        }

        private string _addr;
        public string Address
        {
            get { return _addr; }
            set { SetProperty<string>(ref _addr, value); }
        }
        private string _addr2;
        public string Address2
        {
            get { return _addr2; }
            set { SetProperty<string>(ref _addr2, value); }
        }
        private string _city;
        public string City
        {
            get { return _city; }
            set 
            { 
                SetProperty<string>(ref _city, value);
                OnPropertyChanged("CityState");
                OnPropertyChanged("CityStateZip");
            }
        }
        private string _state;
        public string State
        {
            get { return _state; }
            set 
            { 
                SetProperty<string>(ref _state, value);
                OnPropertyChanged("CityState");
                OnPropertyChanged("CityStateZip");
            }
        }
        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set 
            { 
                SetProperty<string>(ref _zip, value);
                OnPropertyChanged("CityStateZip");
            }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty<string>(ref _phone, value); }
        }
        private int _default;
        public int IsDefault
        {
            get { return _default; }
            set { SetProperty<int>(ref _default, value); }
        }
        public string CityState
        {
            get
            {
                return City + ", " + State;
            }
        }
        public string CityStateZip
        {
            get
            {
                return City + ", " + State + " " + Zip;
            }
        }
        private bool _selected;
        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                SetProperty<bool>(ref _selected, value);
            }
        }
    }

    public class Order_Bindable : BindableBase
    {
    }
    
    public class Favorite_Bindable : BindableBase
    {
    }

    public class Places_Bindable : BindableBase
    {
    }

    public class TrayItem_Bindable : BindableBase
    {
        public List<QuantityClass> QuantityList { get; set; }

        private string _itemId;
        public string ItemId
        {
            get { return _itemId; }
            set { SetProperty<string>(ref _itemId, value); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set 
            { 
                SetProperty<int>(ref _quantity, value);
                OnPropertyChanged("SubTotal");
            }
        }

        private List<TrayItem_Bindable> _subItems = new List<TrayItem_Bindable>();
        public List<TrayItem_Bindable> SubItems
        {
            get { return _subItems; }
            set { SetProperty<List<TrayItem_Bindable>>(ref _subItems, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value); }
        }

        private string _price;
        public string Price
        {
            get { return _price; }
            set 
            { 
                SetProperty<string>(ref _price, value);
                OnPropertyChanged("SubTotal");
            }
        }

        public float SubTotal
        {
            get
            {
                float optionPrice=0.0f;
                foreach (var item in SubItems)
                {
                    optionPrice += float.Parse(item.Price);
                }
                float total = Quantity * (float.Parse(Price) + optionPrice);
                return total;
            }
        }

        private Menu_Bindable _menuItem;
        public Menu_Bindable MenuItem
        {
            get { return _menuItem; }
            set { SetProperty<Menu_Bindable>(ref _menuItem, value); }
        }
    }

    public class Restaurant_Bindable : BindableBase
    {
        public Restaurant_Bindable()
        {
            this.Cuisine.CollectionChanged += Cuisine_CollectionChanged;
        }

        void Cuisine_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("FoodTypes");
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set 
            { 
                SetProperty<string>(ref _address, value);
                this.OnPropertyChanged("CityStateZip");
            }
        }

        public string CityStateZip
        {
            get
            {
                return Address + ", " + City + ", " + State;
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set 
            { 
                SetProperty<string>(ref _city, value);
                this.OnPropertyChanged("CityStateZip");
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty<string>(ref _phone, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                SetProperty<string>(ref _name, value);
                this.OnPropertyChanged("NameUpper");
            }
        }

        public string NameUpper 
        { 
            get
            {
                if (Name != null)
                    return Name.ToUpper();
                else
                    return null;
            }
        }

        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set 
            { 
                SetProperty<string>(ref _zip, value);
            }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty<string>(ref _id, value); }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set 
            { 
                SetProperty<string>(ref _state, value);
                this.OnPropertyChanged("CityStateZip");
            }
        }

        private ObservableCollection<string> _cuisine = new ObservableCollection<string>();
        public ObservableCollection<string> Cuisine
        {
            get { return _cuisine; }
            set { _cuisine = value; }
        }
        public string FoodTypes
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                var list = Cuisine.Take(2);
                foreach (var item in list)
                {
                    sb.Append(item);
                    if (item != list.Last())
                        sb.Append(", ");
                }
                return sb.ToString(); ; 
            }
        }
        //        public Meal_Name meal_name { get; set; }

        private ObservableCollection<Menu_Bindable> _menu = new ObservableCollection<Menu_Bindable>();
        public ObservableCollection<Menu_Bindable> Menu
        {
            get { return _menu; }
            set { _menu = value; }
        }

        private GeoCoordinate _position;
        public GeoCoordinate Position
        {
            get { return _position; }
            set
            {
                SetProperty<GeoCoordinate>(ref _position, value);
            }
        }

//        public Services services { get; set; }
    }

    public class Menu_Bindable : BindableBase
    {
        private ObservableCollection<int> _available = new ObservableCollection<int>();
        public ObservableCollection<int> Available
        {
            get { return _available; }
            set { _available = value; }
        }
        
        private string _additional_fee;
        public string AdditionalFee
        {
            get { return _additional_fee; }
            set { SetProperty<string>(ref _additional_fee, value); }
        }

        private ObservableCollection<Menu_Bindable> _children = new ObservableCollection<Menu_Bindable>();
        public ObservableCollection<Menu_Bindable> Children
        {
            get { return _children; }
            set { SetProperty<ObservableCollection<Menu_Bindable>>(ref _children, value); }
        }
        
        private string _descrip;
        public string Description
        {
            get { return _descrip; }
            set { SetProperty<string>(ref _descrip, value); }
        }

        private string _parentName;
        public string ParentName
        {
            get { return _parentName; }
            set { SetProperty<string>(ref _parentName, value); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty<string>(ref _id, value); }
        }

        private bool _isOrderable;
        public bool IsOrderable
        {
            get { return _isOrderable; }
            set { SetProperty<bool>(ref _isOrderable, value); }
        }


        public string item_select_weight { get; set; }

        private string _freeChildSelect;
        public string FreeChildSelect
        {
            get { return _freeChildSelect; }
            set { SetProperty<string>(ref _freeChildSelect, value); }
        }

        private string _maxFreeChildSelect;
        public string MaxFreeChildSelect
        {
            get { return _maxFreeChildSelect; }
            set { SetProperty<string>(ref _maxFreeChildSelect, value); }
        }

        private string _maxChildSelect;
        public string MaxChildSelect
        {
            get { return _maxChildSelect; }
            set { SetProperty<string>(ref _maxChildSelect, value); }
        }

        private string _minChildSelect;
        public string MinChildSelect
        {
            get { return _minChildSelect; }
            set { SetProperty<string>(ref _minChildSelect, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                SetProperty<string>(ref _name, value);
                OnPropertyChanged("NameUpper");
                OnPropertyChanged("NameLower");
            }
        }
        public string NameLower
        {
            get { return Name.ToLower(); }
        }
        public string NameUpper
        {
            get { return Name.ToUpper(); }
        }

        public override string ToString()
        {
            return Name;
        }

        private string _price;
        public string Price
        {
            get { return _price; }
            set { SetProperty<string>(ref _price, value); }
        }
    }


    public class Delivery_Bindable : BindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty<int>(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty<string>(ref _address, value); }
        }
        private ObservableCollection<string> _cuisine = new ObservableCollection<string>();
        public ObservableCollection<string> Cuisine
        {
            get { return _cuisine; }
            set { _cuisine = value; }
        }
        //public string cs_phone { get; set; }
        //public Rds_Info rds_info { get; set; }
        //public Services services { get; set; }
        //public int allow_tip { get; set; }
        //public int allow_asap { get; set; }
        //public string addr { get; set; }
        //public Full_Addr full_addr { get; set; }
        //public string city { get; set; }
        //public int del { get; set; }
        private float mino;
        public float MinOrder
        {
            get { return mino; }
            set { SetProperty<float>(ref mino, value); }
        }

        private bool _canDeliver;
        public bool CanDeliver
        {
            get { return _canDeliver; }
            set { SetProperty<bool>(ref _canDeliver, value); }
        }
        private bool _canPickup;

        public bool CanPickup
        {
            get { return _canPickup; }
            set { SetProperty<bool>(ref _canPickup, value); }
        }
                
        //public int is_delivering { get; set; }
    }

    public class OrderStatus_Bindable : BindableBase
    {
        private string _msg;
        public string Message
        {
            get { return _msg; }
            set { SetProperty<string>(ref _msg, value); }
        }
        private string _refnum;
        public string RefNumber
        {
            get { return _refnum; }
            set { SetProperty<string>(ref _refnum, value); }
        }
        private string _orderId;
        public string OrderId
        {
            get { return _orderId; }
            set { SetProperty<string>(ref _orderId, value); }
        }
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty<string>(ref _text, value); }
        }
        private int _error;
        public int Error
        {
            get { return _error; }
            set { SetProperty<int>(ref _error, value); }
        }
    }


    public class Fee_Bindable : BindableBase
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty<string>(ref _id, value); }
        }
        private bool _allowTip;
        public bool AllowTip
        {
            get { return _allowTip; }
            set { SetProperty<bool>(ref _allowTip, value); }
        }
        private bool _allowASAP;
        public bool AllowASAP
        {
            get { return _allowASAP; }
            set { SetProperty<bool>(ref _allowASAP, value); }
        }
        private string _tax;
        public string Tax
        {
            get { return _tax; }
            set { SetProperty<string>(ref _tax, value); }
        }
        private string _fee;
        public string Fee
        {
            get { return _fee; }
            set { SetProperty<string>(ref _fee, value); }
        }

        public int[] meals { get; set; }
        public int zone_id { get; set; }
        public string provided_id { get; set; }
        public string provided_id2 { get; set; }
        public string service { get; set; }

        private int _can_service;
        public int CanService
        {
            get { return _can_service; }
            set { SetProperty<int>(ref _can_service, value); }
        }
    
        private bool _delivering;
        public bool IsDelivering
        {
            get { return _delivering; }
            set { SetProperty<bool>(ref _delivering, value); }
        }

        private string _minOrder;
        public string MinOrder
        {
            get { return _minOrder; }
            set { SetProperty<string>(ref _minOrder, value); }
        }

        private string _ready;
        public string IsReady
        {
            get { return _ready; }
            set { SetProperty<string>(ref _ready, value); }
        }

        private string _deliveryTime;
        public string DeliveryTime
        {
            get { return _deliveryTime; }
            set { SetProperty<string>(ref _deliveryTime, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty<string>(ref _message, value); }
        }

        private int _error;
        public int Error
        {
            get { return _error; }
            set { SetProperty<int>(ref _error, value); }
        }
    }

    public class DeliveryCheck_Bindable : BindableBase
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty<string>(ref _id, value); }
        }
        private bool _allowTip;
        public bool AllowTip
        {
            get { return _allowTip; }
            set { SetProperty<bool>(ref _allowTip, value); }
        }
        private bool _allowASAP;
        public bool AllowASAP
        {
            get { return _allowASAP; }
            set { SetProperty<bool>(ref _allowASAP, value); }
        }

        public int[] meals { get; set; }
        public int zone_id { get; set; }
        public string provided_id { get; set; }
        public string provided_id2 { get; set; }
        public int can_service { get; set; }
        public string service { get; set; }

        private bool _delivering;
        public bool IsDelivering
        {
            get { return _delivering; }
            set { SetProperty<bool>(ref _delivering, value); }
        }

        private string _minOrder;
        public string MinOrder
        {
            get { return _minOrder; }
            set { SetProperty<string>(ref _minOrder, value); }
        }

        private string _ready;
        public string IsReady
        {
            get { return _ready; }
            set { SetProperty<string>(ref _ready, value); }
        }

        private string _deliveryTime;
        public string DeliveryTime
        {
            get { return _deliveryTime; }
            set { SetProperty<string>(ref _deliveryTime, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty<string>(ref _message, value); }
        }
        
        private int _error;
        public int Error
        {
            get { return _error; }
            set { SetProperty<int>(ref _error, value); }
        }
    }



}
