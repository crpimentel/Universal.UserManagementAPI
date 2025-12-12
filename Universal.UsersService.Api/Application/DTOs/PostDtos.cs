namespace Universal.UsersService.Api.Application.DTOs
{
    // DTO para la respuesta final de la API interna
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    // DTO para la creación de un post desde la API interna
    public class PostCreateRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
