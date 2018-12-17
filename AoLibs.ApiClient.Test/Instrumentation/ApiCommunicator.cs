using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoLibs.ApiClient.Interfaces;
using AoLibs.ApiClient.Test.Models;
using MediatR;

namespace AoLibs.ApiClient.Test.Instrumentation
{
    public class ApiCommunicator : IApiRequestBuilderConfig
    {
        private readonly IMediator _mediator;
        private IApiRequest<Post> _postApiRequest;
        private IApiRequest<List<Post>> _postsApiRequest;

        public IApiDefinition ApiDefinition { get; } = new TestApiDefinition();
        public IApiClientProvider ApiClientProvider { get; } = new TestApiClientProvider();

        public ApiCommunicator(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ApiMethod]
        public async Task<Post> GetPost(int id)
        {
            if (_postApiRequest is null)
            {
                _postApiRequest = new ApiRequestBuilder<Post>()
                    .WithConfig(this)
                    .WithCallingMethodInfo(this)
                    .WithDefaultJsonResponseConverter()
                    .Build();
            }

            _postApiRequest.Path = $"/posts/{id}";

            return await _mediator.Send(_postApiRequest);
        }

        [ApiMethod(Path = "/posts")]
        public async Task<List<Post>> GetPosts()
        {
            if (_postsApiRequest is null)
            {
                _postsApiRequest = new ApiRequestBuilder<List<Post>>()
                    .WithConfig(this)
                    .WithCallingMethodInfo(this)
                    .WithDefaultJsonResponseConverter()
                    .Build();
            }

            return await _mediator.Send(_postsApiRequest);
        }
    }
}
