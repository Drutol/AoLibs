using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Sample.Shared.BL;
using AoLibs.Sample.Shared.Interfaces;
using Autofac;
using Autofac.Core;

namespace AoLibs.Sample.Shared.Statics
{
    public static class ResourceLocator
    {
        private static ILifetimeScope _scope;

        internal static void RegisterResources(this ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(BuildCallback);

            builder.RegisterType<FancyTrainsProvider>().As<ISomeFancyProvider>().SingleInstance();
            builder.RegisterType<FancyTurtlesProvider>().As<ISomeFancyProvider>().SingleInstance();

            builder.RegisterType<AppVariables>().UsingConstructor(typeof(ISettingsProvider), typeof(IDataCache))
                .SingleInstance();
        }

        private static void BuildCallback(ILifetimeScope obj)
        {
            _scope = obj;
        }

        public static ILifetimeScope ObtainScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }
}
