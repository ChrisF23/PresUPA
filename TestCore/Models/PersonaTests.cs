using System;
using Core;
using Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.Models
{
    /// <summary>
    /// Testing de la clase Persona.
    /// </summary>
    public class PersonaTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public PersonaTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        /// <summary>
        /// Test del constructor
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            _output.WriteLine("Creating Persona ..");
            Persona persona = new Persona()
            {
            };
            
            // Rut null
            {
                var rutThrow = Assert.Throws<ModelException>(() => persona.Validate());
                Assert.Equal("Rut no puede ser null.", rutThrow.Message);
                _output.WriteLine("Rut no puede ser null ---> Success");
            }
            persona.Rut = "194349001";
            
            // Rut no valido
            {
                var rutThrow = Assert.Throws<ModelException>(() => persona.Validate());
                Assert.Equal("Rut no valido", rutThrow.Message);
                _output.WriteLine("Rut no valido ---> Success");
            }
            persona.Rut = "194460880";
            
            // Nombre null
            {
                var nombreThrow = Assert.Throws<ModelException>(() => persona.Validate());
                Assert.Equal("Nombre no puede ser null o vacio.", nombreThrow.Message);
                _output.WriteLine("Nombre null --> Success");
            }
            persona.Nombre = "German";
            
            
            // Paterno null
            {
                var paternoThrow = Assert.Throws<ModelException>(() => persona.Validate());
                Assert.Equal("Apellido Paterno no puede ser null o vacio.", paternoThrow.Message);
                _output.WriteLine("Paterno null --> Success");
            }
            persona.Paterno = "Rojo";
            
            //Email null
            {
                var emailThrow = Assert.Throws<ModelException>(() => persona.Validate());
                Assert.Equal("Email no puede ser null o vacio.", emailThrow.Message);
                _output.WriteLine("");
            }
            persona.Email = "correo.test.unitario.format.cl";
            
            //Email no valido
            {
                var rutThrow = Assert.Throws<ModelException>(() => persona.Validate());
                Assert.Equal("El email tiene un formato invalido", rutThrow.Message);
                _output.WriteLine("");
            }
            persona.Email = "garojar@hotmail.com";
            
            _output.WriteLine(Utils.ToJson(persona));
        }
    }
}