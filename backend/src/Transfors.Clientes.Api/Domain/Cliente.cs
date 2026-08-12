namespace Transfors.Clientes.Api.Domain;

/// <summary>
/// Entidad de dominio que representa a un cliente.
/// Mapea a la tabla dbo.Clientes en SQL Server (EF Core Code-First).
/// </summary>
public class Cliente
{
    public int Id { get; set; }

    public TipoDocumento TipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public DateOnly FechaNacimiento { get; set; }

    public Genero Genero { get; set; }

    public string Telefono { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Ciudad { get; set; } = string.Empty;

    /// <summary>Estado del cliente: true = Activo, false = Inactivo.</summary>
    public bool Estado { get; set; } = true;

    // Campos de auditoría (agregados para trazabilidad; la prueba permite añadir campos).
    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
