using System;
using System.Collections.Generic;
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
    public class App
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
        public static string ToJson<T>(T obj)
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
            Persona per = new Persona()
            {
                Rut = "13014491-8",
                Nombre = "Diego",
                Paterno = "Urrutia",
                Materno = "Astorga",
                DateTime = DateTime.Now
            };
            
            Console.WriteLine(per);
            Console.WriteLine(ToJson(per));

            // Save in the repository
            sistema.Save(per);

            IList<Persona> personas = sistema.GetPersonas();
            Console.WriteLine("Size: " + personas.Count);
            
            foreach (Persona persona in personas)
            {
                Console.WriteLine("Persona = " + ToJson(persona));    
            }
            
            Console.WriteLine("Done.");

        }
    }
}