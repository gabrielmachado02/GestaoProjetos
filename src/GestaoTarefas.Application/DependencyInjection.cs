using System.Reflection;
using FluentValidation;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Application.Services;
using GestaoTarefas.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoTarefas.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Registrar serviços
        services.AddScoped<IProjetoService, ProjetoService>();
        services.AddScoped<ITarefaService, TarefaService>();
        services.AddScoped<IRelatorioService, RelatorioService>();
        
        // Registrar validadores
        services.AddScoped<IValidator<RelatorioRequestDTO>, RelatorioRequestValidator>();
        services.AddScoped<IValidator<CriarProjetoDTO>, CriarProjetoValidator>();
        services.AddScoped<IValidator<CriarTarefaDTO>, CriarTarefaValidator>();
        services.AddScoped<IValidator<AtualizarTarefaDTO>, AtualizarTarefaValidator>();
        services.AddScoped<IValidator<AdicionarComentarioDTO>, AdicionarComentarioValidator>();
        
        // Registrar validação automática para controllers
        //services.AddFluentValidationAutoValidation();
        
        return services;
    }
}