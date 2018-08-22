using System;
using Core.Controllers;
using Core.Models;
using Newtonsoft.Json;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {

        /// <summary>
        /// 
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
            Console.WriteLine("Starting ..");

            Console.WriteLine("Creating Persona ..");
            Persona persona = new Persona()
            {
                Rut = "13014491-8",
                Nombre = "Diego",
                Paterno = "Urrutia",
                Materno = "Astorga"
            };
            Console.WriteLine("Validating ..");
            persona.Validate();
            
            Console.WriteLine(persona);
            Console.WriteLine(ToJson(persona));
            
            ISistema sistema = new Sistema();
            sistema.Save(persona);

            ModelException modelException = new ModelException("Error en el modelo");
            // throw modelException;
        }
    }
}