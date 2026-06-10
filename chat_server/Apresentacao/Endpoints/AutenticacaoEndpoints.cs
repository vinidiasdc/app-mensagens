using Chat.API.Comandos;
using Chat.API.Consultas;
using Chat.API.Apresentacao.Servicos;
using Chat.API.Dominio.Dtos.Requisicoes;
using Chat.API.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Apresentacao.Endpoints;

public static class AutenticacaoEndpoints
{
    public static void AdicioneEndpointsAutenticacao(this WebApplication app)
    {
        app.MapPost("/autenticacao/login", async (ConsultaUsuarioRequisicao requisicao, [FromServices] ConsultaUsuario consultaUsuario, [FromServices] GeradorTokenJWT geradorToken) =>
        {
            Usuario? usuario = await consultaUsuario.Execute(requisicao);

            if (usuario is null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(geradorToken.Gere(usuario.Id, usuario.Login));
        });

        app.MapPost("/autenticacao/cadastro", async (CadastroUsuarioRequisicao requisicao, [FromServices] CadastroUsuarioComando comando) =>
        {
            await comando.Execute(requisicao);

            return Results.Ok();
        });
    }
}
