using ChatServer.Requisicoes;
using ChatServer.Respostas;
using FluentValidation;

namespace ChatServer.Comandos;

public abstract class ComandoComRetornoAbstrato<TRequisicao, TResposta>(IValidator<TRequisicao> validator) : IComando
    where TRequisicao : IRequisicao
    where TResposta : IResposta
{
    private readonly IValidator<TRequisicao> _validator = validator;

    public async Task<TResposta> Execute(TRequisicao requisicao)
    {
        await _validator.ValidateAndThrowAsync(requisicao);
        return await ExecuteAsync(requisicao);
    }

    protected abstract Task<TResposta> ExecuteAsync(TRequisicao requisicao);
}
