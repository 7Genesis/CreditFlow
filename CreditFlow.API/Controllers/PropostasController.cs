using CreditFlow.Application.DTOs;
using CreditFlow.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CreditFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController : ControllerBase
{
    private readonly CriarPropostaUseCase _criarUseCase;
    private readonly ObterPropostaUseCase _obterUseCase;

    public PropostasController(
        CriarPropostaUseCase criarUseCase, 
        ObterPropostaUseCase obterUseCase)
    {
        _criarUseCase = criarUseCase;
        _obterUseCase = obterUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPropostaInput input)
    {
        try
        {
            var idGerado = await _criarUseCase.ExecuteAsync(input);
            return StatusCode(201, new { message = "Proposta criada com sucesso", id = idGerado });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var proposta = await _obterUseCase.ExecuteAsync(id);

        if (proposta == null)
            return NotFound(new { message = "Proposta não encontrada." });

        return Ok(proposta);
    }
}