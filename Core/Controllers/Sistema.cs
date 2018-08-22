using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class Sistema : ISistema
    {
        // Repositorio de personas.
        private readonly IRepository<Persona> _repositoryPersona;

        /// <summary>
        /// Constructor del sistema
        /// </summary>
        public Sistema()
        {
            // Repositorio
            _repositoryPersona = new Repository<Persona>(BuildDbContext());

            // Creacion de la base de datos.
            _repositoryPersona.Initialize();
        }

        private DbContext BuildDbContext()
        {
            // Base de datos en memoria
            var options = new DbContextOptionsBuilder<SqliteContext>()
                //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            
            return new SqliteContext(options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persona"></param>
        public void Save(Persona persona)
        {
            if (persona == null)
            {
                throw new ModelException("Persona es null");
            }

            // Saving the Persona
            _repositoryPersona.Add(persona);
        }
        
        /// <summary>
        /// InMemory
        /// </summary>
        class SqliteContext : DbContext
        {
            public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(@"Data Source=database.db");
            }
        }
    }
}