using FluentValidation;
using GestaoTarefas.Application.DTOs;

namespace GestaoTarefas.Application.Validators;

public class AdicionarComentarioValidator : AbstractValidator<AdicionarComentarioDTO>
{
    public AdicionarComentarioValidator()
    {
        RuleFor(x => x.TarefaId)
            .NotEmpty()
            .WithMessage("O ID da tarefa é obrigatório.");
            
        RuleFor(x => x.Conteudo)
            .NotEmpty()
            .WithMessage("O conteúdo do comentário é obrigatório.")
            .MaximumLength(500)
            .WithMessage("O conteúdo do comentário não pode ter mais de 500 caracteres.");
            
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório.");
    }
}