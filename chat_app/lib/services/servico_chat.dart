import 'dart:async';
import 'package:chat_app/data/mensagem.dart';
import 'package:signalr_netcore/signalr_client.dart';


const _urlBase = String.fromEnvironment('URL_BASE', defaultValue: 'http://localhost:5000');

class ServicoChat {
  HubConnection? _hub;
  final _controlador = StreamController<Mensagem>.broadcast();

  Stream<Mensagem> get mensagens => _controlador.stream;

  Future<void> conectar(String token) async {
    _hub = HubConnectionBuilder()
        .withUrl(
          '$_urlBase/chat',
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
