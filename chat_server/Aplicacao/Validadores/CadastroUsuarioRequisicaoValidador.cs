using Chat.API.Dominio.Dtos.Requisicoes;
using FluentValidation;

namespace Chat.API.Aplicacao.Validadores;

public class CadastroUsuarioRequisicaoValidador : AbstractValidator<CadastroUsuarioRequisicao>
{
    public CadastroUsuarioRequisicaoValidador()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado é inválido.")
            .MaximumLength(254).WithMessage("O e-mail deve ter no máximo 254 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
    }
}
