using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AoLibs.ApiClient.Interfaces
{
    public interface IApiRequestBase
    {
        string Tag { get; }

        IApiDefinition ApiDefinition { get; }
        IApiClientProvider ApiClientProvider { get; }

        HttpMethod HttpMethod { get; }

        string Path { get; set; }
        HttpContent Content { get; set; }
    }
}
