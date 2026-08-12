using Microsoft.EntityFrameworkCore;
using Transfors.Clientes.Api.Domain;

namespace Transfors.Clientes.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.TipoDocumento)
                  .HasConversion<int>()
                  .IsRequired();

            entity.Property(c => c.NumeroDocumento)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(c => c.Nombres)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(c => c.Apellidos)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(c => c.FechaNacimiento)
                  .HasColumnType("date")
                  .IsRequired();

            entity.Property(c => c.Genero)
                  .HasConversion<int>()
                  .IsRequired();

            entity.Property(c => c.Telefono)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(c => c.CorreoElectronico)
                  .HasMaxLength(150)
                  .IsRequired();

            entity.Property(c => c.Direccion)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(c => c.Ciudad)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(c => c.Estado)
                  .IsRequired();

            entity.Property(c => c.FechaCreacion)
                  .HasDefaultValueSql("SYSUTCDATETIME()");

            // El par TipoDocumento + NumeroDocumento debe ser único:
            // no puede existir el mismo documento repetido.
            entity.HasIndex(c => new { c.TipoDocumento, c.NumeroDocumento })
                  .IsUnique();

            entity.HasIndex(c => c.CorreoElectronico);
        });
    }
}
