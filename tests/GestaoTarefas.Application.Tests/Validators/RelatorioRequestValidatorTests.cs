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
        // Arrange
        var request = new RelatorioRequestDTO
        {
            UsuarioId = Guid.NewGuid(),
            EhGerente = true
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ComUsuarioIdInvalido_DeveFalhar()
    {
        // Arrange
        var request = new RelatorioRequestDTO
        {
            UsuarioId = Guid.Empty,
            EhGerente = true
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UsuarioId");
    }
}