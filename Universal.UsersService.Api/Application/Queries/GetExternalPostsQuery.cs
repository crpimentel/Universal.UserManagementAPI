using MediatR;
using System.Collections.Generic;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Queries
{
    public class GetExternalPostsQuery : IRequest<List<PostResponseDto>>
    {
    }
}
