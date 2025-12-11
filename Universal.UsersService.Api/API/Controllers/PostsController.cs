using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Universal.UsersService.Api.Application.Commands;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Queries;

namespace Universal.UsersService.Api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene todos los posts desde la API externa jsonplaceholder.typicode.com.
        /// Requiere autenticación JWT. Retorna una lista de posts con Id, Título, Contenido y UserId.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PostResponseDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetExternalPostsQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo post en la API externa jsonplaceholder.typicode.com.
        /// Requiere autenticación JWT. El título, contenido y UserId deben ser válidos.
        /// Retorna el post creado con su Id asignado por la API externa.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PostResponseDto), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] PostCreateRequestDto request, CancellationToken cancellationToken)
        {
            var command = new CreateExternalPostCommand { Post = request };
            var result = await _mediator.Send(command, cancellationToken);
            return StatusCode(201, result);
        }
    }
}
