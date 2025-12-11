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
        private readonly string _baseUrl;

        public ExternalPostGateway(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ExternalApi:PostsUrl"] ?? "https://jsonplaceholder.typicode.com/posts";
        }

        public async Task<List<ExternalPostDto>> GetPostsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ExternalPostDto>>(_baseUrl);
            return response ?? new List<ExternalPostDto>();
        }

        public async Task<ExternalPostDto> CreatePostAsync(ExternalPostCreateDto postDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, postDto);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<ExternalPostDto>();
            return created!;
        }
    }
}
