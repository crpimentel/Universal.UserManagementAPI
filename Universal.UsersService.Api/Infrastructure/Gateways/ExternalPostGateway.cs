using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Gateways;
using Universal.UsersService.Api.Infrastructure.Gateways;

namespace Universal.UsersService.Api.Infrastructure.Gateways
{
    public class ExternalPostGateway : IExternalPostGateway
    {


        private readonly HttpClient _httpClient;
        private readonly string _postsPath;

        public ExternalPostGateway(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Se recomienda configurar BaseAddress en Program.cs y solo el path aquí
            _postsPath = configuration["ExternalApi:PostsPath"] ?? "/posts";
        }

        public async Task<List<ExternalPostDto>> GetPostsAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(_postsPath, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var posts = await response.Content.ReadFromJsonAsync<List<ExternalPostDto>>(cancellationToken: cancellationToken);
                    return posts ?? new List<ExternalPostDto>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ExternalApiNotFoundException("Posts not found in external API.");
                }
                else
                {
                    throw new ExternalApiException($"External API error: {response.StatusCode}");
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExternalApiTimeoutException("External API request timed out.");
            }
        }

        public async Task<ExternalPostDto> CreatePostAsync(ExternalPostCreateDto postDto, System.Threading.CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_postsPath, postDto, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var created = await response.Content.ReadFromJsonAsync<ExternalPostDto>(cancellationToken: cancellationToken);
                    return created!;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ExternalApiBadRequestException("Bad request to external API.");
                }
                else
                {
                    throw new ExternalApiException($"External API error: {response.StatusCode}");
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExternalApiTimeoutException("External API request timed out.");
            }
        }
    }
}
