using Microsoft.AspNetCore.Mvc;
using Transfors.Clientes.Api.Dtos;
using Transfors.Clientes.Api.Services;

namespace Transfors.Clientes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service) => _service = service;

    /// <summary>Lista clientes, con búsqueda por texto (nombres, apellidos, documento, correo) y filtro por estado.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClienteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClienteResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? estado,
        CancellationToken ct)
    {
        var clientes = await _service.GetAllAsync(search, estado, ct);
        return Ok(clientes);
    }

    /// <summary>Obtiene un cliente por su identificador.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteResponse>> GetById(int id, CancellationToken ct)
    {
        var cliente = await _service.GetByIdAsync(id, ct);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    /// <summary>Crea un nuevo cliente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteResponse>> Create([FromBody] ClienteRequest request, CancellationToken ct)
    {
        var cliente = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    /// <summary>Actualiza un cliente existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteResponse>> Update(int id, [FromBody] ClienteRequest request, CancellationToken ct)
    {
        var cliente = await _service.UpdateAsync(id, request, ct);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    /// <summary>Elimina un cliente.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var eliminado = await _service.DeleteAsync(id, ct);
        return eliminado ? NoContent() : NotFound();
    }
}
