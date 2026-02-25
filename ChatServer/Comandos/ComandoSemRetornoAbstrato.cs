using ChatServer.Requisicoes;
using FluentValidation;

namespace ChatServer.Comandos;

public abstract class ComandoSemRetornoAbstrato<TRequisicao>(IValidator<TRequisicao> validator) : IComando
    where TRequisicao : IRequisicao
{
    private readonly IValidator<TRequisicao> _validator = validator;

    public async Task Execute(TRequisicao requisicao)
    {
        await _validator.ValidateAndThrowAsync(requisicao);
        await ExecuteAsync(requisicao);
    }

    protected abstract Task ExecuteAsync(TRequisicao requisicao);
}
