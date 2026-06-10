using System.Data;
using Chat.API.Dominio.Repositorios;
using Npgsql;

namespace Chat.API.Infraestrutura.Fabricas;

public class FabricaConexao(string connectionString) : IFabricaConexao
{
    public IDbConnection CrieConexaoBancoMensageria() => new NpgsqlConnection(connectionString);
}
