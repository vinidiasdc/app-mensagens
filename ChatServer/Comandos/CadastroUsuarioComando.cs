using ChatServer.Repositorios;
using ChatServer.Requisicoes;
using FluentValidation;

namespace ChatServer.Comandos;

public class CadastroUsuarioComando(IValidator<CadastroUsuarioRequisicao> validator, RepositorioUsuarios repositorio) : ComandoSemRetornoAbstrato<CadastroUsuarioRequisicao>(validator)
{
    protected override async Task ExecuteAsync(CadastroUsuarioRequisicao requisicao)
    {
        repositorio.CrieUsuario(requisicao.Login, requisicao.Nome, requisicao.Senha);
    }
}
