using CreditFlow.Application.DTOs;
using CreditFlow.Domain.Repositories;

namespace CreditFlow.Application.UseCases;

public class AtualizarStatusPropostaUseCase
{
    private readonly IPropostaCreditoRepository _repository;

    public AtualizarStatusPropostaUseCase(IPropostaCreditoRepository repository)
    {
        _repository = repository;
    }
    
    public async Task ExecuteAsync(Guid id, AtualizarStatusInput input)
    {
        // 1.Busca a entediade no banco de dados
        var proposta = await _repository.GetByIdAsync(id);

        if(proposta == null)
            throw new ArgumentException("Proposta não encontrada.");

        // 2. Executa a regra de negócio na entidade de dominio
        if(input.Aprovado)
            proposta.Aprovar();
        else
            proposta.Recusar();

        //3. Persiste a alteração
        await _repository.UpdateAsync(proposta);
    }
}