using System.ComponentModel.DataAnnotations;
using Transfors.Clientes.Api.Domain;

namespace Transfors.Clientes.Api.Dtos;

/// <summary>
/// Datos que la API devuelve al cliente HTTP. No expone la entidad directamente.
/// </summary>
public record ClienteResponse(
    int Id,
    TipoDocumento TipoDocumento,
    string TipoDocumentoDescripcion,
    string NumeroDocumento,
    string Nombres,
    string Apellidos,
    DateOnly FechaNacimiento,
    Genero Genero,
    string GeneroDescripcion,
    string Telefono,
    string CorreoElectronico,
    string Direccion,
    string Ciudad,
    bool Estado,
    DateTime FechaCreacion,
    DateTime? FechaModificacion);

/// <summary>
/// Payload para crear o actualizar un cliente. Incluye validaciones declarativas.
/// </summary>
public class ClienteRequest
{
    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    [EnumDataType(typeof(TipoDocumento), ErrorMessage = "Tipo de documento no válido.")]
    public TipoDocumento TipoDocumento { get; set; }

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El número de documento debe tener entre 3 y 20 caracteres.")]
    [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "El número de documento solo admite letras, números y guiones.")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 2)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, MinimumLength = 2)]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El género es obligatorio.")]
    [EnumDataType(typeof(Genero), ErrorMessage = "Género no válido.")]
    public Genero Genero { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, MinimumLength = 7)]
    [RegularExpression(@"^[0-9\+\-\s]+$", ErrorMessage = "El teléfono solo admite números, espacios, + y -.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [StringLength(150)]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [StringLength(200, MinimumLength = 3)]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ciudad es obligatoria.")]
    [StringLength(100, MinimumLength = 2)]
    public string Ciudad { get; set; } = string.Empty;

    public bool Estado { get; set; } = true;
}
