using System;
using Core;
using Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.Models
{    
    /// <summary>
    /// Testing de la clase Servicio
    /// </summary>
    public class ServicioTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public ServicioTests(ITestOutputHelper output)
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
            Servicio servicio = new Servicio()
            {
            };
            
            //Descripcion es null
            {
                var descripcionThrow = Assert.Throws<ModelException>(() => servicio.Validate());
                Assert.Equal("La descripcion no puede estar vacia.",descripcionThrow.Message);
            }
            servicio.Descripcion = "Descripcion del Test Servicio.....";
            servicio.CostoUnidad = -100;
            
            //Costo es negativo
            {
                var costoThrow = Assert.Throws<ModelException>(() => servicio.Validate());
                Assert.Equal("El costo del servicio no puede ser un valor negativo.",costoThrow.Message);
            }
            servicio.CostoUnidad = 0;
            
            
            //Costo es 0
            {
                var costoThrow = Assert.Throws<ModelException>(() => servicio.Validate());
                Assert.Equal("El costo del servicio no puede ser 0",costoThrow.Message);
            }
            servicio.CostoUnidad = 100;
            servicio.Cantidad = -20;
            
            //Cantidad es negativa
            {
                var costoThrow = Assert.Throws<ModelException>(() => servicio.Validate());
                Assert.Equal("La cantidad del servicio no puede ser negativo.",costoThrow.Message);
            }
            servicio.Cantidad= 0;
            
            //Cantidad es 0
            {
                var costoThrow = Assert.Throws<ModelException>(() => servicio.Validate());
                Assert.Equal("La cantidad del servicio debe ser al menos 1",costoThrow.Message);
            }
            servicio.Cantidad= 100;
            
            _output.WriteLine(Utils.ToJson(servicio));

        }
    }
}