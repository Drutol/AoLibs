using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using Newtonsoft.Json;

namespace AoLibs.ApiClient
{
    public class ApiRequestBuilder<TResponse>
    {
        public IApiDefinition ApiDefinition { get; set; }
        public IApiClientProvider ApiClientProvider { get; set; }
        public HttpResponseConverter<TResponse> HttpResponseConverter { get; set; }
        public string Path { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public HttpContent HttpContent { get; set; }
        public string Tag { get; set; }

        public IApiRequest<TResponse> Build()
        {
            if (ApiDefinition is null)
                throw new ArgumentNullException($"{nameof(ApiDefinition)} can't be null.");
            if (ApiClientProvider is null)
                throw new ArgumentNullException($"{nameof(ApiClientProvider)} can't be null.");
            if (HttpResponseConverter is null)
                throw new ArgumentNullException($"{nameof(HttpResponseConverter)} can't be null.");
            if (HttpMethod is null)
                throw new ArgumentNullException($"{nameof(HttpMethod)} can't be null.");
            if (string.IsNullOrEmpty(Path))
                throw new ArgumentNullException($"{nameof(Path)} can't be null or empty.");

            return new ApiRequest(ApiDefinition, ApiClientProvider, HttpResponseConverter, Path, HttpMethod, Tag)
            {
                Content = HttpContent,
            };
        }

        public ApiRequestBuilder<TResponse> WithPath(string path)
        {
            Path = path;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithHttpMethod(HttpMethod method)
        {
            HttpMethod = method;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithHttpMethod(string method)
        {
            HttpMethod = new HttpMethod(method);
            return this;
        }

        public ApiRequestBuilder<TResponse> WithContent(HttpContent content)
        {
            HttpContent = content;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithJsonContent(object content)
        {
            HttpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            return this;
        }

        public ApiRequestBuilder<TResponse> WithApiDefinition(IApiDefinition apiDefinition)
        {
            ApiDefinition = apiDefinition;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithApiClient(IApiClientProvider apiClientProvider)
        {
            ApiClientProvider = apiClientProvider;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithResponseConverter(HttpResponseConverter<TResponse> httpResponseConverter)
        {
            HttpResponseConverter = httpResponseConverter;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithDefaultJsonResponseConverter()
        {
            HttpResponseConverter = async message =>
                JsonConvert.DeserializeObject<TResponse>(await message.Content.ReadAsStringAsync());
            return this;
        }

        public ApiRequestBuilder<TResponse> WithConfig(IApiRequestBuilderConfig apiRequestBuilderConfig)
        {
            ApiDefinition = apiRequestBuilderConfig.ApiDefinition;
            ApiClientProvider = apiRequestBuilderConfig.ApiClientProvider;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithTag(string tag)
        {
            Tag = tag;
            return this;
        }

        public ApiRequestBuilder<TResponse> WithCallingMethodInfo(object declaringObject, [CallerMemberName] string callerFunctionName = null)
        {
            if (string.IsNullOrEmpty(callerFunctionName))
                throw new ArgumentNullException(nameof(callerFunctionName));

            var callingMethod = declaringObject
                .GetType()
                .GetMethod(callerFunctionName);

            var attr = callingMethod.GetCustomAttribute<ApiMethodAttribute>();

            if (attr is null)
            {
                throw new ArgumentException($"Calling method {callingMethod.Name} doesn't have {nameof(ApiMethodAttribute)} attached.");
            }

            HttpMethod = attr.HttpMethod;
            if(!string.IsNullOrEmpty(attr.Path))
                Path = attr.Path;

            return this;
        }

        class ApiRequest : IApiRequest<TResponse>
        {
            private readonly HttpResponseConverter<TResponse> _responseConverter;

            public string Tag { get; set; }
            public IApiDefinition ApiDefinition { get; }
            public IApiClientProvider ApiClientProvider { get; }
            public HttpMethod HttpMethod { get; }
            public string Path { get; set; }
            public HttpContent Content { get; set; }

            public ApiRequest(IApiDefinition apiDefinition,
                IApiClientProvider apiClientProvider,
                HttpResponseConverter<TResponse> responseConverter,
                string path,
                HttpMethod httpMethod,
                string tag)
            {
                _responseConverter = responseConverter;
                Path = path;
                HttpMethod = httpMethod;
                ApiDefinition = apiDefinition;
                ApiClientProvider = apiClientProvider;

                Tag = string.IsNullOrEmpty(tag) ? $"{GetType().FullName} - {Path}" : tag;
            }

            public Task<TResponse> ToResponse(HttpResponseMessage httpResponseMessage)
            {
                return _responseConverter(httpResponseMessage);
            }
        }
    }
}
