using ChatServer.Requisicoes;
using ChatServer.Respostas;
using FluentValidation;

namespace ChatServer.Consultas;

public abstract class ConsultaAbstrato<TRequisicao, TResposta>(IValidator<TRequisicao> validator) : IConsulta
    where TRequisicao : IRequisicao
    where TResposta : class
{
    private readonly IValidator<TRequisicao> _validator = validator;

    public async Task<TResposta?> Execute(TRequisicao requisicao)
    {
        await _validator.ValidateAndThrowAsync(requisicao);
        return await ExecuteAsync(requisicao);
    }

    protected abstract Task<TResposta?> ExecuteAsync(TRequisicao requisicao);
}
