using System.Data;

namespace Chat.API.Dominio.Repositorios;

public interface IFabricaConexao
{
    IDbConnection CrieConexaoBancoMensageria();
}
