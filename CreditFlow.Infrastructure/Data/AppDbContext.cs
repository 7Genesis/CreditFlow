using CreditFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditFlow.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PropostaCredito> Propostas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração fluente (Fluent API) para mapear a entidade
        modelBuilder.Entity<PropostaCredito>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.CpfCliente)
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(e => e.ValorSolicitado)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.QuantidadeParcelas)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>(); // Salva o Enum como Integer no banco

            entity.Property(e => e.DataCriacao)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}