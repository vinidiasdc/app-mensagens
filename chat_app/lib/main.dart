import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'cubits/autenticacao_cubit.dart';
import 'cubits/autenticacao_estado.dart';
import 'screens/tela_chat.dart';
import 'screens/tela_login.dart';

void main() {
  runApp(const ChatApp());
}

class ChatApp extends StatelessWidget {
  const ChatApp({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => AutenticacaoCubit()..verificarSessao(),
      child: MaterialApp(
        title: 'Chat',
        debugShowCheckedModeBanner: false,
        theme: ThemeData(
          colorScheme: ColorScheme.fromSeed(seedColor: Colors.indigo),
          useMaterial3: true,
        ),
        home: const _RaizApp(),
      ),
    );
  }
}

class _RaizApp extends StatelessWidget {
  const _RaizApp();

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<AutenticacaoCubit, AutenticacaoEstado>(
      builder: (context, estado) {
        if (estado is AutenticacaoSucesso) {
          return TelaChat(token: estado.token);
        }
        return const TelaLogin();
      },
    );
  }
}
