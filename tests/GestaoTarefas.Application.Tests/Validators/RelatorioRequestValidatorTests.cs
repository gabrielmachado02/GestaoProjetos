using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Validators;
using Xunit;

namespace GestaoTarefas.Application.Tests.Validators;

public class RelatorioRequestValidatorTests
{
    private readonly RelatorioRequestValidator _validator;

    public RelatorioRequestValidatorTests()
    {
        _validator = new RelatorioRequestValidator();
    }

    [Fact]
    public void Validate_ComUsuarioIdValido_DevePassar()
    {

        var request = new RelatorioRequestDTO
        {
            UsuarioId = Guid.NewGuid(),
            EhGerente = true
        };


        var result = _validator.Validate(request);


        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ComUsuarioIdInvalido_DeveFalhar()
    {

        var request = new RelatorioRequestDTO
        {
            UsuarioId = Guid.Empty,
            EhGerente = true
        };


        var result = _validator.Validate(request);


        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UsuarioId");
    }
}