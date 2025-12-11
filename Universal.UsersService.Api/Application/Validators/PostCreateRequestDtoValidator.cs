using FluentValidation;
using Universal.UsersService.Api.Application.Commands;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Validators
{
    public class PostCreateRequestDtoValidator : AbstractValidator<CreateExternalPostCommand>
    {
        public PostCreateRequestDtoValidator()
        {
            // Se asegura que el objeto 'Post' dentro del comando no sea nulo.
            RuleFor(x => x.Post).NotNull().WithMessage("Los datos del Post son obligatorios.");
            RuleFor(x => x.Post.Title)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MinimumLength(3).WithMessage("El título debe tener al menos 3 caracteres.");

            RuleFor(x => x.Post.Body)
                .NotEmpty().WithMessage("El contenido es obligatorio.")
                .MinimumLength(5).WithMessage("El contenido debe tener al menos 5 caracteres.");

            RuleFor(x => x.Post.UserId)
                .GreaterThan(0).WithMessage("El UserId debe ser mayor que cero.");
        }
    }
}
