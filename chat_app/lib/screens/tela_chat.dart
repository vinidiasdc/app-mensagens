import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../cubits/autenticacao_cubit.dart';
import '../cubits/chat_cubit.dart';
import '../cubits/chat_estado.dart';
import '../data/mensagem.dart';
import 'tela_login.dart';

class TelaChat extends StatefulWidget {
  final String token;

  const TelaChat({super.key, required this.token});

  @override
  State<TelaChat> createState() => _TelaChatState();
}

class _TelaChatState extends State<TelaChat> {
  final _textoCtrl = TextEditingController();
  final _destinatarioCtrl = TextEditingController();
  final _scrollCtrl = ScrollController();
  late final ChatCubit _chatCubit;

  @override
  void initState() {
    super.initState();
    _chatCubit = ChatCubit();
    _chatCubit.conectar(widget.token);
  }

  @override
  void dispose() {
    _textoCtrl.dispose();
    _destinatarioCtrl.dispose();
    _scrollCtrl.dispose();
    _chatCubit.close();
    super.dispose();
  }

  void _enviar() {
    final destinatario = _destinatarioCtrl.text.trim();
    final texto = _textoCtrl.text.trim();
    if (destinatario.isEmpty || texto.isEmpty) return;
    _chatCubit.enviar(destinatario, texto);
    _textoCtrl.clear();
  }

  void _scrollParaBaixo() {
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (_scrollCtrl.hasClients) {
        _scrollCtrl.animateTo(
          _scrollCtrl.position.maxScrollExtent,
          duration: const Duration(milliseconds: 300),
          curve: Curves.easeOut,
        );
      }
    });
  }

  Future<void> _sair(BuildContext context) async {
    await _chatCubit.desconectar();
    if (!context.mounted) return;
    final autCubit = context.read<AutenticacaoCubit>();
    await autCubit.sair();
    if (!context.mounted) return;
    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(builder: (_) => const TelaLogin()),
      (_) => false,
    );
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider.value(
      value: _chatCubit,
      child: Scaffold(
        appBar: AppBar(
          title: const Text('Chat'),
          actions: [
            IconButton(
              icon: const Icon(Icons.logout),
              tooltip: 'Sair',
              onPressed: () => _sair(context),
            ),
          ],
        ),
        body: BlocConsumer<ChatCubit, ChatEstado>(
          bloc: _chatCubit,
          listener: (context, estado) {
            if (estado is ChatConectado) {
              _scrollParaBaixo();
            } else if (estado is ChatErro) {
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text(estado.mensagem)),
              );
            }
          },
          builder: (context, estado) {
            if (estado is ChatConectando) {
              return const Center(child: CircularProgressIndicator());
            }
            if (estado is ChatErro) {
              return Center(child: Text(estado.mensagem));
            }
            if (estado is ChatDesconectado) {
              return const Center(child: Text('Desconectado'));
            }

            final mensagens =
                estado is ChatConectado ? estado.mensagens : <Mensagem>[];

            return Column(
              children: [
                Padding(
                  padding: const EdgeInsets.fromLTRB(12, 8, 12, 0),
                  child: TextField(
                    controller: _destinatarioCtrl,
                    decoration: const InputDecoration(
                      labelText: 'Para (login do destinatário)',
                      border: OutlineInputBorder(),
                      isDense: true,
                      contentPadding:
                          EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                  ),
                ),
                const SizedBox(height: 4),
                Expanded(
                  child: mensagens.isEmpty
                      ? const Center(
                          child: Text('Nenhuma mensagem ainda. Diga olá!'))
                      : ListView.builder(
                          controller: _scrollCtrl,
                          padding: const EdgeInsets.all(12),
                          itemCount: mensagens.length,
                          itemBuilder: (_, i) =>
                              _BolhaMensagem(mensagem: mensagens[i]),
                        ),
                ),
                _CampoEnvio(
                  controller: _textoCtrl,
                  onEnviar: _enviar,
                ),
              ],
            );
          },
        ),
      ),
    );
  }
}

class _BolhaMensagem extends StatelessWidget {
  final Mensagem mensagem;

  const _BolhaMensagem({required this.mensagem});

  @override
  Widget build(BuildContext context) {
    final tema = Theme.of(context);
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            mensagem.usuario,
            style: tema.textTheme.labelSmall?.copyWith(
              fontWeight: FontWeight.bold,
              color: tema.colorScheme.primary,
            ),
          ),
          const SizedBox(height: 2),
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
            decoration: BoxDecoration(
              color: tema.colorScheme.surfaceContainerHighest,
              borderRadius: BorderRadius.circular(12),
            ),
            child: Text(mensagem.texto),
          ),
        ],
      ),
    );
  }
}

class _CampoEnvio extends StatelessWidget {
  final TextEditingController controller;
  final VoidCallback onEnviar;

  const _CampoEnvio({required this.controller, required this.onEnviar});

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Padding(
        padding: const EdgeInsets.all(8),
        child: Row(
          children: [
            Expanded(
              child: TextField(
                controller: controller,
                decoration: const InputDecoration(
                  hintText: 'Digite uma mensagem...',
                  border: OutlineInputBorder(),
                  contentPadding:
                      EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                ),
                onSubmitted: (_) => onEnviar(),
                textInputAction: TextInputAction.send,
              ),
            ),
            const SizedBox(width: 8),
            IconButton.filled(
              icon: const Icon(Icons.send),
              onPressed: onEnviar,
            ),
          ],
        ),
      ),
    );
  }
}
