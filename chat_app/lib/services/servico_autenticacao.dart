import 'package:dio/dio.dart';
import '../ambiente_urls.dart';
import '../data/resposta_token.dart';

class ServicoAutenticacao {
  final Dio _dio = Dio(BaseOptions(baseUrl: urlChatApi));

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
