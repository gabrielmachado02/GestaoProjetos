using FluentValidation.AspNetCore;
using GestaoTarefas.Application;
using GestaoTarefas.Infrastructure;
using GestaoTarefas.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configurar validação do FluentValidation para retornar BadRequest (400)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            Message = "Erros de validação",
            Errors = errors
        });
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application services
builder.Services.AddApplication();

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseSwagger();  // Gera a documentação da API
app.UseSwaggerUI(c =>  // Gera a interface visual do Swagger
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestaoTarefas API V1");
    c.RoutePrefix = string.Empty;
});
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
   

    // Inicializar e fazer seed do banco de dados
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            // Apply migrations
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            
            // Executar seed dos dados
            logger.LogInformation("Iniciando seed de dados...");
            GestaoTarefas.Infrastructure.DependencyInjection.SeedDataAsync(app.Services).Wait();
            logger.LogInformation("Seed de dados concluído com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a inicialização do banco de dados.");
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();