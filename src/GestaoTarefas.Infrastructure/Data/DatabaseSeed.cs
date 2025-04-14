using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GestaoTarefas.Infrastructure.Data;

public static class DatabaseSeed
{
    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await dbContext.Database.MigrateAsync();
            
            if (await dbContext.Projetos.AnyAsync())
            {
                logger.LogInformation("O banco de dados já está populado. Pulando seed.");
                return;
            }

            logger.LogInformation("Iniciando seed do banco de dados...");

            var usuarioIdComum = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var gerenteId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var projetoDesenvolvimento = new Projeto("Desenvolvimento Sistema", "Projeto para desenvolvimento do novo sistema de gestão", gerenteId);
            var projetoInfra = new Projeto("Infraestrutura", "Projeto para atualização da infraestrutura de servidores", gerenteId);
            var projetoPessoal = new Projeto("Tarefas Pessoais", "Organização de tarefas pessoais", usuarioIdComum);

            dbContext.Projetos.AddRange(projetoDesenvolvimento, projetoInfra, projetoPessoal);
            await dbContext.SaveChangesAsync();

            var tarefaAnalise = new Tarefa(
                "Análise de requisitos", 
                "Analisar e documentar todos os requisitos do novo sistema", 
                DateTime.Today.AddDays(7), 
                PrioridadeTarefa.Alta, 
                projetoDesenvolvimento.Id);
                
            var tarefaDesign = new Tarefa(
                "Design da interface", 
                "Criar wireframes e protótipos da interface do usuário", 
                DateTime.Today.AddDays(14), 
                PrioridadeTarefa.Media, 
                projetoDesenvolvimento.Id);
                
            var tarefaImplementacao = new Tarefa(
                "Implementação do backend", 
                "Desenvolver a API REST para o novo sistema", 
                DateTime.Today.AddDays(30), 
                PrioridadeTarefa.Alta, 
                projetoDesenvolvimento.Id);
                
            dbContext.Tarefas.AddRange(tarefaAnalise, tarefaDesign, tarefaImplementacao);
            
            var tarefaServidores = new Tarefa(
                "Atualização de servidores", 
                "Atualizar os servidores para a nova versão do sistema operacional", 
                DateTime.Today.AddDays(15), 
                PrioridadeTarefa.Alta, 
                projetoInfra.Id);
                
            var tarefaBackup = new Tarefa(
                "Configurar sistema de backup", 
                "Implementar novo sistema de backup automatizado", 
                DateTime.Today.AddDays(10), 
                PrioridadeTarefa.Media, 
                projetoInfra.Id);
                
            dbContext.Tarefas.AddRange(tarefaServidores, tarefaBackup);
            
            var tarefaEstudo = new Tarefa(
                "Estudar .NET 8", 
                "Concluir o curso de .NET 8 online", 
                DateTime.Today.AddDays(20), 
                PrioridadeTarefa.Media, 
                projetoPessoal.Id);
                
            var tarefaExercicio = new Tarefa(
                "Exercícios físicos", 
                "Manter rotina de exercícios 3x por semana", 
                DateTime.Today.AddDays(1), 
                PrioridadeTarefa.Baixa, 
                projetoPessoal.Id);
                
            dbContext.Tarefas.AddRange(tarefaEstudo, tarefaExercicio);
            
            await dbContext.SaveChangesAsync();
            
            tarefaAnalise.AdicionarComentario("Reunião com stakeholders marcada para segunda-feira", gerenteId);
            tarefaAnalise.AdicionarComentario("Documento de requisitos inicial criado", usuarioIdComum);
            
            tarefaImplementacao.AdicionarComentario("Arquitetura definida usando Clean Architecture", gerenteId);
            
            tarefaServidores.AdicionarComentario("Janela de manutenção aprovada para o próximo final de semana", gerenteId);
            
            tarefaAnalise.AtualizarStatus(StatusTarefa.EmAndamento, usuarioIdComum);
            tarefaDesign.AtualizarStatus(StatusTarefa.EmAndamento, usuarioIdComum);
            
            await dbContext.SaveChangesAsync();
            
            logger.LogInformation("Seed do banco de dados concluído com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante o seed do banco de dados.");
            throw;
        }
    }
}