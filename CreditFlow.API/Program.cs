using CreditFlow.Application.UseCases;
using CreditFlow.Domain.Repositories;
using CreditFlow.Infrastructure.Data;
using CreditFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//1. Pegar string de conexão do appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//2. Registrar o DbContext com a string da Infrastructura no PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//3. Registrar o repositório(Injeção de dependência)
// Sempre que aplicação pedir IPropostaCreditoRepository, o .NET injeta PropostaCreditoRepository
builder.Services.AddScoped<IPropostaCreditoRepository, PropostaCreditoRepository>();

//4.Registrar os Casos de Uso da camada de aplicação
builder.Services.AddScoped<AtualizarStatusPropostaUseCase>();
builder.Services.AddScoped<CriarPropostaUseCase>();
builder.Services.AddScoped<ObterPropostaUseCase>();


//5. Adiciona o suporte a Controllers no contêiner
builder.Services.AddControllers();

var app = builder.Build();

//6. Mapeia os endpoints dos Controllers para as rotas da API
app.MapControllers();

// Endpoint de teste rápido para verificar se a API está funcionando
app.MapGet("/", () => "API de Propostas de Crédito está funcionando!");

app.Run();