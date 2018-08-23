using System;
using Core.Models;
using Xunit;

namespace Core.UniTest.Models
{
    /// <summary>
    /// Testing de la clase Persona.
    /// </summary>
    public class PersonaTest
    {
        /// <summary>
        /// Test del constructor
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            Console.WriteLine("Creating Persona ..");
            Persona persona = new Persona()
            {
                Rut = "13014491-8",
                Nombre = "Diego",
                Paterno = "Urrutia",
                Materno = "Astorga"
            };

            Console.WriteLine(persona);
        }
    }
}