using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Queries;
using Universal.UsersService.Api.Application.Gateways;

namespace Universal.UsersService.Api.Application.Handlers
{
    public class GetExternalPostsQueryHandler : IRequestHandler<GetExternalPostsQuery, List<PostResponseDto>>
    {
        private readonly IExternalPostGateway _externalPostGateway;

        public GetExternalPostsQueryHandler(IExternalPostGateway externalPostGateway)
        {
            _externalPostGateway = externalPostGateway;
        }

        public async Task<List<PostResponseDto>> Handle(GetExternalPostsQuery request, CancellationToken cancellationToken)
        {
            var externalPosts = await _externalPostGateway.GetPostsAsync(cancellationToken);
            // Mapeo de DTO externo a DTO de aplicación
            var result = new List<PostResponseDto>();
            foreach (var post in externalPosts)
            {
                result.Add(new PostResponseDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    UserId = post.UserId
                });
            }
            return result;
        }
    }
}
