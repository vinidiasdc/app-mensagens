using Chat.API.Dominio.Entidades;
using Chat.API.Dominio.Repositorios;

namespace Chat.API.Infraestrutura.Repositorios;

public class RepositorioUsuarios : IRepositorioUsuarios
{
    private List<Usuario> Usuarios = []; // Por enquanto em memoria

    public Usuario? ObtenhaUsuario(string login, string senha)
    {
        // Posteriormente sera no banco postgres
        return Usuarios?.FirstOrDefault(u => u.Nome == login && u.Senha == senha);
    }

    public void CrieUsuario(string login, string nome, string senha)
    {
        Usuarios?.Add(new(login, nome, senha));
    }
}
