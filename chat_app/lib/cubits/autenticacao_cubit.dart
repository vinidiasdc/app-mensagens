import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import '../services/servico_autenticacao.dart';
import 'autenticacao_estado.dart';

class AutenticacaoCubit extends Cubit<AutenticacaoEstado> {
  final ServicoAutenticacao _servico;
  final FlutterSecureStorage _armazenamento;

  static const _chaveToken = 'jwt_token';

  AutenticacaoCubit({
    ServicoAutenticacao? servico,
    FlutterSecureStorage? armazenamento,
  })  : _servico = servico ?? ServicoAutenticacao(),
        _armazenamento = armazenamento ?? const FlutterSecureStorage(),
        super(AutenticacaoInicial());

  Future<void> verificarSessao() async {
    final token = await _armazenamento.read(key: _chaveToken);
    if (token != null && token.isNotEmpty) {
      emit(AutenticacaoSucesso(token));
    } else {
      emit(AutenticacaoInicial());
    }
  }

  Future<void> entrar(String login, String senha) async {
    emit(AutenticacaoCarregando());
    try {
      final resposta = await _servico.login(login, senha);
      await _armazenamento.write(key: _chaveToken, value: resposta.token);
      emit(AutenticacaoSucesso(resposta.token));
    } catch (e) {
      emit(AutenticacaoErro('Falha ao entrar. Verifique suas credenciais.'));
    }
  }

  Future<void> cadastrar(String login, String senha, String nome) async {
    emit(AutenticacaoCarregando());
    try {
      await _servico.cadastrar(login, senha, nome);
      emit(AutenticacaoInicial());
    } catch (e) {
      emit(AutenticacaoErro('Falha ao cadastrar. Tente novamente.'));
    }
  }

  Future<void> sair() async {
    await _armazenamento.delete(key: _chaveToken);
    emit(AutenticacaoInicial());
  }
}
