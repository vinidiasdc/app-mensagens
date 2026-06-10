using Chat.API.Dominio.Repositorios;
using Chat.API.Infraestrutura.Fabricas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Chat.API.Infraestrutura.Extensoes;

public static class ExtensaoInfraestrutura
{
    public static void AdicioneDependenciasInfraestrutura(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("ConexaoBancoMensageria") ?? throw new InvalidOperationException("Connection string 'ConexaoBancoMensageria' não configurada.");

        serviceCollection.AddSingleton<IFabricaConexao>(new FabricaConexao(connectionString));

        IEnumerable<Type> classeRepositorios = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IRepositorio).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type repositorio in classeRepositorios)
        {
            Type? interfaceEspecifica = repositorio.GetInterfaces().FirstOrDefault(i => i != typeof(IRepositorio) && typeof(IRepositorio).IsAssignableFrom(i));

            if (interfaceEspecifica is not null)
            {
                serviceCollection.AddScoped(interfaceEspecifica, repositorio);
            }
            else
            {
                serviceCollection.AddScoped(repositorio);
            }
        }
    }
}
