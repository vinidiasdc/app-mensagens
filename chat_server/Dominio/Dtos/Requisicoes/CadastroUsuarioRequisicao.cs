namespace Chat.API.Dominio.Dtos.Requisicoes;

public record class CadastroUsuarioRequisicao(string Email, string Senha, string Nome) : IRequisicao;
