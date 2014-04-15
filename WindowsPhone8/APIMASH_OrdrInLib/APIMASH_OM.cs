using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/

namespace APIMASH_OrdrInLib
{
    public class OrderHistoryObject
    {
        public OrderHistory[] Property1 { get; set; }
    }

    /*
    [{"oid":"525455fca7e7e00572000815","rid":"23858","rname":"Bravo Pizza","tip":"9.00","tax":"3.99","fee":"0.00","total":"57.99",
        "subotal":"45.00",
        "ctime":1381258748,
        "tray":[
            {"partner_args":"",
             "provided_id":"N1585",
                "parent_id":18902356,
                "name":"Veggie Pie",
                "price":4500,
                "menu_item_id":18902357,
                "qty":"1",
                "id":"18902357"}]},
    {"oid":"52545695a7e7e00572000817","rid":"23858","rname":"Bravo Pizza","tip":"6.50","tax":"2.88","fee":"0.00","total":"41.88","subotal":"32.50","ctime":1381258901,
    "tray":[{"partner_args":"","provided_id":"N32","parent_id":18902378,"name":"New York Style Extra Cheese Pizza Pie","price":2350,"menu_item_id":18902393,"opts":{"18902394":{"opts":[{"partner_args":"","provided_id":"N649","parent_id":18902394,"name":"Broccoli","price":300,"menu_item_id":18902396,"qty":"1","id":"18902396"},

    {"partner_args":"","provided_id":"N651","parent_id":18902394,"name":"Eggplant","price":300,"menu_item_id":18902398,"qty":"1","id":"18902398"},{"partner_args":"","provided_id":"N652","parent_id":18902394,"name":"Extra Cheese","price":300,"menu_item_id":18902399,"qty":"1","id":"18902399"}],"partner_args":"","provided_id":"N646","parent_id":18902393,"name":"Pie Toppings","price":0,"menu_item_id":18902394}},"qty":"1","id":"18902393"}]},{"oid":"52556be380f375655a0000ef","rid":"23858","rname":"Bravo Pizza","tip":"4.59","tax":"2.04","fee":"0.00","total":"29.58","subotal":"22.95","ctime":1381329891,"tray":[{"partner_args":"","provided_id":"N52:N166:N167","parent_id":18901991,"name":"Cheese Pizza Slice - New York Style","price":299,"menu_item_id":18901992,
    "opts":{"18901993":{"opts":[{"partner_args":"","provided_id":"N173","parent_id":18901993,"name":"Cheese","price":80,"menu_item_id":18901996,"qty":"5","id":"18901996"},{"partner_args":"","provided_id":"N174","parent_id":18901993,"name":"Eggplant","price":80,"menu_item_id":18901997,"qty":"5","id":"18901997"}],"partner_args":"","provided_id":"N169","parent_id":18901992,"name":"Slice Toppings","price":0,"menu_item_id":18901993}},

    "qty":"5",
    "id":"18901992"}]}]

    */

    public class OrderHistory
    {
        public Tray[] tray { get; set; }
        public object oid { get; set; }
        public string total { get; set; }
        public string tip { get; set; }
        public string tax { get; set; }
        public string fee { get; set; }
        public string subtotal { get; set; }
        public int ctime { get; set; }
        public string rid { get; set; }
        public string rname { get; set; }

        public void Copy(OrderHistory_Bindable binding)
        {
            binding.OrderId = oid;
            binding.Total = total;
            binding.Tip = tip;
            binding.Tax = tax;
            binding.Fee = fee;
            binding.SubTotal = subtotal;
            binding.RestaurantId = rid;
            binding.RestaurantName = rname;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            binding.OrderTime = epoch.AddSeconds(ctime);
            binding.TrayItems = new List<Item_Bindable>();
            if (tray != null)
            {
                float stotal = 0.0f;
                foreach (var item in tray)
                {
                    var newItem = new Item_Bindable();
                    item.Copy(newItem);
                    stotal += float.Parse(newItem.Price);
                    binding.TrayItems.Add(newItem);
                }
                //if (stotal.ToString("0.00") != subtotal)
                //    binding.SubTotal = stotal.ToString("0.00");
            }
        }
    }

