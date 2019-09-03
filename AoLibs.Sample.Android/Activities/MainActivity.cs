using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.DialogStyles;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Dialogs.Android;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Android.Dialogs;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.Statics;
using Autofac;

namespace AoLibs.Sample.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance)]
    public class MainActivity : AppCompatActivity
    {
        private static bool _initialized;
        public static MainActivity Instance { get; set; }
        private static NavigationManager<PageIndex> _manager;
        private static CustomDialogsManager<DialogIndex> _dialogManager;

        public MainActivity()
        {
            Instance = this;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            ////var pageDefinitions = new Dictionary<PageIndex, IPageProvider<NavigationFragmentBase>>
            ////{
            ////    // cached
            ////    {PageIndex.PageA, new CachedPageProvider<TestPageAFragment>()},
            ////    // oneshots
            ////    {PageIndex.PageB, new OneshotPageProvider<TestPageBFragment>()},
            ////};

            if (!_initialized)
            {
                var dialogDefinitions = new Dictionary<DialogIndex, ICustomDialogProvider>
                {
                    {DialogIndex.TestDialogA, new OneshotCustomDialogProvider<TestDialogA>()},
                    {DialogIndex.TestDialogB, new OneshotCustomDialogProvider<TestDialogB>()}
                };

                _manager = new NavigationManager<PageIndex>(
                    SupportFragmentManager,
                    RootView,
                    new DependencyResolver());

                _dialogManager = new CustomDialogsManager<DialogIndex>(
                    SupportFragmentManager,
                    dialogDefinitions,
                    new DependencyResolver());

                // usually you would do it in Application class but for showcase sake I will skip that
                InitializationRoutines.Initialize(builder =>
                {
                    builder.RegisterType<ClipboardProvider>().As<IClipboardProvider>().SingleInstance();
                    builder.RegisterType<DispatcherAdapter>().As<IDispatcherAdapter>().SingleInstance();
                    builder.RegisterType<FileStorageProvider>().As<IFileStorageProvider>().SingleInstance();
                    builder.RegisterType<MessageBoxProvider>().As<IMessageBoxProvider>().SingleInstance();
                    builder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();
                    builder.RegisterType<UriLauncherAdapter>().As<IUriLauncherAdapter>().SingleInstance();
                    builder.RegisterType<VersionProvider>().As<IVersionProvider>().SingleInstance();
                    builder.RegisterType<PickerAdapter>().As<IPickerAdapter>().SingleInstance();
                    builder.RegisterType<ContextProvider>().As<IContextProvider>().SingleInstance();
                    builder.RegisterType<PhotoPickerAdapter>().As<IPhotoPickerAdapter>().SingleInstance();
                    builder.RegisterType<PhoneCallAdapter>().As<IPhoneCallAdapter>().SingleInstance();
                    builder.RegisterType<DataCache>().As<IDataCache>().SingleInstance();

                    builder.RegisterInstance(_manager).As<INavigationManager<PageIndex>>();
                    builder.RegisterInstance(_dialogManager).As<ICustomDialogsManager<DialogIndex>>();
                });

                DialogStyles.PasswordDialogStyle = new PasswordInputDialogStyle();

                ViewModelLocator.MainViewModel.Initialize();

                _initialized = true;
            }
            else
            {
                _manager.RestoreState(SupportFragmentManager, RootView);
                _dialogManager.ChangeFragmentManager(SupportFragmentManager);
            }
        }

        private class ContextProvider : IContextProvider
        {
            public Activity CurrentActivity => Instance;
            Context IContextProvider.CurrentContext => Instance;
        }

        private class DependencyResolver : IDependencyResolver, ICustomDialogDependencyResolver
        {
            TViewModel IDependencyResolver.Resolve<TViewModel>()
            {
                using (var scope = ResourceLocator.ObtainScope())
                {
                    return scope.Resolve<TViewModel>();
                }
            }

            TViewModel ICustomDialogDependencyResolver.Resolve<TViewModel>() 
            {
                using (var scope = ResourceLocator.ObtainScope())
                {
                    return scope.Resolve<TViewModel>();
                }
            }
        }

        public override void OnBackPressed()
        {
            if (!_manager.OnBackRequested())
            {
                MoveTaskToBack(true);
            }
        }

        #region Views

        private FrameLayout _rootView;

        public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));

        #endregion
    }
}