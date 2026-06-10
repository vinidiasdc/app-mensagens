using Chat.API.Dominio.Entidades;
using Chat.API.Dominio.Excecoes;
using Chat.API.Dominio.Repositorios;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace Chat.API.Infraestrutura.Repositorios;

public class RepositorioUsuarios(IFabricaConexao fabricaConexao) : IRepositorioUsuarios
{
    public async Task<Usuario?> ObtenhaUsuario(string email, string senha)
    {
        using NpgsqlConnection conexao = (NpgsqlConnection) fabricaConexao.CrieConexaoBancoMensageria();
        await conexao.OpenAsync();

        using NpgsqlCommand cmd = new("select id, email, nome_usuario from usuarios where email = @email and senha_hash = @senha_hash", conexao);

        cmd.Parameters.AddWithValue("email", email);
        cmd.Parameters.AddWithValue("senha_hash", MonteHash(senha));

        using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Usuario
        {
            Id = reader.GetGuid(0),
            Login = reader.GetString(1),
            Nome = reader.GetString(2),
        };
    }

    public async Task CrieUsuario(string email, string nome, string senha)
    {
        using NpgsqlConnection conexao = (NpgsqlConnection) fabricaConexao.CrieConexaoBancoMensageria();

        await conexao.OpenAsync();

        using NpgsqlCommand cmd = new("insert into usuarios (nome_usuario, email, senha_hash) values (@nome, @email, @senha_hash) on conflict (email) do nothing", conexao);

        cmd.Parameters.AddWithValue("nome", nome);
        cmd.Parameters.AddWithValue("email", email);
        cmd.Parameters.AddWithValue("senha_hash", MonteHash(senha));

        int linhasAfetadas = await cmd.ExecuteNonQueryAsync();

        if (linhasAfetadas == 0)
        {
            throw new UsuarioJaCadastradoException();
        }
    }

    private static string MonteHash(string senha)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));

        string hash = Convert.ToHexString(bytes);

        return hash;
    }
}
