using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace AoLibs.Sample.Shared.Statics
{
    public static class InitializationRoutines
    {
        public static IContainer Container { get; set; }

        public static void Initialize(Action<ContainerBuilder> adaptersRegistration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterResources();
            builder.RegisterViewModels();
            adaptersRegistration(builder);
            Container = builder.Build();
        }    
    }
}
