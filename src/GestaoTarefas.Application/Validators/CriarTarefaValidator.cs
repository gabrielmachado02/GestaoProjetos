using FluentValidation;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Domain.Enums;

namespace GestaoTarefas.Application.Validators;

public class CriarTarefaValidator : AbstractValidator<CriarTarefaDTO>
{
    public CriarTarefaValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty()
            .WithMessage("O título da tarefa é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O título da tarefa não pode ter mais de 100 caracteres.");
            
        RuleFor(x => x.DataVencimento)
            .NotEmpty()
            .WithMessage("A data de vencimento é obrigatória.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("A data de vencimento deve ser maior ou igual à data atual.");
            
        RuleFor(x => x.Prioridade)
            .IsInEnum()
            .WithMessage("Prioridade inválida.");
            
        RuleFor(x => x.ProjetoId)
            .NotEmpty()
            .WithMessage("O ID do projeto é obrigatório.");
    }
}