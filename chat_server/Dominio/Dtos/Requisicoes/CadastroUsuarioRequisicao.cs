namespace Chat.API.Dominio.Dtos.Requisicoes;

public record class CadastroUsuarioRequisicao(string Login, string Senha, string Nome) : IRequisicao;
