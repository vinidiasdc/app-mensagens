import 'dart:async';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../data/mensagem.dart';
import '../services/servico_chat.dart';
import 'chat_estado.dart';

class ChatCubit extends Cubit<ChatEstado> {
  final ServicoChat _servico;
  StreamSubscription<Mensagem>? _mensagens;

  ChatCubit({ServicoChat? servico})
      : _servico = servico ?? ServicoChat(),
        super(ChatDesconectado());

  Future<void> conectar(String token) async {
    emit(ChatConectando());
    try {
      await _servico.conectar(token);
      emit(ChatConectado([]));

      _mensagens = _servico.mensagens.listen((mensagem) {
        final estadoAtual = state;
        if (estadoAtual is ChatConectado) {
          emit(ChatConectado([...estadoAtual.mensagens, mensagem]));
        }
      });
    } catch (e) {
      emit(ChatErro('Falha ao conectar ao chat.'));
    }
  }

  Future<void> enviar(String destinatario, String texto) async {
    try {
      await _servico.enviarMensagem(destinatario, texto);
    } catch (e) {
      emit(ChatErro('Falha ao enviar mensagem.'));
    }
  }

  Future<void> desconectar() async {
    await _mensagens?.cancel();
    _mensagens = null;
    await _servico.desconectar();
    emit(ChatDesconectado());
  }

  @override
  Future<void> close() async {
    await _mensagens?.cancel();
    _servico.dispose();
    return super.close();
  }
}
