using CreditFlow.Application.DTOs;
using CreditFlow.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CreditFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController : ControllerBase
{
    private readonly CriarPropostaUseCase _useCase;

    public PropostasController(CriarPropostaUseCase UseCase)
    {
        _useCase = UseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPropostaInput input)
    {
        try
        {
            //Executa a regra de negócio do Uso Case
            var idGerado = await _useCase.ExecuteAsync(input);

            //Retorna o status HTTP 201 (Created) e o ID gerado no banco
            return StatusCode(201, new { message = "Proposta criada com sucesso", id = idGerado});
        }
        catch (System.Exception ex)
        {
                //Se cair na validação de Domínio(ex: CPF nulo), retornar 400 (Bad Request)
                return BadRequest(new{ erro = ex.Message});
            }
        }
    }

