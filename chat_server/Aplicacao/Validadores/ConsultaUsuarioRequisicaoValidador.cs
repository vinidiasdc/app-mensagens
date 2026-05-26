using Chat.API.Dominio.Dtos.Requisicoes;
using FluentValidation;

namespace Chat.API.Aplicacao.Validadores;

public class ConsultaUsuarioRequisicaoValidador : AbstractValidator<ConsultaUsuarioRequisicao>
{
    public ConsultaUsuarioRequisicaoValidador()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("O login é obrigatório.")
            .MaximumLength(50).WithMessage("O login deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");
    }
}
