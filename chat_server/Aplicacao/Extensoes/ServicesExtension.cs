using Chat.API.Aplicacao.Comandos;
using Chat.API.Aplicacao.Consultas;
using Chat.API.Aplicacao.Validadores;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Chat.API.Aplicacao.Extensoes;

public static class ServicesExtension
{
    public static void AdicioneDependenciasAplicacao(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssemblyContaining<CadastroUsuarioRequisicaoValidador>();
        serviceCollection.AdicioneDependenciasConsultas();
        serviceCollection.AdicioneDependenciasComandos();
    }

    private static void AdicioneDependenciasConsultas(this IServiceCollection serviceCollection)
    {
        IEnumerable<Type> classeConsultas = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IConsulta).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type consulta in classeConsultas)
            serviceCollection.AddScoped(consulta);
    }

    private static void AdicioneDependenciasComandos(this IServiceCollection serviceCollection)
    {
        IEnumerable<Type> classeComandos = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IComando).IsAssignableFrom(t)
                && t.IsClass
                && !t.IsAbstract);

        foreach (Type comando in classeComandos)
            serviceCollection.AddScoped(comando);
    }
}
