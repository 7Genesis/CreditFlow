using CreditFlow.Domain.Enums;

namespace CreditFlow.Domain.Entities;

public class PropostaCredito
{
    public Guid Id { get; private set; }
    public string CpfCliente { get; private set; }
    public decimal ValorSolicitado { get; private set; }
    public int QuantidadeParcelas { get; private set; }
    public StatusProposta Status { get; private set; }
    public DateTime DataCriacao { get; private set; }

    // Construtor que você já criou anteriormente
    public PropostaCredito(string cpfCliente, decimal valorSolicitado, int quantidadeParcelas)
    {
        // ... (mantenha suas validações de CPF e valor aqui) ...
        
        Id = Guid.NewGuid();
        CpfCliente = cpfCliente;
        ValorSolicitado = valorSolicitado;
        QuantidadeParcelas = quantidadeParcelas;
        Status = StatusProposta.EmAnalise; // Status inicial padrão
        DataCriacao = DateTime.UtcNow;
    }

    
    public void Aprovar()
    {
        if (Status != StatusProposta.EmAnalise)
            throw new ArgumentException("Apenas propostas em análise podem ser aprovadas.");
        
        Status = StatusProposta.Aprovada;
    }

    
    public void Recusar()
    {
        if (Status != StatusProposta.EmAnalise)
            throw new ArgumentException("Apenas propostas em análise podem ser recusadas.");
        
        Status = StatusProposta.Rejeitada;
    }
}