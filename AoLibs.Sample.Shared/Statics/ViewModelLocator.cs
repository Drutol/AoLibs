using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Sample.Shared.ViewModels;
using Autofac;

namespace AoLibs.Sample.Shared.Statics
{
    public static class ViewModelLocator
    {
        private static IContainer _container;

        internal static void RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<TestViewModelA>().SingleInstance();
            builder.RegisterType<TestViewModelB>().SingleInstance();
            builder.RegisterType<TestViewModelC>().SingleInstance();

            builder.RegisterType<MainViewModel>().SingleInstance();
        }

        private static void BuildCallback(IContainer obj)
        {
            _container = obj;
        }

        public static MainViewModel MainViewModel => _container.Resolve<MainViewModel>();
    }
}
