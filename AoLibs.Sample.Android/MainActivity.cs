using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using AoLibs.Adapters.Android;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.Core.PageProviders;
using AoLibs.Sample.Android.Fragments;
using AoLibs.Sample.Shared;
using AoLibs.Sample.Shared.Statics;
using Autofac;
using NavigationLib.Android.Navigation;

namespace AoLibs.Sample.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static MainActivity Instance { get; set; }

        public MainActivity()
        {
            Instance = this;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //var pageDefinitions = new Dictionary<PageIndex, IPageProvider<NavigationFragmentBase>>
            //{
            //    // cached
            //    {PageIndex.PageA, new CachedPageProvider<TestPageAFragment>()},
            //    // oneshots
            //    {PageIndex.PageB, new OneshotPageProvider<TestPageBFragment>()},
            //};

            var manager = new NavigationManager<PageIndex>(
                SupportFragmentManager,
                RootView);

            NavigationFragmentBase.ViewModelResolver = new ViewModelResolver();

            //usually you would do it in Application class but for showcase sake I will skip that
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

                builder.RegisterInstance(manager).As<INavigationManager<PageIndex>>();
            });

            ViewModelLocator.MainViewModel.Initialize();
        }

        class ContextProvider : IContextProvider
        {
            public Activity CurrentContext => Instance;
        }

        class ViewModelResolver : IViewModelResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                using (var scope = ResourceLocator.ObtainScope())
                {
                    return scope.Resolve<TViewModel>();
                }
            }
        }

        #region Views

        private FrameLayout _rootView;

        public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));

        #endregion
    }
}

