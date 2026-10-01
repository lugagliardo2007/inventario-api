using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{

    public class InventarioService
    {
        private readonly AppDbContext _context;

        public InventarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MovimientoStock> RegistrarMovimientoAsync(long productoId, TipoMovimiento tipo, int cantidad, string? motivo)
        {
            if (cantidad <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a 0.");

            var producto = await _context.Productos.FindAsync(productoId)
                ?? throw new KeyNotFoundException($"No existe el producto con ID {productoId}.");

            if (!producto.Activo)
                throw new InvalidOperationException("El producto está inactivo.");

            if (tipo == TipoMovimiento.Salida)
            {
                if (!producto.HayStockSuficiente(cantidad))
                    throw new InvalidOperationException($"Stock insuficiente. Disponible: {producto.StockActual}, solicitado: {cantidad}.");

                producto.StockActual -= cantidad;
            }
            else
            {
                producto.StockActual += cantidad;
            }

            var movimiento = new MovimientoStock
            {
                Fecha = DateTime.Now,
                Tipo = tipo,
                Cantidad = cantidad,
                Motivo = motivo,
                ProductoId = producto.Id
            };
            _context.Movimientos.Add(movimiento);

            await VerificarAlertaAsync(producto);

            await _context.SaveChangesAsync();

            return movimiento;
        }


        public async Task VerificarAlertaAsync(Producto producto)
        {
            if (!producto.RequiereAlerta())
                return;

            bool yaHayAlerta = await _context.Alertas
                .AnyAsync(a => a.ProductoId == producto.Id && !a.Leida);
            if (yaHayAlerta)
                return;

            _context.Alertas.Add(new Alerta
            {
                Fecha = DateTime.Now,
                Leida = false,
                Producto = producto,
                Mensaje = $"Stock bajo en '{producto.Nombre}' (SKU {producto.Sku}): " +
                          $"quedan {producto.StockActual} unidades y el mínimo es {producto.StockMinimo}."
            });
        }
    }
}
