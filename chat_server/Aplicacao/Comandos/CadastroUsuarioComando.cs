using Chat.API.Aplicacao.Comandos;
using Chat.API.Dominio.Dtos.Requisicoes;
using Chat.API.Dominio.Repositorios;
using FluentValidation;

namespace Chat.API.Comandos;

public class CadastroUsuarioComando(IValidator<CadastroUsuarioRequisicao> validator, IRepositorioUsuarios repositorio) : ComandoSemRetornoAbstrato<CadastroUsuarioRequisicao>(validator)
{
    protected override async Task ExecuteAsync(CadastroUsuarioRequisicao requisicao)
    {
        repositorio.CrieUsuario(requisicao.Login, requisicao.Nome, requisicao.Senha);
    }
}
