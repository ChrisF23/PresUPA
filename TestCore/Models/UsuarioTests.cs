using System;
using Core;
using Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.Models
{
    /// <summary>
    /// Test de la clase Usuario
    /// </summary>
    public class UsuarioTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public UsuarioTests(ITestOutputHelper output)
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
            Usuario usuario = new Usuario()
            {
            };
            
            //Persona es null
            {
                var personaThrow = Assert.Throws<ModelException>(() => usuario.Validate());
                Assert.Equal("Se requiere la Persona", personaThrow.Message);
                _output.WriteLine("Persona es null --> Success");
            }
            usuario.Persona = new Persona();
            
            //Password es null
            {
                var personaThrow = Assert.Throws<ModelException>(() => usuario.Validate());
                Assert.Equal("Se requiere la Persona", personaThrow.Message);
                _output.WriteLine("Persona es null --> Success");
            }
            usuario.Password=BCrypt.Net.BCrypt.HashPassword("passwordTest");
            
            //Password se encuentra encriptada
            {
                Assert.True(BCrypt.Net.BCrypt.Verify("passwordTest",usuario.Password));
                    _output.WriteLine("Password bajo BCrypt--> Success");
            }
           
            
        }
    }
}