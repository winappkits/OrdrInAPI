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
using System.Text;
using System.Collections;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class MenuItemPage : PhoneApplicationPage
    {
        public MenuItemPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;
            if (Globals.Instance.ordrin.CurrentItem.Children.Count == 0)
            {
                descriptionViewer.MaxHeight = Double.PositiveInfinity;
                optionGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            this.Loaded += MenuItemPage_Loaded;
        }


        void MenuItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            optionList.Height = optionGrid.RowDefinitions[2].ActualHeight;
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        List<Menu_Bindable> subitems = new List<Menu_Bindable>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int quantity = 1;
            int.TryParse(quantityText.Text, out quantity);
            
            // check option quantities
            foreach (var item in Globals.Instance.ordrin.CurrentItem.Children)
            {
                int minOrder=int.Parse(item.MinChildSelect);
                int maxOrder=int.Parse(item.MaxChildSelect);
                int currentCount = 0;
                
                var children = item.Children.Where(s => s.Id != "***");
                foreach (var subitem in children)
                {
                    if (subitems.Contains(subitem))
                    {
                        currentCount++;
                    }
                }

                if (minOrder > 0)
                {
                    if (currentCount == 0)
                    {
                        string company = Application.Current.Resources["AppName"] as string;
                        MessageBox.Show("Please select an option for '" + item.Name + "'", company, MessageBoxButton.OK);
                        return;
                    }
                }
                if (maxOrder > 0)
                {
                    // check to see if we added too many
                    if (currentCount > maxOrder)
                    {
                        string company = Application.Current.Resources["AppName"] as string;
                        MessageBox.Show("You ordered too many options for '" + item.Name + "'", company, MessageBoxButton.OK);
                        return;
                    }
                }
            }

            Globals.Instance.ordrin.AddMenuItem(Globals.Instance.ordrin.CurrentItem, quantity, subitems.Where(s => s.Id != "***").ToList());
            NavigationService.GoBack();
        }

        private string CreateCommaDelimitedList(IList items)
        {
            if (items != null && items.Count > 0)
            {
                string summarizedString = "";
                for (int i = 0; i < items.Count; i++)
                {
                    var newItem = items[i] as Menu_Bindable;
//                    if (newItem.Id != "***")
                        summarizedString += newItem.ToString();

                    // If not last item, add a comma to seperate them
                    if (i != items.Count - 1)
                        summarizedString += ", ";
                }

                return summarizedString;
            }
            else
                return "Nothing selected";
        }
        private void optionPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                foreach (var item in e.RemovedItems)
                {
                    var remove = item as Menu_Bindable;
                    subitems.Remove(remove);
                }
            }
            if (e.AddedItems.Count > 0)
            {
                foreach (var item in e.AddedItems)
                {
                    var addedItem = item as Menu_Bindable;
                    if(subitems.Contains(addedItem) == false)
                        subitems.Add(addedItem);
                }
            }
        }

        private void quantityText_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;
            //s.Foreground = Application.Current.Resources["BlackColor"] as SolidColorBrush;
            s.Select(0, s.Text.Length);
        }

        private void quantityText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox s = sender as TextBox;
            //s.Foreground = Application.Current.Resources["WhiteColor"] as SolidColorBrush;

            int quantity = 1;
            if (int.TryParse(s.Text, out quantity) == false)
            {
                s.Text = "1";
            }
        }

        private void optionPicker_Loaded(object sender, RoutedEventArgs e)
        {
            ListPicker p = sender as ListPicker;
            p.SummaryForSelectedItemsDelegate = CreateCommaDelimitedList;
            //p.SelectedItem = p.Items[0];
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Globals.Instance.ordrin.Tray.Count > 0)
                NavigationService.Navigate(new Uri("/TotalPage.xaml", UriKind.Relative));
            else
            {
                string company = Application.Current.Resources["AppName"] as string;
                MessageBox.Show("Please add something to the cart first before checking out.", company, MessageBoxButton.OK);
            }
        }

    }
}