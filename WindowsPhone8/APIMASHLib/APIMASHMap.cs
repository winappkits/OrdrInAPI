using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using APIMASH_OrdrInLib;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System.IO.IsolatedStorage;
/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/

/*
 *
 *  A P I   M A S H 
 *
 * This class makes the HTTP call and deserialzies the stream to a supplied Type
*/

namespace APIMASH_OrdrInLib
{
    public class OrdrAddress
    {
        public string nick { get; set; }
        public string addr { get; set; }
        public string addr2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public int _default { get; set; }
    }

    public class CardObject
    {
        public string nick { get; set; }
        public string cc_last5 { get; set; }
        public string type { get; set; }
        public string card_token { get; set; }
        public string expiry_year { get; set; }
        public string expiry_month { get; set; }
        public string bill_zip { get; set; }
        public string bill_state { get; set; }
        public string bill_city { get; set; }
        public string bill_addr { get; set; }
    }
}



namespace APIMASHLib
{
    public static class APIMASHMap
    {
#if OBJMAP_JSON // .NET JSON
        public static T DeserializeObject<T>(string objString)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(objString)))
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
                catch (Exception) { throw; }
            }
        }
#endif

#if OBJMAP_XML // .NET XML
        public static T DeserializeObject<T>(string objString)
        {
            using (MemoryStream _Stream = new MemoryStream(Encoding.Unicode.GetBytes(objString)))
            {
                try
                {

                    DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)_serializer.ReadObject(_Stream);

                    XmlSerializer _serializer = new XmlSerializer(typeof(T));
                    return (T)_serializer.Deserialize(_Stream);
                }
                catch (Exception) { throw; }
            }
        }
#endif

#if OBJMAP_NEWTONSOFT // JSON.NET
        public static T DeserializeObject<T>(string objString)
        {
            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            return (T)JsonConvert.DeserializeObject<T>(objString, settings);
        }
#endif

        private static string LoadDL(string name)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if(myIsolatedStorage.FileExists("list.txt"))
                    {
                        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("list.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string ret = reader.ReadToEnd();
                                reader.Close();
                                return ret;
                            }
                        }
                    }
                    return null;
                }
            }
            catch
            {
                return null;//add some code here
            }
        }
        private static void DeleteDL(string name)
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    myIsolatedStorage.DeleteFile(name.Replace('/', '_'));
                }
            }
            catch
            {    //add some code here
            }
        }
        private static void SaveDL(string name, string data)
        {
            try
            {
                DeleteDL(name);                
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("list.txt", FileMode.OpenOrCreate, myIsolatedStorage))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(data);
                            writer.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }

        async public static Task<T> LoadObject<T>(string apiCall, IEnumerable<KeyValuePair<string, string>> data=null, string method="GET")
        {
            var ws = new HttpClient();
            var uriAPICall = new Uri(apiCall);
            if (data == null)
            {
                string objString=null;
                if (method == "GET")
                {
                    if (objString == null)
                    {
                        var r = await ws.GetAsync(uriAPICall);
                        objString = await r.Content.ReadAsStringAsync();
                    }

                    Type listType = typeof(T);
                    if (listType == typeof(List<OrdrAddress>))
                    {
                        JToken blogPostArray = JToken.Parse(objString);
                        var n = new List<OrdrAddress>();
                        foreach (var item in blogPostArray)
                        {
                            var neworder = new OrdrAddress()
                            {
                                nick = (string)(JToken)item.First["nick"],
                                addr = (string)(JToken)item.First["addr"],
                                addr2 = (string)(JToken)item.First["addr2"],
                                city = (string)(JToken)item.First["city"],
                                state = (string)(JToken)item.First["state"],
                                zip = (string)(JToken)item.First["zip"],
                                phone = (string)(JToken)item.First["phone"]
                            };
                            if ((string)(JToken)item.First["default"] != null)
                                neworder._default = int.Parse((string)(JToken)item.First["default"]);
                            n.Add(neworder);
                        }
                        return (T)(n as object);
                    }
                    else if (listType == typeof(List<CardObject>))
                    {
                        JToken blogPostArray = JToken.Parse(objString);
                        var n = new List<CardObject>();
                        foreach (var item in blogPostArray)
                        {
                            var neworder = new CardObject()
                            {
                                nick = (string)(JToken)item.First["nick"],
                                bill_addr = (string)(JToken)item.First["bill_addr"],
                                bill_city = (string)(JToken)item.First["bill_city"],
                                bill_state = (string)(JToken)item.First["bill_state"],
                                bill_zip = (string)(JToken)item.First["bill_zip"],
                                card_token = (string)(JToken)item.First["card_token"],
                                type = (string)(JToken)item.First["type"],
                                cc_last5 = (string)(JToken)item.First["cc_last5"],
                                expiry_year = (string)(JToken)item.First["expiry_year"],
                                expiry_month = (string)(JToken)item.First["expiry_month"]
                            };
                            n.Add(neworder);
                        }
                        return (T)(n as object);
                    }
                    else
                    {
                        return (T)DeserializeObject<T>(objString);
                    }
                }
                else if (method == "DELETE")
                {
                    var r = await ws.DeleteAsync(uriAPICall);
                    objString = await r.Content.ReadAsStringAsync();
                    return (T)DeserializeObject<T>(objString);
                }
                else
                {
                    return (T)DeserializeObject<T>(objString);
                }
            }
            else
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(data);
                if ((method.ToUpper() == "PUT") == false)
                {
                    var response = await ws.PostAsync(uriAPICall, content);
                    var objString = await response.Content.ReadAsStringAsync();
                    return (T)DeserializeObject<T>(objString);
                }
                else
                {
                    var response = await ws.PutAsync(uriAPICall, content);
                    var objString = await response.Content.ReadAsStringAsync();
                    return (T)DeserializeObject<T>(objString);
                }
            }
        }

    }
}
