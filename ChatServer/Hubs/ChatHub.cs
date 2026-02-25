using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(string mensagem)
    {
        string usuario = Context.User?.Identity?.Name ?? throw new ApplicationException("Sem usuário");
        await Clients.All.SendAsync("ReceberMensagem", usuario, mensagem);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceberMensagem", "Server", "Bem vindo ao SignalR Chat!");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("ReceberMensagem", "Server", "Um usuário se desconectou.");
    }
}
