using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;

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
            DbContextOptions<ModelDbContext> options = new DbContextOptionsBuilder<ModelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            
            DbContext dbContext = new ModelDbContext(options);
            
            IRepository<Persona> personas = new ModelRepository<Persona>(dbContext);
            
            return new Sistema(personas);
        }
        
        /// <summary>
        /// Punto de entrada de la aplicacion.
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
            };
            
            Console.WriteLine(per);
            Console.WriteLine(Utils.ToJson(per));

            // Save in the repository
            sistema.Save(per);

            IList<Persona> personas = sistema.GetPersonas();
            Console.WriteLine("Size: " + personas.Count);
            
            foreach (Persona persona in personas)
            {
                Console.WriteLine("Persona = " + Utils.ToJson(persona));    
            }
            
            Console.WriteLine("Done.");

        }
    }
}