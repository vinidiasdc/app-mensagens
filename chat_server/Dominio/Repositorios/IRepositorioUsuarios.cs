using Chat.API.Dominio.Entidades;

namespace Chat.API.Dominio.Repositorios;

public interface IRepositorioUsuarios : IRepositorio
{
    Usuario? ObtenhaUsuario(string login, string senha);

    void CrieUsuario(string login, string nome, string senha);
}
