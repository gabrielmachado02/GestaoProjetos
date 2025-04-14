using AutoMapper;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Repositories;

namespace GestaoTarefas.Application.Services;

public class ProjetoService : IProjetoService
{
    private readonly IProjetoRepository _projetoRepository;
    private readonly IMapper _mapper;

    public ProjetoService(IProjetoRepository projetoRepository, IMapper mapper)
    {
        _projetoRepository = projetoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjetoDTO>> ObterTodosDoUsuarioAsync(Guid usuarioId)
    {
        var projetos = await _projetoRepository.ObterTodosDoUsuarioAsync(usuarioId);
        return _mapper.Map<IEnumerable<ProjetoDTO>>(projetos);
    }

    public async Task<ResultadoOperacao<ProjetoDTO>> ObterPorIdAsync(Guid id)
    {
        var projeto = await _projetoRepository.ObterPorIdAsync(id);
        
        if (projeto == null)
            return ResultadoOperacao<ProjetoDTO>.Falha("Projeto não encontrado.");
            
        return ResultadoOperacao<ProjetoDTO>.Sucedido(_mapper.Map<ProjetoDTO>(projeto));
    }

    public async Task<ResultadoOperacao<ProjetoDTO>> CriarAsync(CriarProjetoDTO dto)
    {
        var usuarioExiste = await _projetoRepository.UsuarioExisteAsync(dto.UsuarioId);
        if (!usuarioExiste)
            return ResultadoOperacao<ProjetoDTO>.Falha("Usuário não encontrado.");

        var projeto = _mapper.Map<Projeto>(dto);
        await _projetoRepository.AdicionarAsync(projeto);
        
        return ResultadoOperacao<ProjetoDTO>.Sucedido(
            _mapper.Map<ProjetoDTO>(projeto), 
            "Projeto criado com sucesso."
        );
    }

    public async Task<ResultadoOperacao<bool>> RemoverAsync(Guid id)
    {
        var projeto = await _projetoRepository.ObterPorIdAsync(id);
        
        if (projeto == null)
            return ResultadoOperacao<bool>.Falha("Projeto não encontrado.");
            
        if (!projeto.PodeSemRemovido())
            return ResultadoOperacao<bool>.Falha(
                "Não é possível remover o projeto porque ele possui tarefas pendentes. " +
                "Conclua ou remova todas as tarefas antes de remover o projeto."
            );
            
        await _projetoRepository.RemoverAsync(id);
        return ResultadoOperacao<bool>.Sucedido(true, "Projeto removido com sucesso.");
    }
}