using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Universal.UsersService.Api.Application.Commands;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Gateways;
using Universal.UsersService.Api.Infrastructure.Gateways;

namespace Universal.UsersService.Api.Application.Handlers
{
    public class CreateExternalPostCommandHandler : IRequestHandler<CreateExternalPostCommand, PostResponseDto>
    {
        private readonly IExternalPostGateway _externalPostGateway;

        public CreateExternalPostCommandHandler(IExternalPostGateway externalPostGateway)
        {
            _externalPostGateway = externalPostGateway;
        }

        public async Task<PostResponseDto> Handle(CreateExternalPostCommand request, CancellationToken cancellationToken)
        {
            var externalCreateDto = new ExternalPostCreateDto
            {
                Title = request.Post.Title,
                Body = request.Post.Body,
                UserId = request.Post.UserId
            };
            var created = await _externalPostGateway.CreatePostAsync(externalCreateDto, cancellationToken);
            return new PostResponseDto
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId
            };
        }
    }
}
