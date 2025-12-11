using MediatR;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Commands
{
    public class CreateExternalPostCommand : IRequest<PostResponseDto>
    {
        public PostCreateRequestDto Post { get; set; } = new PostCreateRequestDto();
    }
}
