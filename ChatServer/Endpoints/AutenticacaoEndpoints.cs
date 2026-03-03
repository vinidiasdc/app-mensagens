using ChatServer.Comandos;
using ChatServer.Consultas;
using ChatServer.Models;
using ChatServer.Requisicoes;
using ChatServer.Respostas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatServer.Endpoints;

public static class AutenticacaoEndpoints
{
    public static void AdicioneEndpointsAutenticacao(this WebApplication app)
    {
        app.MapPost("/autenticacao/login", async (ConsultaUsuarioRequisicao requisicao, [FromServices] ConsultaUsuario consultaUsuario, IConfiguration configuration) =>
        {
            Usuario? usuario = await consultaUsuario.Execute(requisicao);

            if (usuario is null)
                return Results.Unauthorized();

            DateTime expiracao = DateTime.UtcNow.AddDays(20);

            string jwtKey = configuration.GetValue<string>("Jwt:Key")!;
            string jwtIssuer = configuration.GetValue<string>("Jwt:Issuer")!;
            string jwtAudience = configuration.GetValue<string>("Jwt:Audience")!;

            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Login)
            ];

            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(jwtKey));
            SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expiracao,
                signingCredentials: credentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            usuario.TokenJwt = tokenString;

            return Results.Ok(new TokenResposta(tokenString, expiracao));
        });

        app.MapPost("/autenticacao/cadastro", async (CadastroUsuarioRequisicao requisicao, [FromServices] CadastroUsuarioComando comando) =>
        {
            await comando.Execute(requisicao);

            return Results.Ok();
        });
    }
}
