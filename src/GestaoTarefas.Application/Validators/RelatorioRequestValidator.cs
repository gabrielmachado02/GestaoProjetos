using FluentValidation;
using GestaoTarefas.Application.DTOs;

namespace GestaoTarefas.Application.Validators;

public class RelatorioRequestValidator : AbstractValidator<RelatorioRequestDTO>
{
    public RelatorioRequestValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório.");
    }
}