using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveTime.viewmodel
{
    class SyncPageVM : INotifyPropertyChanged
    {
        private IOneDriveClient oneDriveClient { get; set; }

        

        private static readonly string[] _scopes = { "onedrive.readwrite", "onedrive.appfolder", "wl.offline_access", "wl.signin" };

        private string _clientId = "00000000481BA986";
        private string _returnUrl = "https://login.live.com/oauth20_desktop.srf";
        private string oneDriveConsumerBaseUrl = "https://api.onedrive.com/v1.0";

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }

        public SyncPageVM()
        {

        }


        public async void AuthenticateOneDrive()
        {
            Exception error = null;
            if (this.oneDriveClient == null)
            {
                try
                {
                    this.IsBusy = true;
                    var msaAuthProvider = new MsaAuthenticationProvider(_clientId, _returnUrl, _scopes, /*CredentialCache*/ null, new CredentialVault(_clientId));
                    await msaAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                    oneDriveClient = new OneDriveClient(this.oneDriveConsumerBaseUrl, msaAuthProvider);
                    this.IsBusy = false;
                }
                catch (Exception ex)
                {

                }
                if (error != null)
                {


                }
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
