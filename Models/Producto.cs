using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Range(0, 1000000000, ErrorMessage = "El precio no puede ser negativo.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock actual no puede ser negativo.")]
        public int StockActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo.")]
        public int StockMinimo { get; set; }

        public bool Activo { get; set; } = true;


        public long CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public long ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }


        public bool HayStockSuficiente(int cantidad) => StockActual >= cantidad;
        public bool RequiereAlerta() => StockActual <= StockMinimo;
    }
}
