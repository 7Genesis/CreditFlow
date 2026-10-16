using CreditFlow.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CreditFlow.API.Tests;

// Sobe a API de verdade (controllers, DI, validação), trocando o PostgreSQL pelo
// provider InMemory do EF Core. Cada fábrica usa um banco próprio, então os testes não se misturam.
public class CreditFlowApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var remover = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                         || d.ServiceType.FullName!.Contains("IDbContextOptionsConfiguration"))
                .ToList();
            foreach (var d in remover)
                services.Remove(d);

            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(_dbName));
        });
    }
}
