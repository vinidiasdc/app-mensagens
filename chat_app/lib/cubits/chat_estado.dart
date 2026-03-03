import '../data/mensagem.dart';

abstract class ChatEstado {}

class ChatDesconectado extends ChatEstado {}

class ChatConectando extends ChatEstado {}

class ChatConectado extends ChatEstado {
  final List<Mensagem> mensagens;
  ChatConectado(this.mensagens);
}

class ChatErro extends ChatEstado {
  final String mensagem;
  ChatErro(this.mensagem);
}
