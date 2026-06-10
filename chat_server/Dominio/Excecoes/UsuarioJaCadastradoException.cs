namespace Chat.API.Dominio.Excecoes;

public class UsuarioJaCadastradoException() : AplicacaoException("Já existe um usuário cadastrado com esse e-mail.");
