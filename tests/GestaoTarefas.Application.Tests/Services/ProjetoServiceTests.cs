using AutoMapper;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Application.Mappings;
using GestaoTarefas.Application.Services;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Repositories;
using Moq;
using Xunit;

namespace GestaoTarefas.Application.Tests.Services;

public class ProjetoServiceTests
{
    private readonly Mock<IProjetoRepository> _projetoRepositoryMock;
    private readonly IMapper _mapper;
    private readonly IProjetoService _projetoService;

    public ProjetoServiceTests()
    {
        _projetoRepositoryMock = new Mock<IProjetoRepository>();
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        _mapper = mapperConfig.CreateMapper();
        
        _projetoService = new ProjetoService(_projetoRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task ObterTodosDoUsuarioAsync_DeveRetornarListaDeProjetos()
    {

        var usuarioId = Guid.NewGuid();
        var projetos = new List<Projeto>
        {
            new Projeto("Projeto 1", "Descrição 1", usuarioId),
            new Projeto("Projeto 2", "Descrição 2", usuarioId)
        };
        
        _projetoRepositoryMock.Setup(r => r.ObterTodosDoUsuarioAsync(usuarioId))
            .ReturnsAsync(projetos);


        var result = await _projetoService.ObterTodosDoUsuarioAsync(usuarioId);


        Assert.Equal(2, result.Count());
        _projetoRepositoryMock.Verify(r => r.ObterTodosDoUsuarioAsync(usuarioId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoProjetoExiste_DeveRetornarProjeto()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId) { };
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync(projeto);


        var result = await _projetoService.ObterPorIdAsync(projetoId);


        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        Assert.Equal(projeto.Nome, result.Dados.Nome);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoProjetoNaoExiste_DeveRetornarFalha()
    {

        var projetoId = Guid.NewGuid();
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync((Projeto)null);


        var result = await _projetoService.ObterPorIdAsync(projetoId);


        Assert.False(result.Sucesso);
        Assert.Null(result.Dados);
        Assert.Equal("Projeto não encontrado.", result.Mensagem);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_QuandoUsuarioExiste_DeveCriarProjeto()
    {

        var usuarioId = Guid.NewGuid();
        var dto = new CriarProjetoDTO
        {
            Nome = "Novo Projeto",
            Descricao = "Descrição do novo projeto",
            UsuarioId = usuarioId
        };
        
        _projetoRepositoryMock.Setup(r => r.UsuarioExisteAsync(usuarioId))
            .ReturnsAsync(true);
        
        _projetoRepositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<Projeto>()))
            .ReturnsAsync((Projeto p) => p);


        var result = await _projetoService.CriarAsync(dto);


        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        Assert.Equal(dto.Nome, result.Dados.Nome);
        Assert.Equal(dto.Descricao, result.Dados.Descricao);
        Assert.Equal(dto.UsuarioId, result.Dados.UsuarioId);
        _projetoRepositoryMock.Verify(r => r.UsuarioExisteAsync(usuarioId), Times.Once);
        _projetoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Projeto>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_QuandoUsuarioNaoExiste_DeveRetornarFalha()
    {

        var usuarioId = Guid.NewGuid();
        var dto = new CriarProjetoDTO
        {
            Nome = "Novo Projeto",
            Descricao = "Descrição do novo projeto",
            UsuarioId = usuarioId
        };
        
        _projetoRepositoryMock.Setup(r => r.UsuarioExisteAsync(usuarioId))
            .ReturnsAsync(false);


        var result = await _projetoService.CriarAsync(dto);


        Assert.False(result.Sucesso);
        Assert.Null(result.Dados);
        Assert.Equal("Usuário não encontrado.", result.Mensagem);
        _projetoRepositoryMock.Verify(r => r.UsuarioExisteAsync(usuarioId), Times.Once);
        _projetoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Projeto>()), Times.Never);
    }

    [Fact]
    public async Task RemoverAsync_QuandoProjetoExisteESemTarefasPendentes_DeveRemoverProjeto()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId);
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync(projeto);


        var result = await _projetoService.RemoverAsync(projetoId);


        Assert.True(result.Sucesso);
        Assert.True(result.Dados);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
        _projetoRepositoryMock.Verify(r => r.RemoverAsync(projetoId), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_QuandoProjetoNaoExiste_DeveRetornarFalha()
    {

        var projetoId = Guid.NewGuid();
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync((Projeto)null);


        var result = await _projetoService.RemoverAsync(projetoId);


        Assert.False(result.Sucesso);
        Assert.False(result.Dados);
        Assert.Equal("Projeto não encontrado.", result.Mensagem);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
        _projetoRepositoryMock.Verify(r => r.RemoverAsync(projetoId), Times.Never);
    }
}