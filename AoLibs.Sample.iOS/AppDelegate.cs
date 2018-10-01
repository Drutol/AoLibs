using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Adapters.iOS;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.iOS.Navigation;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.Statics;
using Autofac;
using CoreLocation;
using Foundation;
using UIKit;

namespace AoLibs.Sample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public static AppDelegate Instance { get; set; }

        public NavigationManager<PageIndex> NavigationManager { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Instance = this;
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            InitializationRoutines.Initialize(AdaptersRegistration); 


            return true;
        }

        private void AdaptersRegistration(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(x => NavigationManager).As<INavigationManager<PageIndex>>();
            containerBuilder.RegisterType<MessageBoxProvider>().As<IMessageBoxProvider>().SingleInstance();
            containerBuilder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();
            containerBuilder.RegisterType<FileStorageProvider>().As<IFileStorageProvider>().SingleInstance();
            containerBuilder.RegisterType<VersionProvider>().As<IVersionProvider>().SingleInstance();
            containerBuilder.RegisterType<PickerAdapter>().As<IPickerAdapter>().SingleInstance();
            containerBuilder.RegisterType<DispatcherAdapter>().As<IDispatcherAdapter>().SingleInstance();
            containerBuilder.RegisterType<PhoneCallAdapter>().As<IPhoneCallAdapter>().SingleInstance();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message)
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive.
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

