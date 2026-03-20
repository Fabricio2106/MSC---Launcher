using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pedido.Data;
using pedido.Models;

namespace pedido.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoDbContext _context;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(PedidoDbContext context, ILogger<PedidosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// Obtiene todos los pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            _logger.LogInformation("Obteniendo lista de pedidos");
            try
            {
                var pedidos = await _context.Pedidos.ToListAsync();
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pedidos");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// Obtiene un pedido por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedidoById(int id)
        {
            _logger.LogInformation("Obteniendo pedido con ID: {id}", id);
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido con ID {id} no encontrado", id);
                    return NotFound(new { message = "Pedido no encontrado" });
                }

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pedido con ID {id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// Registra un nuevo pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> CreatePedido([FromBody] Pedido pedido)
        {
            _logger.LogInformation("Creando nuevo pedido");
            try
            {
                if (pedido == null)
                {
                    _logger.LogWarning("Intento de crear pedido con datos nulos");
                    return BadRequest(new { message = "Los datos del pedido no pueden estar vacíos" });
                }

                if (string.IsNullOrWhiteSpace(pedido.Numero))
                {
                    return BadRequest(new { message = "El número del pedido es requerido" });
                }

                if (pedido.IdCliente <= 0)
                {
                    return BadRequest(new { message = "El ID del cliente es inválido" });
                }

                if (pedido.Fecha == default)
                {
                    pedido.Fecha = DateTime.Now;
                }

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Pedido creado exitosamente con ID: {id}", pedido.Id);
                return CreatedAtAction(nameof(GetPedidoById), new { id = pedido.Id }, pedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// Actualiza un pedido existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, [FromBody] Pedido pedidoActualizado)
        {
            _logger.LogInformation("Actualizando pedido con ID: {id}", id);
            try
            {
                if (pedidoActualizado == null)
                {
                    _logger.LogWarning("Intento de actualizar pedido con datos nulos");
                    return BadRequest(new { message = "Los datos del pedido no pueden estar vacíos" });
                }

                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido con ID {id} no encontrado para actualizar", id);
                    return NotFound(new { message = "Pedido no encontrado" });
                }

                if (!string.IsNullOrWhiteSpace(pedidoActualizado.Numero))
                {
                    pedido.Numero = pedidoActualizado.Numero;
                }

                if (pedidoActualizado.Fecha != default)
                {
                    pedido.Fecha = pedidoActualizado.Fecha;
                }

                if (pedidoActualizado.IdCliente > 0)
                {
                    pedido.IdCliente = pedidoActualizado.IdCliente;
                }

                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Pedido con ID {id} actualizado exitosamente", id);
                return Ok(new { message = "Pedido actualizado exitosamente", pedido });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar pedido con ID {id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// Elimina un pedido por su ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            _logger.LogInformation("Eliminando pedido con ID: {id}", id);
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido == null)
                {
                    _logger.LogWarning("Pedido con ID {id} no encontrado para eliminar", id);
                    return NotFound(new { message = "Pedido no encontrado" });
                }

                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Pedido con ID {id} eliminado exitosamente", id);
                return Ok(new { message = "Pedido eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar pedido con ID {id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
