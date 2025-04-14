using AutoMapper;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Projeto
        CreateMap<Projeto, ProjetoDTO>();
        CreateMap<CriarProjetoDTO, Projeto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.Tarefas, opt => opt.Ignore());

        // Tarefa
        CreateMap<Tarefa, TarefaDTO>();
        CreateMap<CriarTarefaDTO, Tarefa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.Alteracoes, opt => opt.Ignore())
            .ForMember(dest => dest.Comentarios, opt => opt.Ignore());

        // AlteracaoTarefa
        CreateMap<AlteracaoTarefa, AlteracaoTarefaDTO>();

        // Comentario
        CreateMap<Comentario, ComentarioDTO>();
    }
}