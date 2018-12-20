using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using AoLibs.ApiClient.Test.Models;
using MediatR;
using Newtonsoft.Json;

namespace AoLibs.ApiClient.Test.Instrumentation
{
    public class ApiCommunicator : ApiCommunicatorBase
    {
        public override IApiDefinition ApiDefinition { get; } = new TestApiDefinition();
        public override IApiClientProvider ApiClientProvider { get; } = new TestApiClientProvider();

        public ApiCommunicator(IMediator mediator) : base(mediator)
        {
        }

        [ApiMethod(Path = "/posts/{0}")]
        public Task<DataWrapper<User>> GetUser(int id)
        {
            return Process<DataWrapper<User>>(P(id));
        }

        [ApiMethod(Path = "/users")]
        public Task<PaginatedDataWrapper<List<User>>> GetUsers()
        {
            return Process<PaginatedDataWrapper<List<User>>>();
        }

        [ApiMethod(Path = "/users")]
        public Task PostUser(User user)
        {
            return Process(user);
        }

        [ApiMethod(Path = "/users/{0}")]
        public Task DeleteUser(int id)
        {
            return Process(P(id));
        }
    }
}
