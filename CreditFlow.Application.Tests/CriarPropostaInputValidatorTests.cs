using CreditFlow.Application.DTOs;
using CreditFlow.Application.Validators;

namespace CreditFlow.Application.Tests;

public class CriarPropostaInputValidatorTests
{
    private readonly CriarPropostaInputValidator _validator = new();

    [Fact]
    public void Validate_DeveSerValido_QuandoDadosEstaoCorretos()
    {
        var resultado = _validator.Validate(new CriarPropostaInput("52998224725", 15000m, 24));

        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1234567890")]    // 10 dígitos
    [InlineData("529982247252")]  // 12 dígitos
    public void Validate_DeveFalhar_QuandoCpfNaoTem11Caracteres(string cpf)
    {
        var resultado = _validator.Validate(new CriarPropostaInput(cpf, 15000m, 24));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.CpfCliente));
    }

    [Theory]
    [InlineData("12345678901")]   // dígitos verificadores errados
    [InlineData("11111111111")]   // sequência repetida
    [InlineData("5299822472a")]   // não é só número
    public void Validate_DeveFalhar_QuandoCpfEInvalido(string cpf)
    {
        var resultado = _validator.Validate(new CriarPropostaInput(cpf, 15000m, 24));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.CpfCliente));
    }

    [Theory]
    [InlineData("52998224725")]
    [InlineData("11144477735")]
    [InlineData("00000000191")]   // 0 como dígito verificador não pode falhar
    public void Cpf_DeveAceitarCpfsValidos(string cpf)
    {
        Assert.True(Cpf.EhValido(cpf));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_DeveFalhar_QuandoValorNaoEMaiorQueZero(double valor)
    {
        var resultado = _validator.Validate(new CriarPropostaInput("52998224725", (decimal)valor, 24));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.ValorSolicitado));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(61)]
    public void Validate_DeveFalhar_QuandoParcelasForaDoIntervalo(int parcelas)
    {
        var resultado = _validator.Validate(new CriarPropostaInput("52998224725", 15000m, parcelas));

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == nameof(CriarPropostaInput.QuantidadeParcelas));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(60)]
    public void Validate_DeveAceitar_OsLimitesDeParcelas(int parcelas)
    {
        var resultado = _validator.Validate(new CriarPropostaInput("52998224725", 15000m, parcelas));

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Validate_DeveReportarTodosOsErros_QuandoVariosCamposSaoInvalidos()
    {
        var resultado = _validator.Validate(new CriarPropostaInput("123", 0m, 0));

        Assert.Equal(3, resultado.Errors.Count);
    }
}
