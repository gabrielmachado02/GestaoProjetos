using AutoMapper;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Repositories;

namespace GestaoTarefas.Application.Services;

public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IProjetoRepository _projetoRepository;
    private readonly IMapper _mapper;

    public TarefaService(
        ITarefaRepository tarefaRepository,
        IProjetoRepository projetoRepository,
        IMapper mapper)
    {
        _tarefaRepository = tarefaRepository;
        _projetoRepository = projetoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TarefaDTO>> ObterTodasDoProjetoAsync(Guid projetoId)
    {
        var tarefas = await _tarefaRepository.ObterTodasDoProjeto(projetoId);
        return _mapper.Map<IEnumerable<TarefaDTO>>(tarefas);
    }

    public async Task<ResultadoOperacao<TarefaDTO>> ObterPorIdAsync(Guid id)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(id);
        
        if (tarefa == null)
            return ResultadoOperacao<TarefaDTO>.Falha("Tarefa não encontrada.");
            
        return ResultadoOperacao<TarefaDTO>.Sucedido(_mapper.Map<TarefaDTO>(tarefa));
    }

    public async Task<ResultadoOperacao<TarefaDTO>> CriarAsync(CriarTarefaDTO dto)
    {
        var projeto = await _projetoRepository.ObterPorIdAsync(dto.ProjetoId);
        
        if (projeto == null)
            return ResultadoOperacao<TarefaDTO>.Falha("Projeto não encontrado.");
            
        var quantidadeTarefas = await _tarefaRepository.ContarTarefasDoProjetoAsync(dto.ProjetoId);
        if (quantidadeTarefas >= 20)
            return ResultadoOperacao<TarefaDTO>.Falha("O projeto já atingiu o limite máximo de 20 tarefas.");
            
        var tarefa = _mapper.Map<Tarefa>(dto);
        projeto.AdicionarTarefa(tarefa);
        
        await _tarefaRepository.AdicionarAsync(tarefa);
        
        return ResultadoOperacao<TarefaDTO>.Sucedido(
            _mapper.Map<TarefaDTO>(tarefa), 
            "Tarefa criada com sucesso."
        );
    }

    public async Task<ResultadoOperacao<TarefaDTO>> AtualizarAsync(AtualizarTarefaDTO dto)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(dto.Id);
        
        if (tarefa == null)
            return ResultadoOperacao<TarefaDTO>.Falha("Tarefa não encontrada.");
            
        tarefa.AtualizarStatus(dto.Status, dto.UsuarioId);
        tarefa.AtualizarDetalhes(dto.Titulo, dto.Descricao, dto.DataVencimento, dto.UsuarioId);
        
        await _tarefaRepository.AtualizarAsync(tarefa);
        
        return ResultadoOperacao<TarefaDTO>.Sucedido(
            _mapper.Map<TarefaDTO>(tarefa), 
            "Tarefa atualizada com sucesso."
        );
    }

    public async Task<ResultadoOperacao<bool>> RemoverAsync(Guid id)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(id);
        
        if (tarefa == null)
            return ResultadoOperacao<bool>.Falha("Tarefa não encontrada.");
            
        await _tarefaRepository.RemoverAsync(id);
        
        return ResultadoOperacao<bool>.Sucedido(true, "Tarefa removida com sucesso.");
    }

    public async Task<ResultadoOperacao<TarefaDTO>> AdicionarComentarioAsync(AdicionarComentarioDTO dto)
    {
        var tarefa = await _tarefaRepository.ObterPorIdAsync(dto.TarefaId);
        
        if (tarefa == null)
            return ResultadoOperacao<TarefaDTO>.Falha("Tarefa não encontrada.");
            
        tarefa.AdicionarComentario(dto.Conteudo, dto.UsuarioId);
        
        await _tarefaRepository.AtualizarAsync(tarefa);
        
        return ResultadoOperacao<TarefaDTO>.Sucedido(
            _mapper.Map<TarefaDTO>(tarefa), 
            "Comentário adicionado com sucesso."
        );
    }
}