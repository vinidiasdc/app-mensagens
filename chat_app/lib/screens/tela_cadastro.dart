import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../cubits/autenticacao_cubit.dart';
import '../cubits/autenticacao_estado.dart';

class TelaCadastro extends StatefulWidget {
  const TelaCadastro({super.key});

  @override
  State<TelaCadastro> createState() => _TelaCadastroState();
}

class _TelaCadastroState extends State<TelaCadastro> {
  final _formKey = GlobalKey<FormState>();
  final _loginCtrl = TextEditingController();
  final _senhaCtrl = TextEditingController();
  final _nomeCtrl = TextEditingController();

  @override
  void dispose() {
    _loginCtrl.dispose();
    _senhaCtrl.dispose();
    _nomeCtrl.dispose();
    super.dispose();
  }

  void _cadastrar(BuildContext context) {
    if (_formKey.currentState!.validate()) {
      context.read<AutenticacaoCubit>().cadastrar(
            _loginCtrl.text.trim(),
            _senhaCtrl.text,
            _nomeCtrl.text.trim(),
          );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Chat — Cadastro')),
      body: BlocConsumer<AutenticacaoCubit, AutenticacaoEstado>(
        listener: (context, estado) {
          if (estado is AutenticacaoInicial) {
            ScaffoldMessenger.of(context).showSnackBar(
              const SnackBar(content: Text('Cadastro realizado! Faça login.')),
            );
            Navigator.of(context).pop();
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
                    controller: _nomeCtrl,
                    decoration: const InputDecoration(labelText: 'Nome'),
                    validator: (v) =>
                        v == null || v.isEmpty ? 'Informe o nome' : null,
                  ),
                  const SizedBox(height: 16),
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
                      onPressed: carregando ? null : () => _cadastrar(context),
                      child: carregando
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : const Text('Cadastrar'),
                    ),
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
