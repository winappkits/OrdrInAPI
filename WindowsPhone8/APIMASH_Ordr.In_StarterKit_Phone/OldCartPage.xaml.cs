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
    public partial class OldCartPage : PhoneApplicationPage
    {
        public OldCartPage()
        {
            InitializeComponent();
            this.DataContext = Globals.Instance.ordrin;

        }

    }
}