using ChatServer.Endpoints;
using ChatServer.Extensoes;
using ChatServer.Hubs;
using ChatServer.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

string jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Não foi posssível encontrar a chave para validacao de tokens jwt");
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Não foi posssível encontrar a issuer para validacao de tokens jwt");
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Não foi posssível encontrar o audience para validacao de tokens jwt");

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
                PathString path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken))
                    context.Token = accessToken;

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AdicioneDependenciasRepositorios();
builder.Services.AdicioneDependenciasConsultas();
builder.Services.AdicioneDependenciasComandos();

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.AdicioneEndpointsAutenticacao();

app.MapHub<ChatHub>("/chat");

app.Run();
