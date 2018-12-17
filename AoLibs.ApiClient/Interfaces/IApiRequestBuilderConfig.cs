using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.ApiClient.Interfaces
{
    public interface IApiRequestBuilderConfig
    {
        IApiDefinition ApiDefinition { get; }
        IApiClientProvider ApiClientProvider { get; }
    }
}
