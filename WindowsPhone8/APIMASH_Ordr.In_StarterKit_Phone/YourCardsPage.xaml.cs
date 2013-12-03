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
using System.IO.IsolatedStorage;
using System.IO;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class YourCardsPage : PhoneApplicationPage
    {
        Card_Bindable selectedCard;
        public YourCardsPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;

            Globals.Instance.ordrin.Cards.CollectionChanged += Cards_CollectionChanged;
        }

        void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        private void cardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in Globals.Instance.ordrin.Cards)
            {
                item.IsSelected = false;
            }

            ApplicationBarIconButton deleteAddressButton = this.ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            ApplicationBarIconButton selectAddressButton = this.ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (e.AddedItems.Count > 0)
            {
                selectedCard = e.AddedItems[0] as Card_Bindable;
                selectedCard.IsSelected = true;
                deleteAddressButton.IsEnabled = true;
                selectAddressButton.IsEnabled = true;
            }
            else
            {
                deleteAddressButton.IsEnabled = false;
                selectAddressButton.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddCardPage.xaml", UriKind.Relative));
        }

        private void AddCard_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddCardPage.xaml", UriKind.Relative));
        }

        private void selectCard_Click(object sender, EventArgs e)
        {
            var cards = Globals.Instance.ordrin.Cards;
            foreach (var item in cards)
            {
                item.IsDefault = false;
            }
            selectedCard.IsDefault = true;
            SaveDefaultCard(selectedCard.Nick);
        }

        private void SaveDefaultCard(string name)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("card.default", FileMode.Create))
                {
                    byte[] buffer = System.Text.Encoding.Unicode.GetBytes(name);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }


        private void deleteCard_Click(object sender, EventArgs e)
        {
            string nick = selectedCard.Nick;
            Globals.Instance.ordrin.DeleteCard(nick);
        }

        private void editCard_Click(object sender, EventArgs e)
        {
            string nick = selectedCard.Nick;
            NavigationService.Navigate(new Uri("/AddCardPage.xaml?nick="+nick, UriKind.Relative));
        }
    }
}