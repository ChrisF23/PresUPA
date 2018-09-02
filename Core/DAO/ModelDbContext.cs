using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAO
{
    /// <summary>
    /// Contexto de la base de datos.
    /// </summary>
    public sealed class ModelDbContext : DbContext
    {

        /// <inheritdoc />
        public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
        {
            // Asegurar que se elimino y creo la base de datos
            Database.EnsureDeleted();
            Database.EnsureCreated();
            
        }
        
        //TODO: Aqui se deben registrar las entidades restantes del modelo!!!
        
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Registro de las entidades a utilizar.
            modelBuilder.Entity<Persona>();
            modelBuilder.Entity<Usuario>();
            modelBuilder.Entity<Cotizacion>();
        }
    }
}