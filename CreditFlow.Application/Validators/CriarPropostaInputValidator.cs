using CreditFlow.Application.DTOs;
using FluentValidation;

namespace CreditFlow.Application.Validators;

public class CriarPropostaInputValidator : AbstractValidator<CriarPropostaInput>
{
    public CriarPropostaInputValidator()
    {
        RuleFor(x => x.CpfCliente)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(11).WithMessage("O CPF deve conter 11 dígitos.")
            .Must(Cpf.EhValido).WithMessage("O CPF é inválido.");

        RuleFor(x => x.ValorSolicitado)
            .GreaterThan(0).WithMessage("O valor da proposta deve ser maior que zero.");

        RuleFor(x => x.QuantidadeParcelas)
            .InclusiveBetween(1, 60).WithMessage("A quantidade de parcelas deve ser entre 1 e 60.");
    }
}