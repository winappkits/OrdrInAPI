using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    public partial class ConfirmPage : PhoneApplicationPage
    {
        public ConfirmPage()
        {
            InitializeComponent();

            this.DataContext = Globals.Instance.ordrin;

            thankText.Text = string.Format("Thank you for ordering from {0} using Ordr.In Software. We've received your order and are confirming it with the merchant now so that it's ready for you to enjoy.", Globals.Instance.ordrin.CurrentRestaurant.Name);

            Globals.Instance.ordrin.LoadOrders();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Globals.Instance.ordrin.Tray.Clear();
            Globals.Instance.ordrin.UpdateTrayCount();

            int cnt = NavigationService.BackStack.Count();
            for (int i = 0; i < cnt - 1; i++)
            {
                NavigationService.RemoveBackEntry();
            }

            base.OnNavigatingFrom(e);
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string fbwall = "I just ordered using Ordr.In Restaurant app.";

            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = fbwall;
            shareStatusTask.Show();
        }

        private void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string tweet = "Thanks @OrdrIn. Ordered from " + Globals.Instance.ordrin.CurrentRestaurant.Name + " & it's going to be awesome!";

            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = tweet;
            shareStatusTask.Show();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Microsoft.Phone.Tasks.PhoneCallTask task = new Microsoft.Phone.Tasks.PhoneCallTask();
            task.DisplayName = Globals.Instance.ordrin.CurrentRestaurant.Name;
            task.PhoneNumber = Globals.Instance.ordrin.CurrentRestaurant.Phone;
            task.Show();
        }
    }
}