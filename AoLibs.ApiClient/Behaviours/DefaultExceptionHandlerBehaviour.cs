using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AoLibs.ApiClient.Behaviours
{
    class DefaultExceptionHandlerBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;
            try
            {
                response = await next();
            }
            catch (Exception e)
            {
                throw;
            }

            return response;
        }
    }
}
