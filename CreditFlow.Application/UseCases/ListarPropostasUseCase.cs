using CreditFlow.Application.DTOs;
using CreditFlow.Domain.Repositories;

namespace CreditFlow.Application.UseCases;

public class ListarPropostasUseCase
{
    private readonly IPropostaCreditoRepository _repository;

    public ListarPropostasUseCase(IPropostaCreditoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PropostaResponse>> ExecuteAsync()
    {
        var propostas = await _repository.GetAllAsync();

        // Converte (Mapeia) a lista de Entidades para uma lista de DTOs
        return propostas.Select(p => new PropostaResponse(
            p.Id,
            p.CpfCliente,
            p.ValorSolicitado,
            p.QuantidadeParcelas,
            p.Status.ToString(),
            p.DataCriacao
        ));
    }
}