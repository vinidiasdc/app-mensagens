using Chat.API.Dominio.Repositorios;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Chat.API.Infraestrutura.Extensoes;

public static class InfraestruturaExtension
{
    public static void AdicioneDependenciasInfraestrutura(this IServiceCollection serviceCollection)
    {
        IEnumerable<Type> classeRepositorios = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IRepositorio).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type repositorio in classeRepositorios)
        {
            // Mudar para scoped quando repositorios fazer consultas diretamente do banco
            serviceCollection.AddSingleton(repositorio);

            Type? interfaceEspecifica = repositorio.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IRepositorio) && typeof(IRepositorio).IsAssignableFrom(i));

            if (interfaceEspecifica is not null)
                serviceCollection.AddSingleton(interfaceEspecifica, repositorio);
        }
    }
}
