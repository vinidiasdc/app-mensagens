using Chat.API.Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Apresentacao.Hubs;

[Authorize]
public class ChatHub(ILogger<ChatHub> logger) : Hub
{
    public async Task SendMessage(Mensagem mensagem)
    {
        string remetenteId = Context.UserIdentifier!;

        await Clients.Users(mensagem.DestinatarioId, remetenteId)
                     .SendAsync("ReceberMensagem", remetenteId, mensagem.Texto);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        logger.LogInformation("Usuário conectado: {Login} | ID: {Id}", Context.User!.Identity!.Name, Context.UserIdentifier);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
