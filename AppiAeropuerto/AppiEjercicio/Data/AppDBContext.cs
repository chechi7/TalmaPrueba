using Microsoft.EntityFrameworkCore;
using AppiEjercicio.Models;

namespace AppiEjercicio.Data
{
        public class AppDBContext : DbContext
        {
            public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
            {

            }

        public DbSet<Vuelos> Vuelos { get; set; }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vuelos>(tb =>
            {
                tb.HasKey(col => col.IdVuelo);
                tb.Property(col => col.IdVuelo)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(col => col.CiudadOrigen).HasMaxLength(50);
                tb.Property(col => col.CiudadDestino).HasMaxLength(50);
                tb.Property(col => col.Aerolinea).HasMaxLength(50);
                tb.Property(col => col.NumeroVuelo).HasMaxLength(50);

            });

            modelBuilder.Entity<Vuelos>().ToTable("InfoVuelos");
        }
    }
}
