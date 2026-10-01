using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MovimientosStock")]
    public class MovimientoStock
    {
        [Key]
        public long Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public TipoMovimiento Tipo { get; set; }

        public int Cantidad { get; set; }

        [StringLength(200)]
        public string? Motivo { get; set; }

        public long ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
