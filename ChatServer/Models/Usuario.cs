namespace ChatServer.Models;

public class Usuario(string nome, string login, string senha)
{
    public string Nome { get; set; } = nome;
    public string Login { get; set; } = login;
    public string Senha { get; set; } = senha;
    public string? TokenJwt { get; set; }
};
