namespace Chat.API.Dominio.Entidades;

public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } =  string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? TokenJwt { get; set; }
};
