using ChatServer.Models;

namespace ChatServer.Repositorios;

public interface IRepositorioUsuarios : IRepositorio
{
    Usuario? ObtenhaUsuario(string login, string senha);

    void CrieUsuario(string login, string nome, string senha);
}
