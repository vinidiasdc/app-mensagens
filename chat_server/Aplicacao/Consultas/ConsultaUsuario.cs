using Chat.API.Aplicacao.Consultas;
using Chat.API.Dominio.Dtos.Requisicoes;
using Chat.API.Dominio.Entidades;
using Chat.API.Dominio.Repositorios;
using FluentValidation;

namespace Chat.API.Consultas;

public class ConsultaUsuario(IValidator<ConsultaUsuarioRequisicao> validator, IRepositorioUsuarios repositorio) : ConsultaAbstrato<ConsultaUsuarioRequisicao, Usuario>(validator)
{
    protected override async Task<Usuario?> ExecuteAsync(ConsultaUsuarioRequisicao requisicao)
    {
        return repositorio.ObtenhaUsuario(requisicao.Login, requisicao.Senha);
    }
}
