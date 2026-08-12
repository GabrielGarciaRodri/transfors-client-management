using Transfors.Clientes.Api.Domain;
using Transfors.Clientes.Api.Dtos;

namespace Transfors.Clientes.Api.Services;

/// <summary>
/// Mapeo entre la entidad de dominio y los DTOs, y descripciones legibles de los enums.
/// </summary>
public static class ClienteMapping
{
    public static string Describe(TipoDocumento t) => t switch
    {
        TipoDocumento.CedulaCiudadania => "Cédula de ciudadanía",
        TipoDocumento.CedulaExtranjeria => "Cédula de extranjería",
        TipoDocumento.Pasaporte => "Pasaporte",
        TipoDocumento.TarjetaIdentidad => "Tarjeta de identidad",
        TipoDocumento.Nit => "NIT",
        _ => t.ToString()
    };

    public static string Describe(Genero g) => g switch
    {
        Genero.Masculino => "Masculino",
        Genero.Femenino => "Femenino",
        Genero.Otro => "Otro",
        Genero.PrefieroNoDecir => "Prefiero no decir",
        _ => g.ToString()
    };

    public static ClienteResponse ToResponse(this Cliente c) => new(
        c.Id,
        c.TipoDocumento,
        Describe(c.TipoDocumento),
        c.NumeroDocumento,
        c.Nombres,
        c.Apellidos,
        c.FechaNacimiento,
        c.Genero,
        Describe(c.Genero),
        c.Telefono,
        c.CorreoElectronico,
        c.Direccion,
        c.Ciudad,
        c.Estado,
        c.FechaCreacion,
        c.FechaModificacion);

    public static void ApplyTo(this ClienteRequest req, Cliente c)
    {
        c.TipoDocumento = req.TipoDocumento;
        c.NumeroDocumento = req.NumeroDocumento.Trim();
        c.Nombres = req.Nombres.Trim();
        c.Apellidos = req.Apellidos.Trim();
        c.FechaNacimiento = req.FechaNacimiento;
        c.Genero = req.Genero;
        c.Telefono = req.Telefono.Trim();
        c.CorreoElectronico = req.CorreoElectronico.Trim();
        c.Direccion = req.Direccion.Trim();
        c.Ciudad = req.Ciudad.Trim();
        c.Estado = req.Estado;
    }
}
