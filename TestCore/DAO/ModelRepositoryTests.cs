using System;
using System.Collections.Generic;
using System.Threading;
using Core;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.DAO
{
    /// <summary>
    /// Test del repositorio EF
    /// </summary>
    public class ModelRepositoryTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public ModelRepositoryTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException("Se requiere la consola");
        }

        /// <summary>
        /// Prueba de la clase completa.
        /// </summary>
        [Fact]
        public void InsercionBusquedaEntidadTest()
        {
            _output.WriteLine("Starting test ..");
            
            DbContext dbContext = BuildTestModelContext();
            IRepository<Entidad> repo = new ModelRepository<Entidad>(dbContext);

            // Test add
            {            
                // Entidad de prueba
                Entidad entidad = new Entidad()
                {
                    Cortito = 12,
                    Largo = 120000000,
                    Valor = 123000,
                    FechaDeAniversario = DateTime.Now,
                    NombreDeLaEntidad = "Este es mi nombre"
                };
                
                // Debug to output
                _output.WriteLine(Utils.ToJson(entidad));
    
                // El id tiene que ser 0
                Assert.True(entidad.Id == 0, "Id es != de 0");
    
                // Agrego la entidad
                repo.Add(entidad);
    
                // El id tiene que ser 1 (autoincrease)
                Assert.True(entidad.Id == 1, "Id es != 1");
            }

            // Busqueda de las entidades
            {
                IList<Entidad> entidades = repo.GetAll();

                // DEBE existir el elemento que se ingreso.
                Assert.Equal(1, entidades.Count);

                // El id de la entidad en la base de datos debe ser igual a 1
                Assert.True(entidades[0].Id == 1, "Id es != 1");

                // Ciclo para obtener todos los elementos en el repositorio
                foreach (Entidad e in entidades)
                {
                    Console.WriteLine(Utils.ToJson(e));
                }
                
            }

            // Get by id (found)
            {
                Assert.NotNull(repo.GetById(1));
            }
            
            // Get by id (not found)
            {
                Entidad entidad = repo.GetById(-1);
                Assert.Null(entidad);
            }
            
            // GetAll with expression
            {
                IList<Entidad> entidades = repo.GetAll(e => e.Cortito == 12);
                
                Assert.Equal(1, entidades.Count);
            }
            
            // Remove
            {
                Entidad entidad = repo.GetById(1);
                repo.Remove(entidad);
                Assert.Null(repo.GetById(1));
            }
            
            _output.WriteLine("Test ended!");
            
        }

        /// <summary>
        /// Construye el contexto de prueba.
        /// </summary>
        /// <returns></returns>
        private static DbContext BuildTestModelContext()
        {
            DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
                // .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // RAM Database
                .UseSqlite(@"Data Source=test.db") // SQLite
                .EnableSensitiveDataLogging()
                .Options;
            
            return new TestDbContext(options);
        }
        
    }
    
    /// <summary>
    /// Entidad de testing
    /// </summary>
    public sealed class Entidad : BaseEntity
    {
        public int Valor { get; set; }
        
        public long Largo { get; set; }
        
        public short Cortito { get; set; }
        
        public string NombreDeLaEntidad { get; set; }
        
        public DateTime FechaDeAniversario { get; set; }

        public override void Validate()
        {
            // Empty
        }
    }
    
    /// <summary>
    /// Contexto de la base de datos de prueba.
    /// </summary>
    public sealed class TestDbContext : DbContext
    {

        /// <inheritdoc />
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
            // Asegurar que se elimino y creo la base de datos
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Registro de las entidades a utilizar.
            modelBuilder.Entity<Entidad>();
        }
    }
    
}