using CreditFlow.Application.DTOs;
using CreditFlow.Domain.Repositories;

namespace CreditFlow.Application.UseCases;

public class ObterPropostaUseCase
{
    private readonly IPropostaCreditoRepository _propostaRepository;

    public ObterPropostaUseCase(IPropostaCreditoRepository repository)
    {
        _propostaRepository = repository;
    }
    
    public async Task<PropostaResponse?> ExecuteAsync(Guid id)
    {
        var proposta = await _propostaRepository.GetByIdAsync(id);
        if (proposta == null)
            return null;

        return new PropostaResponse(
            proposta.Id,
            proposta.CpfCliente,
            proposta.ValorSolicitado,
            proposta.QuantidadeParcelas,
            proposta.Status.ToString(),
            proposta.DataCriacao
        );
    }
}