using AutoMapper;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Projeto, ProjetoDTO>();
        CreateMap<CriarProjetoDTO, Projeto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.Tarefas, opt => opt.Ignore());


        CreateMap<Tarefa, TarefaDTO>();
        CreateMap<CriarTarefaDTO, Tarefa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.Alteracoes, opt => opt.Ignore())
            .ForMember(dest => dest.Comentarios, opt => opt.Ignore());


        CreateMap<AlteracaoTarefa, AlteracaoTarefaDTO>();


        CreateMap<Comentario, ComentarioDTO>();
    }
}