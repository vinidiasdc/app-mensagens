using ChatServer.Requisicoes;
using FluentValidation;

namespace ChatServer.Validators;

public class CadastroUsuarioRequisicaoValidator : AbstractValidator<CadastroUsuarioRequisicao>
{
    public CadastroUsuarioRequisicaoValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("O login é obrigatório.")
            .MaximumLength(50).WithMessage("O login deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
    }
}
