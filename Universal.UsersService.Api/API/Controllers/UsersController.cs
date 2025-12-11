using Microsoft.AspNetCore.Mvc;
using Universal.UsersService.Api.Application.DTOs;
using MediatR;
using Universal.UsersService.Api.Application.Commands;
using Universal.UsersService.Api.Application.Queries;

namespace Universal.UsersService.Api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// Valida nombre, email y contraseña. Retorna el usuario creado y un JWT si es exitoso.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserCreateResponse), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] UserCreateRequest request)
        {
            var command = new RegisterUserCommand
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };
            var result = await _mediator.Send(command);
            return StatusCode(201, result);
        }

        /// <summary>
        /// Autentica un usuario y retorna un JWT si las credenciales son válidas.
        /// </summary>
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(UserAuthResponse), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthRequest request)
        {
            var query = new AuthenticateUserQuery
            {
                Email = request.Email,
                Password = request.Password
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
