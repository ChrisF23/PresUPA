using System;
using Core;
using Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.Models
{
    /// <summary>
    /// Testing de la clase Cliente
    /// </summary>
    public class ClienteTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public ClienteTests(ITestOutputHelper output)
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
            Cliente cliente = new Cliente()
            {
            };

            //Persona es null
            {
                var personaThrow = Assert.Throws<ModelException>(() => cliente.Validate());
                Assert.Equal("Se requiere la Persona", personaThrow.Message);
                _output.WriteLine("Persona es null --> Success");
            }
            cliente.Persona = new Persona();
            
            _output.WriteLine(Utils.ToJson(cliente));

        }
    }
}