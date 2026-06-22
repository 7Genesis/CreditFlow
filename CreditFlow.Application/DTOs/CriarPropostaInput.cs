namespace CreditFlow.Application.DTOs;

// Usando o recurso de 'record' do C#, ideal para DTOs por serem imutaveis e leves
public record CriarPropostaInput(
    string CpfCliente,
    decimal ValorSolicitado,
    int QuantidadeParcelas
);