    public class Tray
    {
        public int id { get; set; }
        public string price { get; set; }
        public string name { get; set; }
        public string qty { get; set; }
        public Opt[] opts { get; set; }

        public void Copy(Item_Bindable binding)
        {
            binding.Id = id;
            binding.Price = (float.Parse(qty) * float.Parse(price) / 100.0f).ToString("0.00");
            binding.Name = name;
            binding.Quantity = qty;
            binding.Options = new List<Option_Bindable>();
            if (opts != null)
            {
                foreach (var item in opts)
                {
                    var option = new Option_Bindable();
                    item.Copy(option);
                    binding.Options.Add(option);
                }
            }
        }
    }

    public class Opt
    {
        public int id { get; set; }
        public string price { get; set; }
        public string name { get; set; }
        public string qty { get; set; }
        public void Copy(Option_Bindable binding)
        {
            binding.Id = id;
            binding.Price = price;
            binding.Name = name;
            binding.Quantity = qty;
        }
    }

    public class Error
    {
        public string id { get; set; }
        public string description { get; set; }
    }


    public class PasswordResetObject
    {
        public string msg { get; set; }
        public _Appdata _appdata { get; set; }
        public int _err { get; set; }
    }

    public class _Appdata
    {
        public string token { get; set; }
    }

    public class CreateAddressObject
    {
        public string _error { get; set; }
        public string msg { get; set; }
    }

    public class DeleteObject
    {
        public string msg { get; set; }
    }

    public class DeleteCardObject
    {
        public string msg { get; set; }
    }

    public class CreateCardObject
    {
        public string _err { get; set; }
        public string msg { get; set; }
    }


    public class Rootobject
    {
        public string rid { get; set; }
        public int allow_tip { get; set; }
        public string allow_asap { get; set; }
        public int can_service { get; set; }
        public string service { get; set; }
        public int delivery { get; set; }
        public string mino { get; set; }
        public string ready { get; set; }
        public string del { get; set; }
        public string msg { get; set; }
        public string tax { get; set; }
        public string fee { get; set; }
        public int _err { get; set; }
    }




    public static class MyExtensions
    {
        public static void Copy(this OrdrAddress order, Address_Bindable bindable)
        {
            bindable.Nick = order.nick;
            bindable.Address = order.addr;
            bindable.Address2 = order.addr2;
            bindable.City = order.city;
            bindable.State = order.state;
            bindable.Zip = order.zip;
            bindable.Phone = order.phone;
            bindable.IsDefault = order._default;
        }

        public static void Copy(this CardObject order, Card_Bindable bindable)
        {
            bindable.Nick = order.nick;
            bindable.Address = order.bill_addr;
            bindable.City = order.bill_city;
            bindable.State = order.bill_state;
            bindable.Zip = order.bill_zip;
            bindable.CardToken = order.card_token;
            bindable.Last5 = order.cc_last5;
            bindable.CardMonth = order.expiry_month;
            bindable.CardYear = order.expiry_year;
            bindable.CardType = order.type;
        }
    }

    public class AccountObject
    {
        public string em { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string name
        {
            get
            {
                return first_name + " " + last_name;
            }
        }
        public string pw { get; set; }
        public string _id { get; set; }
    }

    public class UserCreateObject
    {
        public int _err { get; set; }
        public string user_id { get; set; }
        public string msg { get; set; }
        public Errs errs { get; set; }
    }

    public class Errs
    {
        public string pw { get; set; }
    }


    public class LocationObject
    {
        public string authenticationResultCode { get; set; }
        public string brandLogoUri { get; set; }
        public string copyright { get; set; }
        public Resourceset[] resourceSets { get; set; }
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string traceId { get; set; }
    }

