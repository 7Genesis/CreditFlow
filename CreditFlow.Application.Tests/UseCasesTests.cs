using CreditFlow.Application.DTOs;
using CreditFlow.Application.UseCases;
using CreditFlow.Application.Validators;
using CreditFlow.Domain.Entities;
using CreditFlow.Domain.Enums;
using FluentValidation;

namespace CreditFlow.Application.Tests;

public class UseCasesTests
{
    private readonly FakePropostaRepository _repository = new();

    [Fact]
    public async Task Criar_DevePersistirPropostaEmAnalise_QuandoEntradaEValida()
    {
        var useCase = new CriarPropostaUseCase(_repository, new CriarPropostaInputValidator());

        var id = await useCase.ExecuteAsync(new CriarPropostaInput("52998224725", 15000m, 24));

        var salva = Assert.Single(_repository.Propostas);
        Assert.Equal(id, salva.Id);
        Assert.Equal(StatusProposta.EmAnalise, salva.Status);
    }

    [Fact]
    public async Task Criar_DeveLancarValidationException_ENaoPersistir_QuandoEntradaEInvalida()
    {
        var useCase = new CriarPropostaUseCase(_repository, new CriarPropostaInputValidator());

        await Assert.ThrowsAsync<ValidationException>(
            () => useCase.ExecuteAsync(new CriarPropostaInput("123", 0m, 0)));

        Assert.Empty(_repository.Propostas);
    }

    [Fact]
    public async Task Obter_DeveRetornarNulo_QuandoPropostaNaoExiste()
    {
        var useCase = new ObterPropostaUseCase(_repository);

        var resultado = await useCase.ExecuteAsync(Guid.NewGuid());

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Obter_DeveMapearPropostaParaResponse_QuandoExiste()
    {
        var proposta = new PropostaCredito("12345678901", 15000m, 24);
        _repository.Propostas.Add(proposta);

        var resultado = await new ObterPropostaUseCase(_repository).ExecuteAsync(proposta.Id);

        Assert.NotNull(resultado);
        Assert.Equal(proposta.Id, resultado.Id);
        Assert.Equal("EmAnalise", resultado.Status);
        Assert.Equal(15000m, resultado.ValorSolicitado);
    }

    [Fact]
    public async Task Listar_DeveRetornarTodasAsPropostas()
    {
        _repository.Propostas.Add(new PropostaCredito("12345678901", 1000m, 6));
        _repository.Propostas.Add(new PropostaCredito("10987654321", 2000m, 12));

        var resultado = await new ListarPropostasUseCase(_repository).ExecuteAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Theory]
    [InlineData(true, StatusProposta.Aprovada)]
    [InlineData(false, StatusProposta.Rejeitada)]
    public async Task AtualizarStatus_DeveAplicarDecisaoEPersistir(bool aprovado, StatusProposta esperado)
    {
        var proposta = new PropostaCredito("12345678901", 15000m, 24);
        _repository.Propostas.Add(proposta);

        await new AtualizarStatusPropostaUseCase(_repository)
            .ExecuteAsync(proposta.Id, new AtualizarStatusInput(aprovado));

        Assert.Equal(esperado, proposta.Status);
        Assert.Equal(1, _repository.UpdateCount);
    }

    [Fact]
    public async Task AtualizarStatus_DeveLancarArgumentException_QuandoPropostaNaoExiste()
    {
        var useCase = new AtualizarStatusPropostaUseCase(_repository);

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecuteAsync(Guid.NewGuid(), new AtualizarStatusInput(true)));

        Assert.Equal("Proposta não encontrada.", ex.Message);
    }

    [Fact]
    public async Task AtualizarStatus_NaoDevePersistir_QuandoTransicaoNaoEPermitida()
    {
        var proposta = new PropostaCredito("12345678901", 15000m, 24);
        proposta.Recusar();
        _repository.Propostas.Add(proposta);

        await Assert.ThrowsAsync<ArgumentException>(
            () => new AtualizarStatusPropostaUseCase(_repository)
                .ExecuteAsync(proposta.Id, new AtualizarStatusInput(true)));

        Assert.Equal(0, _repository.UpdateCount);
    }
}
