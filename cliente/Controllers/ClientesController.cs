using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cliente.Data;
using cliente.Models;

namespace cliente.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteDbContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ClienteDbContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            _logger.LogInformation("Obteniendo lista de clientes");
            try
            {
                var clientes = await _context.Clientes.ToListAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClienteById(int id)
        {
            _logger.LogInformation("Obteniendo cliente con ID: {id}", id);
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {id} no encontrado", id);
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con ID {id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente([FromBody] Cliente cliente)
        {
            _logger.LogInformation("Creando nuevo cliente");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                cliente.FechaRegistro = DateTime.UtcNow;
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente creado exitosamente con ID: {id}", cliente.Id);
                return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, new { message = "Error al crear cliente" });
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente clienteActualizado)
        {
            if (id != clienteActualizado.Id)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            _logger.LogInformation("Actualizando cliente con ID: {id}", id);

            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {id} no encontrado para actualizar", id);
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                cliente.Nombre = clienteActualizado.Nombre;
                cliente.Apellido = clienteActualizado.Apellido;
                cliente.Email = clienteActualizado.Email;
                cliente.Telefono = clienteActualizado.Telefono;
                cliente.Direccion = clienteActualizado.Direccion;
                cliente.Activo = clienteActualizado.Activo;

                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente con ID {id} actualizado exitosamente", id);
                return Ok(new { message = "Cliente actualizado exitosamente", cliente = cliente });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente con ID {id}", id);
                return StatusCode(500, new { message = "Error al actualizar cliente" });
            }
        }

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            _logger.LogInformation("Eliminando cliente con ID: {id}", id);

            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {id} no encontrado para eliminar", id);
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente con ID {id} eliminado exitosamente", id);
                return Ok(new { message = "Cliente eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente con ID {id}", id);
                return StatusCode(500, new { message = "Error al eliminar cliente" });
            }
        }
    }
}
