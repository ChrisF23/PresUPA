using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.DAO
{
    /// <summary>
    /// Testing del repositorio de personas
    /// </summary>
    public sealed class PersonaRepositoryTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public PersonaRepositoryTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException("Se requiere la consola");
        }

        /// <summary>
        /// Test de insercion y busqueda en el repositorio.
        /// </summary>
        [Fact]
        public void InsercionBusquedaPersonaTest()
        {
            // Contexto
            DbContext dbContext = BuildTestModelContext();

            // Repositorio de personas
            IRepository<Persona> repo = new ModelRepository<Persona>(dbContext);

            // Creacion
            {
                Persona persona = new Persona()
                {
                    Rut = "130144918",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga",
                    Email = "durrutia@ucn.cl"
                };
                
                // Insert into the backend
                repo.Add(persona);
                
            }
            
            // Busqueda (exitosa)
            {
                Persona persona = repo.GetAll(p => p.Rut.Equals("130144918")).FirstOrDefault();
                Assert.NotNull(persona);
            }
            
            // Busqueda (no exitosa)
            {
                Persona persona = repo.GetAll(p => p.Rut.Equals("132204810")).FirstOrDefault();
                Assert.Null(persona);
            }
            
            // Busqueda (no exitosa)
            {
                Persona persona = repo.GetAll(null).FirstOrDefault();
                Assert.Null(persona);
            }
            
            // Todos
            {
                IList<Persona> personas = repo.GetAll();
                Assert.NotEmpty(personas);
            }
            
            // Busqueda por nombre (exito)
            {
                IList<Persona> personas = repo.GetAll(p => p.Nombre.Equals("Diego"));
                Assert.NotEmpty(personas);
            }

            // Busqueda por nombre (no exito)
            {
                IList<Persona> personas = repo.GetAll(p => p.Nombre.Equals("Francisco"));
                Assert.Empty(personas);
            }
            
            // Busqueda por email
            {
                Assert.NotEmpty(repo.GetAll(p => p.Email.Equals("durrutia@ucn.cl")));
            }
            
            // Busqueda por rut y email
            {
                Assert.NotNull(repo.GetAll(p => p.Rut.Equals("130144918")).FirstOrDefault());   
                Assert.NotNull(repo.GetAll(p => p.Email.Equals("durrutia@ucn.cl")).FirstOrDefault());   
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