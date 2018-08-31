using System;
using Core.Controllers;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    /// <summary>
    /// Clase de inicializacion del sistema.
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// Inicializa y construye el sistema.
        /// </summary>
        /// <returns></returns>
        public static Sistema BuildSistema()
        {
            DbContextOptions<ModelDbContext> options = new DbContextOptionsBuilder<ModelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Memory
                // .UseSqlite(@"Data Source=database.db") // SQLite
                .EnableSensitiveDataLogging()
                .Options;
            
            DbContext dbContext = new ModelDbContext(options);
            
            IRepository<Persona> personas = new ModelRepository<Persona>(dbContext);
            IRepository<Usuario> usuarios = new ModelRepository<Usuario>(dbContext);
            
            return new Sistema(personas, usuarios);
        }
    }
}