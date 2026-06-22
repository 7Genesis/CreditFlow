using CreditFlow.Application.DTOs;
using CreditFlow.Domain.Entities;
using CreditFlow.Domain.Repositories;

namespace CreditFlow.Application.UseCases;

public class CriarPropostaUseCase
{
    private readonly IPropostaCreditoRepository _repository;

    //O .NET vai injetar o repositorio automaticamente aqui via constructor
    public CriarPropostaUseCase(IPropostaCreditoRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> ExecuteAsync(CriarPropostaInput input)
    {
        //1. Instancia a regra de negocio do Dominio
        var novaProposta = new PropostaCredito(
            input.CpfCliente,
            input.ValorSolicitado,
            input.QuantidadeParcelas
        );

        //2. Salva no banco de dados atraves da abastracao do repositorio
        await _repository.AddAsync(novaProposta);

        //3. Retorna o ID gerado para a API responder ao usuario
        return novaProposta.Id;
    }
}