namespace Transfors.Clientes.Api.Services;

/// <summary>
/// Se lanza cuando una operación viola una regla de negocio de unicidad
/// (por ejemplo, documento ya registrado). El controller la traduce a HTTP 409.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
