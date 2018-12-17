using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AoLibs.ApiClient
{
    public delegate Task<TResponse> HttpResponseConverter<TResponse>(HttpResponseMessage responseMessage);
}
