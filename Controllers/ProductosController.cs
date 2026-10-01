using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly InventarioService _inventario;

        public ProductosController(AppDbContext context, InventarioService inventario)
        {
            _context = context;
            _inventario = inventario;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos([FromQuery] bool incluirInactivos = false)
        {
            var consulta = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .AsQueryable();

            if (!incluirInactivos)
                consulta = consulta.Where(p => p.Activo);

            return await consulta.OrderBy(p => p.Nombre).ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(long id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null) return NotFound();

            return producto;
        }

        
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            producto.Id = 0;
            producto.Categoria = null;
            producto.Proveedor = null;

            if (!await _context.Categorias.AnyAsync(c => c.Id == producto.CategoriaId))
                return BadRequest($"No existe la categoría con ID {producto.CategoriaId}.");

            if (!await _context.Proveedores.AnyAsync(p => p.Id == producto.ProveedorId))
                return BadRequest($"No existe el proveedor con ID {producto.ProveedorId}.");

            if (await _context.Productos.AnyAsync(p => p.Sku == producto.Sku))
                return Conflict($"Ya existe un producto con el SKU {producto.Sku}.");

            await _inventario.VerificarAlertaAsync(producto);
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(long id, Producto datos)
        {
            if (id != datos.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            if (!await _context.Categorias.AnyAsync(c => c.Id == datos.CategoriaId))
                return BadRequest($"No existe la categoría con ID {datos.CategoriaId}.");

            if (!await _context.Proveedores.AnyAsync(p => p.Id == datos.ProveedorId))
                return BadRequest($"No existe el proveedor con ID {datos.ProveedorId}.");

            if (await _context.Productos.AnyAsync(p => p.Sku == datos.Sku && p.Id != id))
                return Conflict($"Ya existe otro producto con el SKU {datos.Sku}.");

            producto.Sku = datos.Sku;
            producto.Nombre = datos.Nombre;
            producto.Descripcion = datos.Descripcion;
            producto.Precio = datos.Precio;
            producto.StockMinimo = datos.StockMinimo;
            producto.Activo = datos.Activo;
            producto.CategoriaId = datos.CategoriaId;
            producto.ProveedorId = datos.ProveedorId;

            await _inventario.VerificarAlertaAsync(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(long id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            producto.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
