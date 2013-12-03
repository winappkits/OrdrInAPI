using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using APIMASH_Ordr.In_StarterKit_Phone.Resources;

namespace APIMASH_Ordr.In_StarterKit_Phone.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.Addresses = new ObservableCollection<ItemViewModel>();
            this.Places = new ObservableCollection<ItemViewModel>();
            this.Restaurants = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ObservableCollection<ItemViewModel> Addresses { get; private set; }
        public ObservableCollection<ItemViewModel> Places { get; private set; }
        public ObservableCollection<ItemViewModel> Restaurants { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { LineOne = "Village Pancakes or Challah French Toast", LineTwo = "$8.99", LineThree = "HOME" });
            this.Items.Add(new ItemViewModel() { LineOne = "Egg Sandwich with Cheese on a Croissant", LineTwo = "$4.00", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { LineOne = "Seared Tuna Steak Salad", LineTwo = "$13.75", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { LineOne = "Portobello & Parmasian Salad", LineTwo = "$7.00", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { LineOne = "Philadelphia Cheesesteak Wrap", LineTwo = "$5.50", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Addresses.Add(new ItemViewModel() { LineOne = "Village Pancakes or Challah French Toast", LineTwo = "$8.99", LineThree = "HOME" });

            this.Places.Add(new ItemViewModel() { LineOne = "Moonstruck on Third", LineTwo = "$8.99", LineThree = "HOME" });
            this.Places.Add(new ItemViewModel() { LineOne = "The Cottage", LineTwo = "$8.99", LineThree = "HOME" });
            this.Places.Add(new ItemViewModel() { LineOne = "Brick Oven Pizza", LineTwo = "$8.99", LineThree = "HOME" });
            this.Places.Add(new ItemViewModel() { LineOne = "33 Great Sichuan Chef", LineTwo = "$8.99", LineThree = "HOME" });
            this.Places.Add(new ItemViewModel() { LineOne = "282 Sea King Thai NY", LineTwo = "$8.99", LineThree = "HOME" });

            this.Restaurants.Add(new ItemViewModel() { LineOne = "Grey Dog", LineTwo = "$8.99", LineThree = "HOME" });
            this.Restaurants.Add(new ItemViewModel() { LineOne = "Washington Square Diner", LineTwo = "$8.99", LineThree = "HOME" });
            this.Restaurants.Add(new ItemViewModel() { LineOne = "Charlie Mom", LineTwo = "$8.99", LineThree = "HOME" });
            this.Restaurants.Add(new ItemViewModel() { LineOne = "Saigon Market", LineTwo = "$8.99", LineThree = "HOME" });
            this.Restaurants.Add(new ItemViewModel() { LineOne = "Spurs Empire", LineTwo = "$8.99", LineThree = "HOME" });
            
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}