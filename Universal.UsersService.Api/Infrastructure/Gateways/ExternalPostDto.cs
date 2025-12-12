namespace Universal.UsersService.Api.Infrastructure.Gateways
{
    // DTO que representa la respuesta de la API externa jsonplaceholder.typicode.com/posts
    public class ExternalPostDto
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    // DTO para crear un nuevo post en la API externa
    public class ExternalPostCreateDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
