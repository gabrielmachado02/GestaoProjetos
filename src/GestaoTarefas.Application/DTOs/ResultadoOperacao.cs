namespace GestaoTarefas.Application.DTOs;

public class ResultadoOperacao<T>
{
    public bool Sucesso { get; private set; }
    public string Mensagem { get; private set; } = string.Empty;
    public T? Dados { get; private set; }

    public static ResultadoOperacao<T> Falha(string mensagem)
    {
        return new ResultadoOperacao<T>
        {
            Sucesso = false,
            Mensagem = mensagem
        };
    }

    public static ResultadoOperacao<T> Sucedido(T dados, string mensagem = "")
    {
        return new ResultadoOperacao<T>
        {
            Sucesso = true,
            Dados = dados,
            Mensagem = mensagem
        };
    }
}