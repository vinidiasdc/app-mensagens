using ChatServer.Comandos;
using ChatServer.Consultas;
using ChatServer.Repositorios;
using System.Reflection;

namespace ChatServer.Extensoes;

public static class ServicesExtension
{
    public static void AdicioneDependenciasConsultas(this IServiceCollection serviceCollection)
    {
        IEnumerable<Type> classeConsultas = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IConsulta).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type consulta in classeConsultas)
        {
            serviceCollection.AddScoped(consulta);
        }
    }

    public static void AdicioneDependenciasComandos(this IServiceCollection serviceCollection)
    {
        IEnumerable<Type> classeComandos = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IComando).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type consulta in classeComandos)
        {
            serviceCollection.AddScoped(consulta);
        }
    }

    public static void AdicioneDependenciasRepositorios(this IServiceCollection serviceCollection)
    {
        IEnumerable<Type> classeRepositorios = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IRepositorio).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type repositorio in classeRepositorios)
        {
            //Mudar para scoped quando repositorios fazer consultas diretamente do banco
            serviceCollection.AddSingleton(repositorio);

            Type? interfaceEspecifica = repositorio.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IRepositorio) && typeof(IRepositorio).IsAssignableFrom(i));

            if (interfaceEspecifica is not null)
                serviceCollection.AddSingleton(interfaceEspecifica, repositorio);
        }
    }
}
