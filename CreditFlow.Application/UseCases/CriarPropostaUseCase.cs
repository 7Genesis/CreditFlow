using CreditFlow.Application.DTOs;
using CreditFlow.Domain.Entities;
using CreditFlow.Domain.Repositories;
using FluentValidation;

namespace CreditFlow.Application.UseCases;

public class CriarPropostaUseCase
{
    private readonly IValidator<CriarPropostaInput> _validator;
    private readonly IPropostaCreditoRepository _repository;

    //O .NET vai injetar o repositorio automaticamente aqui via constructor
    public CriarPropostaUseCase(IPropostaCreditoRepository repository, IValidator<CriarPropostaInput> validator)
    {
        _repository = repository;
        _validator = validator;
    }
public async Task<Guid> ExecuteAsync(CriarPropostaInput input)
{
    // 1. Validação explícita: Se os dados forem inválidos, lança uma exceção antes de processar qualquer coisa
    var validationResult = await _validator.ValidateAsync(input);
    
    if (!validationResult.IsValid)
    {
        // Agrupa todos os erros de validação em uma única mensagem
        var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
        throw new ValidationException(errors);
    }

    // 2. Instancia a regra de negocio do Dominio
    var novaProposta = new PropostaCredito(
        input.CpfCliente,
        input.ValorSolicitado,
        input.QuantidadeParcelas
    );

    // 3. Salva no banco de dados
    await _repository.AddAsync(novaProposta);

    return novaProposta.Id;
}
}