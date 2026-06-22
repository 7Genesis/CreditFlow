using CreditFlow.Domain.Entities;

namespace CreditFlow.Domain.Repositories;

public interface IPropostaCreditoRepository
{
    Task AddAsync(PropostaCredito proposta);
    Task<PropostaCredito?> GetByIdAsync(Guid id);
    Task UpdateAsync(PropostaCredito proposta);
}