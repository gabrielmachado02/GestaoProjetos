using GestaoTarefas.Domain.Repositories;
using GestaoTarefas.Infrastructure.Data;
using GestaoTarefas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoTarefas.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IProjetoRepository, ProjetoRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();
        services.AddScoped<IRelatorioRepository, RelatorioRepository>();

        return services;
    }
    
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        await DatabaseSeed.SeedDatabaseAsync(serviceProvider);
    }
}