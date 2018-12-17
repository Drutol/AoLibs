using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AoLibs.ApiClient.Interfaces
{
    public interface IApiRequest<TResponse> : IApiRequestBase, IRequest<TResponse>
    {
        Task<TResponse> ToResponse(HttpResponseMessage httpResponseMessage);
    }
}
