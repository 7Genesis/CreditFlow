using CreditFlow.Domain.Entities;
using CreditFlow.Domain.Repositories;
using CreditFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditFlow.Infrastructure.Repositories;

public class PropostaCreditoRepository : IPropostaCreditoRepository
{
    private readonly AppDbContext _context;

    public PropostaCreditoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PropostaCredito proposta)
    {
        await _context.Propostas.AddAsync(proposta);
        await _context.SaveChangesAsync();
    }

    public async Task<PropostaCredito?> GetByIdAsync(Guid id)
    {
        return await _context.Propostas.FindAsync(id);
    }

    public async Task UpdateAsync(PropostaCredito proposta)
    {
        _context.Propostas.Update(proposta);
        await _context.SaveChangesAsync();
    }
}