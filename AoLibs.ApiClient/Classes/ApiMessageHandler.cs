using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using MediatR;

namespace AoLibs.ApiClient
{
    public class ApiRequestHandler<TRequest> : IRequestHandler<TRequest, Unit> 
        where TRequest : IApiMessage<TRequest>
    {
        public Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
