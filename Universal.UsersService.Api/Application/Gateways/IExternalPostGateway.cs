using System.Collections.Generic;
using System.Threading.Tasks;
using Universal.UsersService.Api.Infrastructure.Gateways;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Gateways
{
    public interface IExternalPostGateway
    {
        Task<List<ExternalPostDto>> GetPostsAsync(System.Threading.CancellationToken cancellationToken = default);
        Task<ExternalPostDto> CreatePostAsync(ExternalPostCreateDto postDto, System.Threading.CancellationToken cancellationToken = default);
    }
}
