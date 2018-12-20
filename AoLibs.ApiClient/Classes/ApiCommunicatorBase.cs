using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace AoLibs.ApiClient
{
    public abstract class ApiCommunicatorBase : IApiRequestBuilderConfig
    {
        private readonly IMediator _mediator;

        public abstract IApiDefinition ApiDefinition { get; }
        public abstract IApiClientProvider ApiClientProvider { get; }

        private readonly Dictionary<string, IApiRequestBase> _apiRequests = new Dictionary<string, IApiRequestBase>();

        protected ApiCommunicatorBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Requests

        protected Task<T> Process<T>(HttpContent content, [CallerMemberName] string caller = null)
        {
            return Process<T>(null, content, caller);
        }

        protected Task<TResponse> Process<TResponse>(object[] pathParameters, object body, [CallerMemberName] string caller = null)
        {
            return Process<TResponse>(
                pathParameters,
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
                caller);
        }

        protected Task<TResponse> Process<TResponse>(object body, [CallerMemberName] string caller = null)
        {
            return Process<TResponse>(
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
                caller);
        }

        protected virtual async Task<T> Process<T>(object[] pathParameters = null, HttpContent content = null, [CallerMemberName] string caller = null)
        {
            if (!_apiRequests.ContainsKey(caller))
                _apiRequests[caller] = RequestFromDefaultBuilder<T>(caller);

            var request = _apiRequests[caller];

            if (pathParameters != null)
                request.CurrentPath = string.Format(request.PathTemplate, pathParameters);

            request.Content = content;

            return await _mediator.Send(request as IApiRequest<T>);
        }

        #endregion

        #region Messages

        protected Task Process(HttpContent content, [CallerMemberName] string caller = null)
        {
            return Process(null, content, caller);
        }

        protected Task Process(object[] pathParameters, object body, [CallerMemberName] string caller = null) 
        {
            return Process(
                pathParameters,
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
                caller);
        }

        protected Task Process(object body, [CallerMemberName] string caller = null)
        {
            return Process(
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
                caller);
        }

        protected virtual Task Process(object[] pathParameters = null, HttpContent content = null, [CallerMemberName] string caller = null)
        {
            if (!_apiRequests.ContainsKey(caller))
                _apiRequests[caller] = MessageFromDefaultBuilder(caller);

            var request = _apiRequests[caller];

            if (pathParameters != null)
                request.CurrentPath = string.Format(request.PathTemplate, pathParameters);

            request.Content = content;

            return _mediator.Send(request as IApiMessage<ApiMessageBuilder.ApiMessage>);
        }

        #endregion

        #region Builders

        protected virtual IApiRequest<T> RequestFromDefaultBuilder<T>([CallerMemberName] string caller = null)
        {
            return new ApiRequestBuilder<T>()
                .WithConfig(this)
                .WithCallingMethodInfo(this, caller)
                .WithDefaultJsonResponseConverter()
                .Build();
        }

        protected virtual IApiMessage<ApiMessageBuilder.ApiMessage> MessageFromDefaultBuilder([CallerMemberName] string caller = null)
        {
            return new ApiMessageBuilder()
                .WithConfig(this)
                .WithCallingMethodInfo(this, caller)
                .Build();
        }

        #endregion

        protected static object[] P(params object[] parameters) => parameters;
    }
}
