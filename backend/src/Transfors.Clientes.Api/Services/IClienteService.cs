using Transfors.Clientes.Api.Dtos;

namespace Transfors.Clientes.Api.Services;

public interface IClienteService
{
    /// <summary>Lista clientes con filtro opcional de búsqueda y estado (vía stored procedure).</summary>
    Task<IReadOnlyList<ClienteResponse>> GetAllAsync(string? search, bool? estado, CancellationToken ct = default);

    Task<ClienteResponse?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Crea un cliente. Lanza <see cref="ConflictException"/> si el documento ya existe.</summary>
    Task<ClienteResponse> CreateAsync(ClienteRequest request, CancellationToken ct = default);

    /// <summary>Actualiza un cliente. Devuelve null si no existe; lanza <see cref="ConflictException"/> por documento duplicado.</summary>
    Task<ClienteResponse?> UpdateAsync(int id, ClienteRequest request, CancellationToken ct = default);

    /// <summary>Elimina un cliente. Devuelve false si no existe.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
