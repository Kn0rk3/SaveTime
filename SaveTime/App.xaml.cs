using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SaveTime.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OneDrive.Sdk;
using Microsoft.Graph;

namespace SaveTime
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private int appVersion = 1;
        public IOneDriveClient OneDriveClient { get; set; }
        public IAuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            this.SetLocalAndRoamingSettings();
            this.MigrateDatabase();
        }

        private void MigrateDatabase()
        {
            using (SaveTimeDataContext db = new SaveTimeDataContext(Windows.Storage.ApplicationData.Current.LocalSettings.Values["connectionString"].ToString()))
            {
                db.Database.Migrate();

            }
        }

        private void SetLocalAndRoamingSettings()
        {
            //
            // local settings
            //
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["connectionString"] = "Filename = " + Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SaveTimeDB.sqlite");

            //
            // roaming settings
            //
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (!roamingSettings.Values.ContainsKey("askedForOneDriveUsing"))
            {
                roamingSettings.Values["askedForOneDriveUsing"] = false;
            }

            if (!roamingSettings.Values.ContainsKey("usingOneDrive"))
            {
                roamingSettings.Values["usingOneDrive"] = false;
            }

            if (!roamingSettings.Values.ContainsKey("createdOneDriveFolderYet"))
            {
                roamingSettings.Values["createdOneDriveFolderYet"] = false;
            }
        }

        private void SetRoamingFolder()
        {
            Windows.Storage.StorageFolder roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            {
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    this.DebugSettings.EnableFrameRateCounter = true;
                }
#endif

                AppShell shell = Window.Current.Content as AppShell;

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (shell == null)
                {
                    // Create a AppShell to act as the navigation context and navigate to the first page
                    shell = new AppShell();

                    // Set the default language
                    shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                    shell.AppFrame.NavigationFailed += OnNavigationFailed;

                    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        //TODO: Load state from previously suspended application
                        //hier kann erzwungen werden, dass die roaming daten geladen werden
                    }
                }

                // Place our app shell in the current Window
                Window.Current.Content = shell;

                if (shell.AppFrame.Content == null)
                {
                    if (!(bool)Windows.Storage.ApplicationData.Current.RoamingSettings.Values["askedForOneDriveUsing"])
                    {
                        shell.AppFrame.Navigate(typeof(view.SyncPage), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
                    }
                    else
                    {
                        // When the navigation stack isn't restored, navigate to the first page
                        // suppressing the initial entrance animation.
                        shell.AppFrame.Navigate(typeof(view.MainPage), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
                    }
                    
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            //Hier muss die offene Timecard als File in den Roaming Ordner gespeichert werden
            //Die Datenbank muss auf OneDrive gespeichert werden
            deferral.Complete();
        }
    }
}
