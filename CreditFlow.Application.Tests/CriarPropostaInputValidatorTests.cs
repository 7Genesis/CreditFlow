using CreditFlow.Application.DTOs;
using CreditFlow.Application.Validators;

namespace CreditFlow.Application.Tests;

public class CriarPropostaInputValidatorTests
{
    private readonly CriarPropostaInputValidator _validator = new();

    [Fact]
    public void Validate_DeveSerValido_QuandoDadosEstaoCorretos()
    {
        var resultado = _validator.Validate(new CriarPropostaInput("12345678901", 15000m, 24));

        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1234567890")]    // 10 dígitos
    [InlineData("123456789012")]  // 12 dígitos
    public void Validate_DeveFalhar_QuandoCpfNaoTem11Caracteres(string cpf)
    {
        var resultado = _validator.Validate(new CriarPropostaInput(cpf, 15000m, 24));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.CpfCliente));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_DeveFalhar_QuandoValorNaoEMaiorQueZero(double valor)
    {
        var resultado = _validator.Validate(new CriarPropostaInput("12345678901", (decimal)valor, 24));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.ValorSolicitado));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(61)]
    public void Validate_DeveFalhar_QuandoParcelasForaDoIntervalo(int parcelas)
    {
        var resultado = _validator.Validate(new CriarPropostaInput("12345678901", 15000m, parcelas));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.QuantidadeParcelas));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(60)]
    public void Validate_DeveAceitar_OsLimitesDeParcelas(int parcelas)
    {
        var resultado = _validator.Validate(new CriarPropostaInput("12345678901", 15000m, parcelas));

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Validate_DeveReportarTodosOsErros_QuandoVariosCamposSaoInvalidos()
    {
        var resultado = _validator.Validate(new CriarPropostaInput("123", 0m, 0));

        Assert.Equal(3, resultado.Errors.Count);
    }
}
