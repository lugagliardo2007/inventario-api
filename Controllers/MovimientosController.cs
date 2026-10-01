using System.ComponentModel.DataAnnotations;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    
    public class MovimientoRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Indicá un producto válido.")]
        public long ProductoId { get; set; }

        public TipoMovimiento Tipo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [StringLength(200)]
        public string? Motivo { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly InventarioService _inventario;

        public MovimientosController(AppDbContext context, InventarioService inventario)
        {
            _context = context;
            _inventario = inventario;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoStock>>> GetMovimientos([FromQuery] long? productoId)
        {
            var consulta = _context.Movimientos.Include(m => m.Producto).AsQueryable();

            if (productoId.HasValue)
                consulta = consulta.Where(m => m.ProductoId == productoId.Value);

            return await consulta.OrderByDescending(m => m.Fecha).ToListAsync();
        }

        
        [HttpPost]
        public async Task<ActionResult<MovimientoStock>> PostMovimiento(MovimientoRequest request)
        {
            try
            {
                var movimiento = await _inventario.RegistrarMovimientoAsync(
                    request.ProductoId, request.Tipo, request.Cantidad, request.Motivo);

                return Ok(movimiento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
