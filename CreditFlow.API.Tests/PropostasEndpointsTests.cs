using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CreditFlow.API.Tests;

public class PropostasEndpointsTests : IClassFixture<CreditFlowApiFactory>
{
    private readonly HttpClient _client;

    public PropostasEndpointsTests(CreditFlowApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<Guid> CriarPropostaAsync(string cpf = "52998224725", decimal valor = 15000m, int parcelas = 24)
    {
        var resposta = await _client.PostAsJsonAsync("/api/propostas",
            new { cpfCliente = cpf, valorSolicitado = valor, quantidadeParcelas = parcelas });
        Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

        var corpo = await resposta.Content.ReadFromJsonAsync<JsonElement>();
        return corpo.GetProperty("id").GetGuid();
    }

    [Fact]
    public async Task Get_Raiz_RetornaMensagemDeSaude()
    {
        var resposta = await _client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
    }

    [Fact]
    public async Task Post_CriaProposta_E_Get_RetornaEmAnalise()
    {
        var id = await CriarPropostaAsync();

        var resposta = await _client.GetAsync($"/api/propostas/{id}");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var proposta = await resposta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("EmAnalise", proposta.GetProperty("status").GetString());
        Assert.Equal("52998224725", proposta.GetProperty("cpfCliente").GetString());
    }

    [Fact]
    public async Task Post_RetornaBadRequest_QuandoValidacaoFalha()
    {
        var resposta = await _client.PostAsJsonAsync("/api/propostas",
            new { cpfCliente = "123", valorSolicitado = 0, quantidadeParcelas = 99 });

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    [Fact]
    public async Task Get_PorId_RetornaNotFound_QuandoNaoExiste()
    {
        var resposta = await _client.GetAsync($"/api/propostas/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
    }

    [Fact]
    public async Task Get_Lista_IncluiPropostaCriada()
    {
        var id = await CriarPropostaAsync(cpf: "11144477735");

        var resposta = await _client.GetAsync("/api/propostas");

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        var lista = await resposta.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Contains(lista.EnumerateArray(), p => p.GetProperty("id").GetGuid() == id);
    }

    [Fact]
    public async Task Put_Status_Aprova_E_RetornaNoContent()
    {
        var id = await CriarPropostaAsync();

        var resposta = await _client.PutAsJsonAsync($"/api/propostas/{id}/status", new { aprovado = true });

        Assert.Equal(HttpStatusCode.NoContent, resposta.StatusCode);
        var proposta = await _client.GetFromJsonAsync<JsonElement>($"/api/propostas/{id}");
        Assert.Equal("Aprovada", proposta.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Put_Status_RetornaBadRequest_QuandoPropostaJaFoiDecidida()
    {
        var id = await CriarPropostaAsync();
        await _client.PutAsJsonAsync($"/api/propostas/{id}/status", new { aprovado = false });

        var resposta = await _client.PutAsJsonAsync($"/api/propostas/{id}/status", new { aprovado = true });

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    [Fact]
    public async Task Put_Status_RetornaBadRequest_QuandoPropostaNaoExiste()
    {
        var resposta = await _client.PutAsJsonAsync($"/api/propostas/{Guid.NewGuid()}/status", new { aprovado = true });

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }
}
