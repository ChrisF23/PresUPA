using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestCore.DAO
{
    /// <summary>
    /// Testing del repositorio de personas
    /// </summary>
    public sealed class TestPersonaRepository
    {
        [Fact]
        public void PersonaSavingTest()
        {
            // Contexto
            DbContext dbContext = BuildTestModelContext();

            // Repositorio de personas
            PersonaRepository repo = new PersonaRepository(dbContext);

            // Creacion
            {
                Persona persona = new Persona()
                {
                    Rut = "13014491-8",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga"
                };
                
                // Insert into the backend
                repo.Add(persona);
                
            }
            
            // Busqueda (exitosa)
            {
                Persona persona = repo.GetByRut("13014491-8");
                Assert.NotNull(persona);
            }
            
            // Busqueda (no exitosa)
            {
                Persona persona = repo.GetByRut("1-1");
                Assert.Null(persona);
            }
            
            // Todos
            {
                IList<Persona> personas = repo.GetAll();
                Assert.NotEmpty(personas);
            }
            
            // Busqueda por nombre
            {
                IList<Persona> personas = repo.GetAll(p => p.Nombre.Equals("Diego"));
                Assert.NotEmpty(personas);
            }

            // Busqueda por nombre
            {
                IList<Persona> personas = repo.GetAll(p => p.Nombre.Equals("Francisco"));
                Assert.Empty(personas);
            }
            
            // Eliminacion
            {
                Persona persona = repo.GetById(1);
                Assert.NotNull(persona);
                
                repo.Remove(persona);                
            }
            
            // Busqueda no exitosa
            {
                Persona persona = repo.GetById(1);
                Assert.Null(persona);
            }
            
        
        }

        /// <summary>
        /// Construccion del DbContext de prueba
        /// </summary>
        /// <returns></returns>
        private static DbContext BuildTestModelContext()
        {
            DbContextOptions<ModelDbContext> options = new DbContextOptionsBuilder<ModelDbContext>()
                // .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseSqlite(@"Data Source=personas.db") // SQLite
                .EnableSensitiveDataLogging()
                .Options;
            
            return new ModelDbContext(options);
        }
    }
}