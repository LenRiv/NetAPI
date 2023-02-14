using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net_API.Modelos;

namespace Net_API.Datos
{
    public class AppDbContext : DbContext
    {
        //Configuracion EntityFrameWork y DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
                
        }

        public DbSet<Villa> Villas { get; set; }


        //Creamos datos en la tabla por defecto
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {

            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la Villa...",
                    ImagenUrl = "",
                    Inquilinos = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 200,
                    Encanto = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,

                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Premium Vistas Piscina",
                    Detalle = "Detalle de la Villa...",
                    ImagenUrl = "",
                    Inquilinos = 4,
                    MetrosCuadrados = 80,
                    Tarifa = 400,
                    Encanto = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }

                );
        
                


        }

    }
}
