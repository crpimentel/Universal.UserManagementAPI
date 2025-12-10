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
        /// Endpoint para registrar un nuevo usuario.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateRequest request)
        {
            try
            {
                var command = new RegisterUserCommand
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password
                };
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para autenticar un usuario.
        /// </summary>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthRequest request)
        {
            try
            {
                var query = new AuthenticateUserQuery
                {
                    Email = request.Email,
                    Password = request.Password
                };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
