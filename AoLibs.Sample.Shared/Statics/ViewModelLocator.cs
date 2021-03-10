using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Sample.Shared.DialogViewModels;
using AoLibs.Sample.Shared.ViewModels;
using Autofac;

namespace AoLibs.Sample.Shared.Statics
{
    public static class ViewModelLocator
    {
        private static ILifetimeScope _container;

        internal static void RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<TestViewModelA>().SingleInstance();
            builder.RegisterType<TestViewModelB>();
            builder.RegisterType<TestViewModelC>().SingleInstance();

            builder.RegisterType<TestDialogViewModelA>().SingleInstance();
            builder.RegisterType<TestDialogViewModelB>();

            builder.RegisterType<MainViewModel>().SingleInstance();
        }

        private static void BuildCallback(ILifetimeScope obj)
        {
            _container = obj;
        }

        public static MainViewModel MainViewModel => _container.Resolve<MainViewModel>();
    }
}
