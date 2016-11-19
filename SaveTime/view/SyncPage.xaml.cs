using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk.Authentication;
using SaveTime.viewmodel;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SaveTime.view
{
    public sealed partial class SyncPage : Page
    {
        private SyncPageVM syncPageViewModel;

        public SyncPage()
        {
            //this.DataContext = SyncPageVM
            this.InitializeComponent();
            this.Name = "OneDrive";
        }

        //private void AuthenticateClick(object sender, RoutedEventArgs e)
        //{
        //    syncPageViewModel.AuthenticateOneDrive();
        //}
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            syncPageViewModel = new SyncPageVM();
            this.DataContext = syncPageViewModel;

            // Call the base method, to execute the rest of the navigation event
            base.OnNavigatedTo(e);
        }

        private void useOneDriveToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

            syncPageViewModel.AuthenticateOneDrive();
        }

        //        public class RestaurantParams
        //        {
        //            public string Name { get; set; }
        //            public string Text { get; set; }
        //            // ...
        //        }
        //        And then pass it via:
        //var parameters = new RestaurantParams();
        //        parameters.Name = "Lorem ipsum";
        //parameters.Text = "Dolor sit amet.";
        //// ...

        //Frame.Navigate(typeof(PageTwo), parameters);
        //On your next page you can now access them via:
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //        {
        //            base.OnNavigatedTo(e);

        //            var parameters = (RestaurantParams)e.Parameter;

        //            // parameters.Name
        //            // parameters.Text
        //            // ...
        //        }
    }
}
