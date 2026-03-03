class RespostaToken {
  final String token;
  final DateTime expiracao;

  const RespostaToken({required this.token, required this.expiracao});

  factory RespostaToken.fromJson(Map<String, dynamic> json) {
    return RespostaToken(
      token: json['token'] as String,
      expiracao: DateTime.parse(json['expiracao'] as String),
    );
  }
}
