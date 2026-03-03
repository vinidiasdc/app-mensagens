import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../cubits/autenticacao_cubit.dart';
import '../cubits/autenticacao_estado.dart';
import 'tela_cadastro.dart';
import 'tela_chat.dart';

class TelaLogin extends StatefulWidget {
  const TelaLogin({super.key});

  @override
  State<TelaLogin> createState() => _TelaLoginState();
}

class _TelaLoginState extends State<TelaLogin> {
  final _formKey = GlobalKey<FormState>();
  final _loginCtrl = TextEditingController();
  final _senhaCtrl = TextEditingController();

  @override
  void dispose() {
    _loginCtrl.dispose();
    _senhaCtrl.dispose();
    super.dispose();
  }

  void _entrar(BuildContext context) {
    if (_formKey.currentState!.validate()) {
      context.read<AutenticacaoCubit>().entrar(
            _loginCtrl.text.trim(),
            _senhaCtrl.text,
          );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Chat — Login')),
      body: BlocConsumer<AutenticacaoCubit, AutenticacaoEstado>(
        listener: (context, estado) {
          if (estado is AutenticacaoSucesso) {
            Navigator.of(context).pushReplacement(
              MaterialPageRoute(
                builder: (_) => TelaChat(token: estado.token),
              ),
            );
          } else if (estado is AutenticacaoErro) {
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(content: Text(estado.mensagem)),
            );
          }
        },
        builder: (context, estado) {
          final carregando = estado is AutenticacaoCarregando;
          return Padding(
            padding: const EdgeInsets.all(24),
            child: Form(
              key: _formKey,
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  TextFormField(
                    controller: _loginCtrl,
                    decoration: const InputDecoration(labelText: 'Login'),
                    validator: (v) =>
                        v == null || v.isEmpty ? 'Informe o login' : null,
                  ),
                  const SizedBox(height: 16),
                  TextFormField(
                    controller: _senhaCtrl,
                    decoration: const InputDecoration(labelText: 'Senha'),
                    obscureText: true,
                    validator: (v) =>
                        v == null || v.isEmpty ? 'Informe a senha' : null,
                  ),
                  const SizedBox(height: 32),
                  SizedBox(
                    width: double.infinity,
                    child: FilledButton(
                      onPressed: carregando ? null : () => _entrar(context),
                      child: carregando
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : const Text('Entrar'),
                    ),
                  ),
                  TextButton(
                    onPressed: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(
                          builder: (_) => const TelaCadastro(),
                        ),
                      );
                    },
                    child: const Text('Não tem conta? Cadastre-se'),
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}
