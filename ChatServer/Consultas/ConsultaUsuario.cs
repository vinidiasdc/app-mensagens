using ChatServer.Models;
using ChatServer.Repositorios;
using ChatServer.Requisicoes;
using FluentValidation;

namespace ChatServer.Consultas;

public class ConsultaUsuario(IValidator<ConsultaUsuarioRequisicao> validator, RepositorioUsuarios repositorio) : ConsultaAbstrato<ConsultaUsuarioRequisicao, Usuario>(validator)
{
    protected override async Task<Usuario?> ExecuteAsync(ConsultaUsuarioRequisicao requisicao)
    {
        return repositorio.ObtenhaUsuario(requisicao.Login, requisicao.Senha);
    }
}
