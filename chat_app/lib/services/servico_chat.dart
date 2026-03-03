import 'dart:async';
import 'package:signalr_netcore/signalr_client.dart';
import '../ambiente_urls.dart';
import '../data/mensagem.dart';

class ServicoChat {
  HubConnection? _hub;
  final _controlador = StreamController<Mensagem>.broadcast();

  Stream<Mensagem> get mensagens => _controlador.stream;

  Future<void> conectar(String token) async {
    _hub = HubConnectionBuilder()
        .withUrl(
          '$urlChatApi/chat',
          options: HttpConnectionOptions(
            accessTokenFactory: () async => token,
          ),
        )
        .build();

    _hub!.on('ReceberMensagem', (argumentos) {
      if (argumentos != null) {
        final usuario = argumentos[0]?.toString() ?? '';
        final texto = argumentos[1]?.toString() ?? '';
        _controlador.add(Mensagem(usuario: usuario, texto: texto));
      }
    });

    await _hub!.start();
  }

  Future<void> enviarMensagem(String destinatarioId, String texto) async {
    await _hub?.invoke('SendMessage', args: [
      {'destinatarioId': destinatarioId, 'texto': texto}
    ]);
  }

  Future<void> desconectar() async {
    await _hub?.stop();
    _hub = null;
  }

  void dispose() {
    _controlador.close();
  }
}
