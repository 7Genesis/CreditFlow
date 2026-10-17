using CreditFlow.Domain.Entities;
using CreditFlow.Domain.Repositories;

namespace CreditFlow.Application.Tests;

// Repositório em memória para testar os casos de uso sem banco.
public class FakePropostaRepository : IPropostaCreditoRepository
{
    public List<PropostaCredito> Propostas { get; } = new();
    public int UpdateCount { get; private set; }

    public Task AddAsync(PropostaCredito proposta)
    {
        Propostas.Add(proposta);
        return Task.CompletedTask;
    }

    public Task<PropostaCredito?> GetByIdAsync(Guid id) =>
        Task.FromResult(Propostas.FirstOrDefault(p => p.Id == id));

    public Task UpdateAsync(PropostaCredito proposta)
    {
        UpdateCount++;
        return Task.CompletedTask;
    }

    public Task<IEnumerable<PropostaCredito>> GetAllAsync() =>
        Task.FromResult<IEnumerable<PropostaCredito>>(Propostas.ToList());
}
