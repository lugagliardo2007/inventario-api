using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Alertas")]
    public class Alerta
    {
        [Key]
        public long Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [StringLength(500)]
        public string Mensaje { get; set; } = string.Empty;

        public bool Leida { get; set; }

        public long ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
