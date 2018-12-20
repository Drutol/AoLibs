using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using MediatR;

namespace AoLibs.ApiClient
{
    public class ApiRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IApiRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage(request.HttpMethod, request.ApiDefinition.BaseAddress + request.CurrentPath)
            {
                Content = request.Content
            };

            var response = await request.ApiClientProvider.Client.SendAsync(message, cancellationToken);

            return await request.ToResponse(response);
        }
    }
}
