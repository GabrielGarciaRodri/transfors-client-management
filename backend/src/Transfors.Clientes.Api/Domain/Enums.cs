namespace Transfors.Clientes.Api.Domain;

/// <summary>
/// Tipos de documento soportados. Se persiste como int en la base de datos.
/// </summary>
public enum TipoDocumento
{
    CedulaCiudadania = 1,
    CedulaExtranjeria = 2,
    Pasaporte = 3,
    TarjetaIdentidad = 4,
    Nit = 5
}

/// <summary>
/// Género del cliente. Se persiste como int en la base de datos.
/// </summary>
public enum Genero
{
    Masculino = 1,
    Femenino = 2,
    Otro = 3,
    PrefieroNoDecir = 4
}
