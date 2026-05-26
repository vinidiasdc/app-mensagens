namespace Chat.API.Dominio.Dtos.Requisicoes;

public record class ConsultaUsuarioRequisicao(string Login, string Senha) : IRequisicao;
