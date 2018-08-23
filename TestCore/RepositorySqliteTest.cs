using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Core.DAO;
using Core.Models;

namespace Core.UniTest.DAO
{
    /// <summary>
    /// Testing del repositorio
    /// </summary>
    public sealed class RepositorySqliteTest
    {
        /// <summary>
        /// Testing del constructor y de cada una de las clases.
        /// </summary>
        [Fact]
        public void WholeTest()
        {
            // Configuration de la base de datos (SQLite)
            DbContextOptions<SqliteDbContext> options = new DbContextOptionsBuilder<SqliteDbContext>()
                //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            // El contexto por defecto utiliza sqlite.
            SqliteDbContext sqliteDbContext = new SqliteDbContext(options);
            
            // Repositorio de prueba
            IRepository<Entidad> repository = new RepositorySqlite<Entidad>(sqliteDbContext);
            
            // Inicializacion del repositorio
            repository.Initialize();

            // Entidad de prueba
            Entidad entidad = new Entidad()
            {
                Cortito = 12,
                Largo = 120000000,
                Valor = 123000,
                FechaDeAniversario = DateTime.Now,
                NombreDeLaEntidad = "Este es mi nombre"
            };
            
            // Almaceno en el repositorio
            repository.Add(entidad);

            // Busqueda de las entidades
            IList<Entidad> entidades = repository.GetAll();
            
            // DEBE existir el elemento que se ingreso.
            Assert.Equal(1, entidades.Count);

            // Ciclo para obtener todos los elementos en el repositorio
            foreach (Entidad entidad1 in entidades)
            {
                Console.WriteLine(App.ToJson(entidad));
            }

        }
    }

    /// <summary>
    /// Entidad de testing
    /// </summary>
    public sealed class Entidad : IModel
    {
        public int Valor { get; set; }
        
        public long Largo { get; set; }
        
        public short Cortito { get; set; }
        
        public string NombreDeLaEntidad { get; set; }
        
        public DateTime FechaDeAniversario { get; set; }
        
        public void Validate()
        {
            // Nothing here
        }
    }
}