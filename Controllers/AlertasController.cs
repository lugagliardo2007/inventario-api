using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlertasController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alerta>>> GetAlertas([FromQuery] bool soloNoLeidas = false)
        {
            var consulta = _context.Alertas.Include(a => a.Producto).AsQueryable();

            if (soloNoLeidas)
                consulta = consulta.Where(a => !a.Leida);

            return await consulta.OrderByDescending(a => a.Fecha).ToListAsync();
        }

        
        [HttpPut("{id}/leer")]
        public async Task<IActionResult> MarcarComoLeida(long id)
        {
            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta == null) return NotFound();

            alerta.Leida = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