    public class Resourceset
    {
        public int estimatedTotal { get; set; }
        public Resource[] resources { get; set; }
    }

    public class Resource
    {
        public string __type { get; set; }
        public float[] bbox { get; set; }
        public string name { get; set; }
        public Point point { get; set; }
        public Address address { get; set; }
        public string confidence { get; set; }
        public string entityType { get; set; }
        public Geocodepoint[] geocodePoints { get; set; }
        public string[] matchCodes { get; set; }
    }

    public class Point
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Address
    {
        public string addressLine { get; set; }
        public string adminDistrict { get; set; }
        public string adminDistrict2 { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
    }

    public class Geocodepoint
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
        public string calculationMethod { get; set; }
        public string[] usageTypes { get; set; }
    }

    public class RestaurantObject
    {
        public string addr { get; set; }
        public string city { get; set; }
        public string cs_contact_phone { get; set; }
        public string[] cuisine { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public Meal_Name meal_name { get; set; }
        public Menu[] menu { get; set; }
        public string name { get; set; }
        public string postal_code { get; set; }
        public Rds_Info rds_info { get; set; }
        public string restaurant_id { get; set; }
        public Services services { get; set; }
        public string state { get; set; }

        public void Copy(Restaurant_Bindable bindable)
        {
            bindable.Address = addr;
            bindable.City = city;
            bindable.Id = restaurant_id;
            bindable.Name = name;
            bindable.Phone = cs_contact_phone;
            bindable.State = state;
            bindable.Zip = postal_code;
            bindable.Menu.Clear();
            foreach (var item in menu)
            {
                var newMenu = new Menu_Bindable();
                item.Copy(newMenu);
                bindable.Menu.Add(newMenu);
            }
            foreach (var item in cuisine)
            {
                bindable.Cuisine.Add(item);
            }
        }
    }

    public class Meal_Name
    {
        public string _0 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
    }

    public class Services
    {
        public string delivery { get; set; }
        public string pickup { get; set; }
    }

    public class Menu
    {
        public string additional_fee { get; set; }
        public Child[] children { get; set; }
        public string descrip { get; set; }
        public string free_child_select { get; set; }
        public string id { get; set; }
        public int is_orderable { get; set; }
        public string item_select_weight { get; set; }
        public string max_child_select { get; set; }
        public string max_free_child_select { get; set; }
        public string min_child_select { get; set; }
        public string name { get; set; }
        public string price { get; set; }

        public void Copy(Menu_Bindable bindable)
        {
            bindable.Name = name;
            bindable.Description = descrip;
            bindable.Price = price;
            bindable.Id = id;
            bindable.IsOrderable = (is_orderable == 1);
            bindable.AdditionalFee = additional_fee;
            bindable.FreeChildSelect = free_child_select;
            bindable.MaxFreeChildSelect = max_free_child_select;
            bindable.MinChildSelect = min_child_select;
            bindable.MaxChildSelect = max_child_select;
            bindable.Children.Clear();
            if (children != null)
            {
                foreach (var item in children)
                {
                    var bind = new Menu_Bindable();
                    item.Copy(bind);
                    bind.ParentName = name;
                    bindable.Children.Add(bind);
                }
            }
        }
    }

    public class Child
    {
        public string additional_fee { get; set; }
        public int[] availability { get; set; }
        public string descrip { get; set; }
        public string free_child_select { get; set; }
        public string id { get; set; }
        public string is_orderable { get; set; }
        public string item_select_weight { get; set; }
        public string max_child_select { get; set; }
        public string max_free_child_select { get; set; }
        public string min_child_select { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Child1[] children { get; set; }

        public void Copy(Menu_Bindable bindable)
        {
            bindable.Name = name;
            bindable.Description = descrip;
            bindable.Price = price;
            bindable.Id = id;
            bindable.IsOrderable = (is_orderable == "1");
            bindable.AdditionalFee = additional_fee;
            bindable.FreeChildSelect = free_child_select;
            bindable.MaxFreeChildSelect = max_free_child_select;
            bindable.MinChildSelect = min_child_select;
            bindable.MaxChildSelect = max_child_select;
            bindable.Children.Clear();
            if (children != null)
            {
                foreach (var item in children)
                {
                    var bind = new Menu_Bindable();
                    item.Copy(bind);
                    bind.ParentName = name;
                    bindable.Children.Add(bind);
                }
            }
            bindable.Available.Clear();
            if (availability != null)
            {
                foreach (var item in availability)
                {
                    bindable.Available.Add(item);
                }
            }
        }
    }

