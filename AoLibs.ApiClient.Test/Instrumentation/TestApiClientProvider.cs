using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using AoLibs.ApiClient.Interfaces;

namespace AoLibs.ApiClient.Test.Instrumentation
{
    public class TestApiClientProvider : IApiClientProvider
    {
        public HttpClient Client { get; } = new HttpClient();
    }
}
