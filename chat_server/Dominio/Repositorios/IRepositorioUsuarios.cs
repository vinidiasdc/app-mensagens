using Chat.API.Dominio.Entidades;

namespace Chat.API.Dominio.Repositorios;

public interface IRepositorioUsuarios : IRepositorio
{
    Task<Usuario?> ObtenhaUsuario(string email, string senha);

    Task CrieUsuario(string email, string nome, string senha);
}
