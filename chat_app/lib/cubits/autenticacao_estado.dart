abstract class AutenticacaoEstado {}

class AutenticacaoInicial extends AutenticacaoEstado {}

class AutenticacaoCarregando extends AutenticacaoEstado {}

class AutenticacaoSucesso extends AutenticacaoEstado {
  final String token;
  AutenticacaoSucesso(this.token);
}

class AutenticacaoErro extends AutenticacaoEstado {
  final String mensagem;
  AutenticacaoErro(this.mensagem);
}
