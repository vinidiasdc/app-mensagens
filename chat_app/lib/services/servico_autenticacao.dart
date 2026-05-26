import 'package:chat_app/data/resposta_token.dart';
import 'package:dio/dio.dart';

const _urlBase = String.fromEnvironment('URL_BASE', defaultValue: 'http://localhost:5000');

class ServicoAutenticacao {
  final Dio _dio = Dio(BaseOptions(baseUrl: _urlBase));

  Future<RespostaToken> login(String login, String senha) async {
    final resposta = await _dio.post(
      '/autenticacao/login',
      data: {'login': login, 'senha': senha},
    );
    return RespostaToken.fromJson(resposta.data as Map<String, dynamic>);
  }

  Future<void> cadastrar(String login, String senha, String nome) async {
    await _dio.post(
      '/autenticacao/cadastro',
      data: {'login': login, 'senha': senha, 'nome': nome},
    );
  }
}
