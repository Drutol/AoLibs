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
    public class ApiMessageBuilder
    {
        public IApiDefinition ApiDefinition { get; set; }
        public IApiClientProvider ApiClientProvider { get; set; }
        public string PathTemplate { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public HttpContent HttpContent { get; set; }
        public string Tag { get; set; }

        public IApiMessage<ApiMessage> Build()
        {
            if (ApiDefinition is null)
                throw new ArgumentNullException($"{nameof(ApiDefinition)} can't be null.");
            if (ApiClientProvider is null)
                throw new ArgumentNullException($"{nameof(ApiClientProvider)} can't be null.");
            if (HttpMethod is null)
                throw new ArgumentNullException($"{nameof(HttpMethod)} can't be null.");
            if (string.IsNullOrEmpty(PathTemplate))
                throw new ArgumentNullException($"{nameof(PathTemplate)} can't be null or empty.");

            return new ApiMessage(ApiDefinition, ApiClientProvider, PathTemplate, HttpMethod, Tag)
            {
                Content = HttpContent,
            };
        }

        public ApiMessageBuilder WithPathTemplate(string path)
        {
            PathTemplate = path;
            return this;
        }

        public ApiMessageBuilder WithHttpMethod(HttpMethod method)
        {
            HttpMethod = method;
            return this;
        }

        public ApiMessageBuilder WithHttpMethod(string method)
        {
            HttpMethod = new HttpMethod(method);
            return this;
        }

        public ApiMessageBuilder WithContent(HttpContent content)
        {
            HttpContent = content;
            return this;
        }

        public ApiMessageBuilder WithJsonContent(object content)
        {
            HttpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            return this;
        }

        public ApiMessageBuilder WithApiDefinition(IApiDefinition apiDefinition)
        {
            ApiDefinition = apiDefinition;
            return this;
        }

        public ApiMessageBuilder WithApiClient(IApiClientProvider apiClientProvider)
        {
            ApiClientProvider = apiClientProvider;
            return this;
        }

        public ApiMessageBuilder WithConfig(IApiRequestBuilderConfig apiRequestBuilderConfig)
        {
            ApiDefinition = apiRequestBuilderConfig.ApiDefinition;
            ApiClientProvider = apiRequestBuilderConfig.ApiClientProvider;
            return this;
        }

        public ApiMessageBuilder WithTag(string tag)
        {
            Tag = tag;
            return this;
        }

        public ApiMessageBuilder WithCallingMethodInfo(object declaringObject, [CallerMemberName] string callerFunctionName = null)
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
                PathTemplate = attr.Path;

            return this;
        }

        public class ApiMessage : IApiMessage<ApiMessage>
        {
            public string Tag { get; set; }
            public IApiDefinition ApiDefinition { get; }
            public IApiClientProvider ApiClientProvider { get; }
            public HttpMethod HttpMethod { get; }
            public string PathTemplate { get; }
            public string CurrentPath { get; set; }
            public HttpContent Content { get; set; }

            public ApiMessage(IApiDefinition apiDefinition,
                IApiClientProvider apiClientProvider,
                string path,
                HttpMethod httpMethod,
                string tag)
            {
                PathTemplate = path;
                HttpMethod = httpMethod;
                ApiDefinition = apiDefinition;
                ApiClientProvider = apiClientProvider;
                CurrentPath = PathTemplate;

                Tag = string.IsNullOrEmpty(tag) ? $"{GetType().FullName} - {PathTemplate}" : tag;
            }
        }
    }
}
