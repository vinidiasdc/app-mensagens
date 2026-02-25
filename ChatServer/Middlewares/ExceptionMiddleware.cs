using FluentValidation;
using System.Net;
using System.Text.Json;

namespace ChatServer.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await EscrevaResposta(context, HttpStatusCode.BadRequest,
                ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado");
            await EscrevaResposta(context, HttpStatusCode.InternalServerError,
                ["Ocorreu um erro interno no servidor"]);
        }
    }

    private static async Task EscrevaResposta(HttpContext context, HttpStatusCode status, IEnumerable<string> erros)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        string json = JsonSerializer.Serialize(new { erros });
        await context.Response.WriteAsync(json);
    }
}
