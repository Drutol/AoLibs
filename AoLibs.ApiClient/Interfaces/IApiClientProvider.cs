using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AoLibs.ApiClient.Interfaces
{
    public interface IApiClientProvider
    {
        HttpClient Client { get; }
    }
}
