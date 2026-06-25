using CreditFlow.Domain.Entities;
using CreditFlow.Domain.Enums;

namespace CreditFlow.Domain.Tests;

public class PropostaCreditoTests
{
    [Fact]
    public void Aprovar_DeveAlterarStatusParaAprovada_QuandoPropostaEstiverEmAnalise()
    {
        //Arrange (preparação: cria uma proposta válida, que nasce como "EmAnalise")
        var proposta = new PropostaCredito("12345678901", 15000m, 24);

        //Act (Ação: chama o método que queremos testar)
        proposta.Aprovar();

        //Assert (Validação: garante que o resultado é exatamente o esperado)
        Assert.Equal(StatusProposta.Aprovada, proposta.Status);
    }

    [Fact]
    public void Recusar_DeveAlterarStatusParaRejeitada_QuandoPropostaEstiverEmAnalise()
    {
        //Arrange
        var proposta = new PropostaCredito("12345678901", 15000m, 24);

        //Act
        proposta.Recusar();

        //Assert
        Assert.Equal(StatusProposta.Rejeitada, proposta.Status);
    }

    [Fact]
    public void Aprovar_DeveLancarExcecao_QuandoPropostaJaEstiverRejeitada()
    {
        //Arrange
        var proposta = new PropostaCredito("12345678901", 15000m, 24);
        proposta.Recusar(); //Força o status para Rejeitada primeiro

        //Act & Assert (Verifica se a tentativa de aprovar lança o erro esperado)
        var excecao = Assert.Throws<ArgumentException>(() => proposta.Aprovar());
        Assert.Equal("Apenas propostas em análise podem ser aprovadas.", excecao.Message);
    }
}