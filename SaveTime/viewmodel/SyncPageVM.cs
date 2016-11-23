using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace SaveTime.viewmodel
{
    class SyncPageVM : INotifyPropertyChanged
    {
        private IOneDriveClient oneDriveClient { get; set; }
        private AccountSession clientSession;


        private static string[] _scopes = { "onedrive.readwrite", "onedrive.appfolder", "wl.offline_access", "wl.signin" };

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

        private bool isUsingOneDrive;
        public bool IsUsingOneDrive
        {
            get
            {
                return isUsingOneDrive;
            }

            set
            {
                isUsingOneDrive = value;
                NotifyPropertyChanged("IsUsingOneDrive");
            }
        }

        public SyncPageVM()
        {
            IsUsingOneDrive = (bool)Windows.Storage.ApplicationData.Current.RoamingSettings.Values["usingOneDrive"];
            if (isUsingOneDrive)
            {
                oneDriveClient = ((App)Application.Current).OneDriveClient;
            }
        }

        public void RemoveOneDriveConnection()
        {
            this.IsBusy = true;
            var app = (App)Application.Current;
            try
            {
                var msaAuthProvider = app.AuthProvider as MsaAuthenticationProvider;
                msaAuthProvider.SignOutAsync();
            }
            catch
            {

            }
            finally
            {
                app.AuthProvider = null;
                app.OneDriveClient = null;
            }
            this.IsBusy = false;
        }

        public void InitializeOneDriveConnection()
        {
            this.IsBusy = true;
            InitializeClient();
            this.IsBusy = false;
        }

        //public async Task AuthenticateOneDrive()
        //{
        //    Exception error = null;
        //    if (this.oneDriveClient == null)
        //    {
        //        try
        //        {
        //            this.IsBusy = true;
        //            var msaAuthProvider = new MsaAuthenticationProvider(_clientId, _returnUrl, _scopes, /*CredentialCache*/ null, new CredentialVault(_clientId));
        //            await msaAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
        //            oneDriveClient = new OneDriveClient(this.oneDriveConsumerBaseUrl, msaAuthProvider);

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        if (error != null)
        //        {


        //        }
        //    }
        //}

        //public async void GetAppRoot()
        //{
        //    this.IsBusy = true;
        //    if (oneDriveClient == null)
        //    {
        //        AuthenticateOneDrive();
        //    }

        //    //await GetFolder(oneDriveClient.Drive.Special.AppRoot, true);
        //    var rootItem = await oneDriveClient.Drive.Root.Request().GetAsync();
        //    this.IsBusy = false;
        //}


        private async void InitializeClient()
        {
            var app = (App)Application.Current;
            if (app.OneDriveClient == null)
            {
                Task authTask;

                var msaAuthProvider = new MsaAuthenticationProvider(
                        this._clientId ,
                        this._returnUrl,
                        _scopes,
                        new CredentialVault(this._clientId));
                authTask = msaAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                app.OneDriveClient = new OneDriveClient(this.oneDriveConsumerBaseUrl, msaAuthProvider);
                app.AuthProvider = msaAuthProvider;
                try
                {
                    await authTask;
                }
                catch (ServiceException exception)
                {
                    // Swallow the auth exception but write message for debugging.
                    Debug.WriteLine(exception.Error.Message);
                }
            }
        }

        //private async Task GetFolder(IItemRequestBuilder builder, bool childrenToo)
        //{
        //    this.IsBusy = true;
        //    Exception error = null;
        //    //IChildrenCollectionPage children = null;

        //    try
        //    {
        //        var root = await builder.Request().GetAsync();

        //        //if (childrenToo)
        //        //{
        //        //    children = await builder.Children.Request().GetAsync();
        //        //}

        //        //DisplayHelper.ShowContent(
        //        //    "SHOW FOLDER ++++++++++++++++++++++",
        //        //    root,
        //        //    children,
        //        //    async message =>
        //        //    {
        //        //        var dialog = new MessageDialog(message);
        //        //        await dialog.ShowAsync();
        //        //    });

        //        this.IsBusy = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex;
        //    }

        //    if (error != null)
        //    {
        //        var dialog = new MessageDialog(error.Message, "Error!");
        //        await dialog.ShowAsync();
        //        this.IsBusy = false;
        //        return;
        //    }
        //}

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
