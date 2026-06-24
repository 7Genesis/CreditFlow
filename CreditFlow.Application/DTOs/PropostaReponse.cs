namespace CreditFlow.Application.DTOs;

// Usando o recurso de 'record' do C#, ideal para DTOs por serem imutaveis e leves
public record PropostaResponse(
    Guid Id,
    string CpfCliente,
    decimal ValorSolicitado,
    int QuantidadeParcelas,
    string Status,
    DateTime DataCriacao
);
