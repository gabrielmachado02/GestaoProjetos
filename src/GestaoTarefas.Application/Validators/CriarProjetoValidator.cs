using FluentValidation;
using GestaoTarefas.Application.DTOs;

namespace GestaoTarefas.Application.Validators;

public class CriarProjetoValidator : AbstractValidator<CriarProjetoDTO>
{
    public CriarProjetoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do projeto é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do projeto não pode ter mais de 100 caracteres.");
            
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório.");
    }
}