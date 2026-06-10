using Chat.API.Aplicacao.Extensoes;
using Chat.API.Apresentacao.Endpoints;
using Chat.API.Apresentacao.Hubs;
using Chat.API.Apresentacao.Middlewares;
using Chat.API.Apresentacao.Servicos;
using Chat.API.Infraestrutura.Extensoes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

string jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Não foi possível encontrar a chave para validacao de tokens jwt");
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Não foi possível encontrar a issuer para validacao de tokens jwt");
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Não foi possível encontrar o audience para validacao de tokens jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                StringValues accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddSingleton<GeradorTokenJWT>();
builder.Services.AdicioneDependenciasAplicacao();
builder.Services.AdicioneDependenciasInfraestrutura(builder.Configuration);

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.AdicioneEndpointsAutenticacao();

app.MapHub<ChatHub>("/chat");

app.Run();
