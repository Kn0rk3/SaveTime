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


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SaveTime.view
{
    public sealed partial class SyncPage : Page
    {

        private readonly string[] _scopes =
        {
            "onedrive.readonly",
            "onedrive.appfolder",
            "wl.signin",
        };

        private IOneDriveClient oneDriveClient;
        private string _clientId = "00000000481BA986 ";
        private string _returnUrl = "https://login.live.com/oauth20_desktop.srf";
        private string oneDriveConsumerBaseUrl = "https://api.onedrive.com/v1.0";
        

        public SyncPage()
        {

            this.InitializeComponent();
            this.Name = "Setting";
        }

        private async void AuthenticateClick(object sender, RoutedEventArgs e)
        {
            ShowBusy(true);
            Exception error = null;

            try
            {
                var msaAuthProvider = new MsaAuthenticationProvider(_clientId,_returnUrl,_scopes, /*CredentialCache*/ null, new CredentialVault(_clientId));
                await msaAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();                
                oneDriveClient = new OneDriveClient(this.oneDriveConsumerBaseUrl, msaAuthProvider);
                ShowBusy(false);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                
                
            }
        }

        private void ShowBusy(bool isBusy)
        {
            ProgressRing.IsActive = isBusy;
        }
    }
}
