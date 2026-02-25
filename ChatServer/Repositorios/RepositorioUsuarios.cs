using ChatServer.Models;

namespace ChatServer.Repositorios;

public class RepositorioUsuarios : IRepositorioUsuarios
{
    private List<Usuario> Usuarios = []; // Por enquanto em memoria

    public Usuario? ObtenhaUsuario(string login, string senha)
    {
        // Posteriormente sera no banco postgres
        Usuario? usuario = Usuarios?.FirstOrDefault(u =>
            u.Nome == login && u.Senha == senha);

        return usuario;
    }

    public void CrieUsuario(string login, string nome, string senha)
    {
        Usuarios?.Add(new(login, nome, senha));
    }
}
