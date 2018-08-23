using System;
using Core.Controllers;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Inicializa y construye el sistema.
        /// </summary>
        /// <returns></returns>
        private static Sistema BuildSistema()
        {
            // Configuration de la base de datos (SQLite)
            DbContextOptions<SqliteDbContext> options = new DbContextOptionsBuilder<SqliteDbContext>()
                //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            // El contexto por defecto utiliza sqlite.
            SqliteDbContext sqliteDbContext = new SqliteDbContext(options);
            
            // Repositorio de personas
            IRepository<Persona> personas = new RepositorySqlite<Persona>(sqliteDbContext);
            
            // Sistema
            return new Sistema(personas);
            
        }

        /// <summary>
        /// Imprime un objeto en formato json.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string ToJson<T>(T obj)
        { 
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ModelException"></exception>
        private static void Main(string[] args)
        {
            Console.WriteLine("Building Sistema ..");
            ISistema sistema = BuildSistema();

            Console.WriteLine("Creating Persona ..");
            Persona persona = new Persona()
            {
                Rut = "13014491-8",
                Nombre = "Diego",
                Paterno = "Urrutia",
                Materno = "Astorga"
            };
            
            Console.WriteLine(persona);
            Console.WriteLine(ToJson(persona));

            // Save in the repository
            sistema.Save(persona);

        }
    }
}