using GestaoTarefas.Api.Controllers;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestaoTarefas.Api.Tests.Controllers;

public class RelatoriosControllerTests
{
    private readonly Mock<IRelatorioService> _relatorioServiceMock;
    private readonly RelatoriosController _controller;

    public RelatoriosControllerTests()
    {
        _relatorioServiceMock = new Mock<IRelatorioService>();
        _controller = new RelatoriosController(_relatorioServiceMock.Object);
    }

    [Fact]
    public async Task GetMediaTarefasConcluidasPorUsuario_QuandoUsuarioGerente_DeveRetornarOk()
    {
        // Arrange
        var media = 2.5;
        var usuarioId = Guid.NewGuid();
        
        _relatorioServiceMock.Setup(s => s.ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync())
            .ReturnsAsync(media);
            
        var request = new RelatorioRequestDTO
        {
            EhGerente = true,
            UsuarioId = usuarioId
        };

        // Act
        var result = await _controller.GetMediaTarefasConcluidasPorUsuario(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<dynamic>(okResult.Value);
        
        Assert.Equal(media, returnedData.MediaTarefasConcluidasPorUsuario);
        Assert.Equal(usuarioId, returnedData.UsuarioId);
        Assert.Equal("Últimos 30 dias", returnedData.Periodo);
    }

    [Fact]
    public async Task GetMediaTarefasConcluidasPorUsuario_QuandoUsuarioNaoGerente_DeveRetornarForbid()
    {
        // Arrange
        // Não configurar o serviço pois não deveria ser chamado
        
        var request = new RelatorioRequestDTO
        {
            EhGerente = false,
            UsuarioId = Guid.NewGuid()
        };

        // Act
        var result = await _controller.GetMediaTarefasConcluidasPorUsuario(request);

        // Assert
        Assert.IsType<ForbidResult>(result.Result);
        _relatorioServiceMock.Verify(s => s.ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync(), Times.Never);
    }
}