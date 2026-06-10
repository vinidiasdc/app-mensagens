using Chat.API.Dominio.Dtos.Requisicoes;
using FluentValidation;

namespace Chat.API.Aplicacao.Validadores;

public class ConsultaUsuarioRequisicaoValidador : AbstractValidator<ConsultaUsuarioRequisicao>
{
    public ConsultaUsuarioRequisicaoValidador()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado é inválido.")
            .MaximumLength(254).WithMessage("O e-mail deve ter no máximo 254 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");
    }
}
