//using Microsoft.EntityFrameworkCore;
//using ExtranetQz.Areas.Kardex.Models;

//namespace ExtranetQz.Areas.Kardex.Data
//{
//    public class KardexDbContext : DbContext
//    {
//        public KardexDbContext(DbContextOptions<KardexDbContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<Movimiento> Movimientos { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Movimiento>().ToTable("movimientos");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Clase_Movimiento).HasColumnName("clase_movimiento");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Codigo_Material).HasColumnName("codigo_material");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Descripcion).HasColumnName("descripcion");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Cantidad).HasColumnName("cantidad");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Lote).HasColumnName("lote");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Fecha_Entrada).HasColumnName("fecha_entrada");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Costo).HasColumnName("costo");
//            modelBuilder.Entity<Movimiento>().Property(m => m.Importe).HasColumnName("importe");
//        }
//    }
//}
