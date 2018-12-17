using System;
using System.Reflection;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using AoLibs.ApiClient.Test.Instrumentation;
using Autofac;
using MediatR;
using Xunit;

namespace AoLibs.ApiClient.Test
{
    public class ApiClientTest
    {
        [Fact]
        public async Task Test1()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterGeneric(typeof(ApiRequestHandler<>)).As(typeof(IRequestHandler<,>));
            builder.RegisterGeneric(typeof(ApiRequestHandler<,>)).As(typeof(IRequestHandler<,>));

            builder.RegisterType()

            var container = builder.Build();

            var mediator = container.Resolve<IMediator>();
            
            var communicator = new ApiCommunicator(mediator);

            var posts = await communicator.GetPosts();


        }
    }
}