    public class Child1
    {
        public string additional_fee { get; set; }
        public Child2[] children { get; set; }
        public string descrip { get; set; }
        public string free_child_select { get; set; }
        public string id { get; set; }
        public int is_orderable { get; set; }
        public string item_select_weight { get; set; }
        public string max_child_select { get; set; }
        public string max_free_child_select { get; set; }
        public string min_child_select { get; set; }
        public string name { get; set; }
        public string price { get; set; }

        public void Copy(Menu_Bindable bindable)
        {
            bindable.Name = name;
            bindable.Description = descrip;
            bindable.Price = price;
            bindable.Id = id;
            bindable.IsOrderable = (is_orderable == 1);
            bindable.AdditionalFee = additional_fee;
            bindable.FreeChildSelect = free_child_select;
            bindable.MaxFreeChildSelect = max_free_child_select;
            bindable.MinChildSelect = min_child_select;
            bindable.MaxChildSelect = max_child_select;
            bindable.Children.Clear();
            if (children != null)
            {
                int max = int.Parse(max_child_select);
                var select = new Menu_Bindable();
                select.ParentName = name;
                if (max < 2)
                    select.Name = "Select an Option";
                else
                    select.Name = "Select Options";
                select.Id = "***";
                bindable.Children.Add(select);
                foreach (var item in children)
                {
                    var bind = new Menu_Bindable();
                    item.Copy(bind);
                    bind.ParentName = name;
                    bindable.Children.Add(bind);
                }
            }
            bindable.Available.Clear();
        }
    }

    public class Child2
    {
        public string additional_fee { get; set; }
        public int[] availability { get; set; }
        public string descrip { get; set; }
        public string free_child_select { get; set; }
        public string id { get; set; }
        public string is_orderable { get; set; }
        public string item_select_weight { get; set; }
        public string max_child_select { get; set; }
        public string max_free_child_select { get; set; }
        public string min_child_select { get; set; }
        public string name { get; set; }
        public string price { get; set; }

        public void Copy(Menu_Bindable bindable)
        {
            bindable.Name = name;
            bindable.Description = descrip;
            bindable.Price = price;
            bindable.Id = id;
            bindable.IsOrderable = (is_orderable == "1");
            bindable.AdditionalFee = additional_fee;
            bindable.FreeChildSelect = free_child_select;
            bindable.MaxFreeChildSelect = max_free_child_select;
            bindable.MinChildSelect = min_child_select;
            bindable.MaxChildSelect = max_child_select;
            bindable.Children.Clear();
            bindable.Available.Clear();
            if (availability != null)
            {
                foreach (var item in availability)
                {
                    bindable.Available.Add(item);
                }
            }
        }
    }



    public class DeliveryListRootobject
    {
        public DeliveryObject[] Property1 { get; set; }
    }

    public class DeliveryObject
    {
        public int id { get; set; }
        public string na { get; set; }
        public string cs_phone { get; set; }
        public Rds_Info rds_info { get; set; }
        public Services services { get; set; }
        public int allow_tip { get; set; }
        public string allow_asap { get; set; }
        public string[] cu { get; set; }
        public string addr { get; set; }
        public Full_Addr full_addr { get; set; }
        public string city { get; set; }
        public int del { get; set; }
        public float mino { get; set; }
        public int is_delivering { get; set; }

