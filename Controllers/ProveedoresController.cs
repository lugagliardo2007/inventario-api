using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            return await _context.Proveedores.OrderBy(p => p.RazonSocial).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(long id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();
            return proveedor;
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            if (await _context.Proveedores.AnyAsync(p => p.Cuit == proveedor.Cuit))
                return Conflict($"Ya existe un proveedor con el CUIT {proveedor.Cuit}.");

            proveedor.Id = 0;
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(long id, Proveedor datos)
        {
            if (id != datos.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();

            if (await _context.Proveedores.AnyAsync(p => p.Cuit == datos.Cuit && p.Id != id))
                return Conflict($"Ya existe otro proveedor con el CUIT {datos.Cuit}.");

            proveedor.RazonSocial = datos.RazonSocial;
            proveedor.Cuit = datos.Cuit;
            proveedor.Telefono = datos.Telefono;
            proveedor.Direccion = datos.Direccion;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(long id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();

            if (await _context.Productos.AnyAsync(p => p.ProveedorId == id))
                return Conflict("No se puede eliminar el proveedor porque tiene productos asociados.");

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
