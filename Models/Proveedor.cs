using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Cuit { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Telefono { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }
    }
}
