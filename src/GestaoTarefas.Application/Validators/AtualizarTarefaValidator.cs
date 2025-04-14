using FluentValidation;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Domain.Enums;

namespace GestaoTarefas.Application.Validators;

public class AtualizarTarefaValidator : AbstractValidator<AtualizarTarefaDTO>
{
    public AtualizarTarefaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID da tarefa é obrigatório.");
            
        RuleFor(x => x.Titulo)
            .NotEmpty()
            .WithMessage("O título da tarefa é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O título da tarefa não pode ter mais de 100 caracteres.");
            
        RuleFor(x => x.DataVencimento)
            .NotEmpty()
            .WithMessage("A data de vencimento é obrigatória.");
            
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status inválido.");
            
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório.");
    }
}