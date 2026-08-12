using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Transfors.Clientes.Api.Data;
using Transfors.Clientes.Api.Domain;
using Transfors.Clientes.Api.Dtos;

namespace Transfors.Clientes.Api.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _db;

    public ClienteService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ClienteResponse>> GetAllAsync(string? search, bool? estado, CancellationToken ct = default)
    {
        // Se demuestra el uso de un STORED PROCEDURE de SQL Server para el listado/búsqueda,
        // mapeado directamente a la entidad Cliente mediante EF Core (FromSqlRaw
        // parametriza los valores, evitando inyección SQL).
        var searchParam = new SqlParameter("@Search", (object?)search ?? DBNull.Value);
        var estadoParam = new SqlParameter("@Estado", (object?)estado ?? DBNull.Value);

        var clientes = await _db.Clientes
            .FromSqlRaw("EXEC dbo.usp_Clientes_Listar @Search, @Estado", searchParam, estadoParam)
            .AsNoTracking()
            .ToListAsync(ct);

        return clientes.Select(c => c.ToResponse()).ToList();
    }

    public async Task<ClienteResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var cliente = await _db.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return cliente?.ToResponse();
    }

    public async Task<ClienteResponse> CreateAsync(ClienteRequest request, CancellationToken ct = default)
    {
        await EnsureDocumentoUnicoAsync(request, id: null, ct);

        var cliente = new Cliente { FechaCreacion = DateTime.UtcNow };
        request.ApplyTo(cliente);

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync(ct);

        return cliente.ToResponse();
    }

    public async Task<ClienteResponse?> UpdateAsync(int id, ClienteRequest request, CancellationToken ct = default)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (cliente is null) return null;

        await EnsureDocumentoUnicoAsync(request, id, ct);

        request.ApplyTo(cliente);
        cliente.FechaModificacion = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return cliente.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (cliente is null) return false;

        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    /// <summary>Valida que no exista otro cliente con el mismo tipo y número de documento.</summary>
    private async Task EnsureDocumentoUnicoAsync(ClienteRequest request, int? id, CancellationToken ct)
    {
        var numero = request.NumeroDocumento.Trim();
        var existe = await _db.Clientes.AnyAsync(
            c => c.TipoDocumento == request.TipoDocumento
                 && c.NumeroDocumento == numero
                 && (id == null || c.Id != id),
            ct);

        if (existe)
            throw new ConflictException(
                $"Ya existe un cliente con {ClienteMapping.Describe(request.TipoDocumento)} número {numero}.");
    }
}
