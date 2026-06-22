using CreditFlow.Domain.Enums;

namespace CreditFlow.Domain.Entities;

public class PropostaCredito
{
    public Guid Id { get; private set; }
    public string CpfCliente { get; private set; } = string.Empty;
    public decimal ValorSolicitado { get; private set; }
    public int QuantidadeParcelas { get; private set; }
    public StatusProposta Status { get; private set; }
    public DateTime DataCriacao { get; private set; }

    protected PropostaCredito() { }

    public PropostaCredito(string cpfCliente, decimal valorSolicitado, int quantidadeParcelas)
    {
        if(string.IsNullOrWhiteSpace(cpfCliente))
            throw new ArgumentException("CPF do cliente é obrigatório.");

        if(valorSolicitado <= 0)
            throw new ArgumentException("Valor solicitado deve ser maior que zero.");

        if(quantidadeParcelas <= 0)
            throw new ArgumentException("Quantidade de parcelas deve ser maior que zero.");

        Id = Guid.NewGuid();
        CpfCliente = cpfCliente;
        ValorSolicitado = valorSolicitado;
        QuantidadeParcelas = quantidadeParcelas;
        Status = StatusProposta.EmAnalise;
        DataCriacao = DateTime.UtcNow;
    }

public void Aprovar()
    {
        if (Status != StatusProposta.EmAnalise)
            throw new InvalidOperationException("Apenas propostas em análise podem ser aprovadas.");

        Status = StatusProposta.Aprovada;
    }
public void Rejeitar()
    {
        if (Status != StatusProposta.EmAnalise)
            throw new InvalidOperationException("Apenas propostas em análise podem ser recusadas.");

        Status = StatusProposta.Rejeitada;
    }
}