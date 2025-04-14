using GestaoTarefas.Api.Controllers;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestaoTarefas.Api.Tests.Controllers;

public class ProjetosControllerTests
{
    private readonly Mock<IProjetoService> _projetoServiceMock;
    private readonly ProjetosController _controller;

    public ProjetosControllerTests()
    {
        _projetoServiceMock = new Mock<IProjetoService>();
        _controller = new ProjetosController(_projetoServiceMock.Object);
    }

    [Fact]
    public async Task GetProjetos_DeveRetornarOk_ComListaDeProjetos()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projetos = new List<ProjetoDTO>
        {
            new() { Id = Guid.NewGuid(), Nome = "Projeto 1", UsuarioId = usuarioId },
            new() { Id = Guid.NewGuid(), Nome = "Projeto 2", UsuarioId = usuarioId }
        };
        
        _projetoServiceMock.Setup(s => s.ObterTodosDoUsuarioAsync(usuarioId))
            .ReturnsAsync(projetos);

        // Act
        var result = await _controller.GetProjetos(usuarioId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProjetos = Assert.IsAssignableFrom<IEnumerable<ProjetoDTO>>(okResult.Value);
        Assert.Equal(2, returnedProjetos.Count());
    }

    [Fact]
    public async Task GetProjeto_QuandoProjetoExiste_DeveRetornarOk_ComProjeto()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projeto = new ProjetoDTO { Id = projetoId, Nome = "Projeto Teste", UsuarioId = usuarioId };
        
        _projetoServiceMock.Setup(s => s.ObterPorIdAsync(projetoId))
            .ReturnsAsync(ResultadoOperacao<ProjetoDTO>.Sucedido(projeto));

        // Act
        var result = await _controller.GetProjeto(projetoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProjeto = Assert.IsType<ProjetoDTO>(okResult.Value);
        Assert.Equal(projetoId, returnedProjeto.Id);
    }

    [Fact]
    public async Task GetProjeto_QuandoProjetoNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        
        _projetoServiceMock.Setup(s => s.ObterPorIdAsync(projetoId))
            .ReturnsAsync(ResultadoOperacao<ProjetoDTO>.Falha("Projeto não encontrado."));

        // Act
        var result = await _controller.GetProjeto(projetoId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CriarProjeto_QuandoSucedido_DeveRetornarCreatedAtAction()
    {
        // Arrange
        var dto = new CriarProjetoDTO
        {
            Nome = "Novo Projeto",
            Descricao = "Descrição do novo projeto",
            UsuarioId = Guid.NewGuid()
        };
        
        var projetoCriado = new ProjetoDTO
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            UsuarioId = dto.UsuarioId
        };
        
        _projetoServiceMock.Setup(s => s.CriarAsync(dto))
            .ReturnsAsync(ResultadoOperacao<ProjetoDTO>.Sucedido(projetoCriado));

        // Act
        var result = await _controller.CriarProjeto(dto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedProjeto = Assert.IsType<ProjetoDTO>(createdAtActionResult.Value);
        Assert.Equal(projetoCriado.Id, returnedProjeto.Id);
        Assert.Equal("GetProjeto", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task CriarProjeto_QuandoFalhado_DeveRetornarBadRequest()
    {
        // Arrange
        var dto = new CriarProjetoDTO
        {
            Nome = "Novo Projeto",
            Descricao = "Descrição do novo projeto",
            UsuarioId = Guid.NewGuid()
        };
        
        _projetoServiceMock.Setup(s => s.CriarAsync(dto))
            .ReturnsAsync(ResultadoOperacao<ProjetoDTO>.Falha("Erro ao criar projeto."));

        // Act
        var result = await _controller.CriarProjeto(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task RemoverProjeto_QuandoSucedido_DeveRetornarNoContent()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        
        _projetoServiceMock.Setup(s => s.RemoverAsync(projetoId))
            .ReturnsAsync(ResultadoOperacao<bool>.Sucedido(true));

        // Act
        var result = await _controller.RemoverProjeto(projetoId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoverProjeto_QuandoFalhado_DeveRetornarBadRequest()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        
        _projetoServiceMock.Setup(s => s.RemoverAsync(projetoId))
            .ReturnsAsync(ResultadoOperacao<bool>.Falha("Não é possível remover o projeto porque ele possui tarefas pendentes."));

        // Act
        var result = await _controller.RemoverProjeto(projetoId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}