        public void Copy(Delivery_Bindable bindable)
        {
            try
            {
                bindable.Name = na;
                bindable.Id = id;
                bindable.MinOrder = mino;
                bindable.Address = addr;
                bindable.CanDeliver = (is_delivering == 1) || (services.delivery == "delivery");
                bindable.CanPickup = (is_delivering == 0) || (services.pickup == "pickup");
                bindable.Cuisine.Clear();
                if (cu != null)
                {
                    foreach (var item in cu)
                    {
                        bindable.Cuisine.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }

    public class Rds_Info
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
    }

    public class Deliver
    {
        public int time { get; set; }
        public float mino { get; set; }
        public int can { get; set; }
    }

    public class Full_Addr
    {
        public string addr { get; set; }
        public string addr2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
    }


    /*
    {
        "rid":"24268",
        "allow_tip":1,
        "allow_asap":1,
        "can_service":0,
        "service":"delivery",
        "delivery":0,
        "mino":"0.00",
        "ready":"40-50",
        "del":"40-50",
        "msg":"This restaurant is not open for online ordering during your requested time. It is available from 07:00AM to 10:00PM.",
        "tax":"0.00",
        "fee":"0.00",
        "_err":0}
    */
    public class FeeObject
    {
        public string rid { get; set; }
        public int allow_tip { get; set; }
        public string allow_asap { get; set; }
        public string tax { get; set; }
        public string fee { get; set; }
        public int[] meals { get; set; }
        public int zone_id { get; set; }
        public string provided_id { get; set; }
        public string provided_id2 { get; set; }
        public int can_service { get; set; }
        public string service { get; set; }
        public int delivery { get; set; }
        public string mino { get; set; }
        public string ready { get; set; }
        public string del { get; set; }
        public string msg { get; set; }
        public int _err { get; set; }

        public void Copy(Fee_Bindable bindable)
        {
            bindable.AllowASAP = allow_asap == "yes";
            bindable.AllowTip = allow_tip == 1;
            bindable.DeliveryTime = del;
            bindable.Error = _err;
            bindable.Id = rid;
            bindable.IsDelivering = delivery == 1;
            bindable.IsReady = ready;
            bindable.Message = msg;
            bindable.MinOrder = mino;
            bindable.Fee = fee;
            bindable.Tax = tax;
            bindable.CanService = can_service;
        }
    }


    public class DeliveryCheckObject
    {
        public string rid { get; set; }
        public int allow_tip { get; set; }
        public string allow_asap { get; set; }
        public int[] meals { get; set; }
        public int zone_id { get; set; }
        public string provided_id { get; set; }
        public string provided_id2 { get; set; }
        public int can_service { get; set; }
        public string service { get; set; }
        public int delivery { get; set; }
        public string mino { get; set; }
        public string ready { get; set; }
        public string del { get; set; }
        public string msg { get; set; }
        public int _err { get; set; }

        public void Copy(DeliveryCheck_Bindable bindable)
        {
            bindable.AllowASAP = allow_asap == "yes";
            bindable.AllowTip = allow_tip == 1;
            bindable.DeliveryTime = del;
            bindable.Error = _err;
            bindable.Id = rid;
            bindable.IsDelivering = delivery == 1;
            bindable.IsReady = ready;
            bindable.Message = msg;
            bindable.MinOrder = mino;
        }

    }

    public class OrderStatusObject
    {
        public int _err { get; set; }
        public string _msg { get; set; }
        public string msg { get; set; }
        public int err_code { get; set; }
        public int _error { get; set; }
        public string refnum { get; set; }
        public string cs_order_id { get; set; }
        public string text { get; set; }
        public void Copy(OrderStatus_Bindable bindable)
        {
            bindable.Message = msg + _msg;
            bindable.Error = _error;
            bindable.RefNumber = refnum;
            bindable.OrderId = cs_order_id;
            bindable.Text = text;
        }
    }


}
