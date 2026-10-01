using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Proveedor> Proveedores { get; set; } = null!;
        public DbSet<MovimientoStock> Movimientos { get; set; } = null!;
        public DbSet<Alerta> Alertas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Producto>().HasIndex(p => p.Sku).IsUnique();
            modelBuilder.Entity<Proveedor>().HasIndex(p => p.Cuit).IsUnique();

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria).WithMany()
                .HasForeignKey(p => p.CategoriaId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor).WithMany()
                .HasForeignKey(p => p.ProveedorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoStock>()
                .HasOne(m => m.Producto).WithMany()
                .HasForeignKey(m => m.ProductoId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Alerta>()
                .HasOne(a => a.Producto).WithMany()
                .HasForeignKey(a => a.ProductoId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
