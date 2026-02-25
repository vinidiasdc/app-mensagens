namespace ChatServer.Requisicoes;

public record class ConsultaUsuarioRequisicao(string Login, string Senha) : IRequisicao;
