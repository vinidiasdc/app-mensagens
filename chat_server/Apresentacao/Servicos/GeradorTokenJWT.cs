using Chat.API.Dominio.Dtos.Respostas;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.API.Apresentacao.Servicos;

public class GeradorTokenJWT(IConfiguration configuration)
{
    public TokenResposta Gere(Guid id, string login)
    {
        DateTime expiracao = DateTime.UtcNow.AddDays(20);

        string jwtKey = configuration.GetValue<string>("Jwt:Key")!;
        string jwtIssuer = configuration.GetValue<string>("Jwt:Issuer")!;
        string jwtAudience = configuration.GetValue<string>("Jwt:Audience")!;

        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, login)
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

        return new TokenResposta(tokenString, expiracao);
    }
}
