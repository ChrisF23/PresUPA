using System;
using System.Collections.Generic;
using Core;
using Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.Models
{
    /// <summary>
    /// Testing de la clase Cotizacion
    /// </summary>
    public class CotizacionTests
    {
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public CotizacionTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }


        /// <summary>
        /// Test del constructor
        /// OJO: No se hace testing del estado, al ser un Enumm
        /// automaticamente tomara el primer valor
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            _output.WriteLine("Creating Persona ..");
            Cotizacion cotizacion = new Cotizacion()
            {
            };
            /// OJO: Assert.Equal(EXPECTED, ACTUAL);
            
            //Identificador es null
            {
                var idThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("El identificador no puede ser null.", idThrow.Message);
            }
            cotizacion.Identificador = "v2.1";
            
            //Numero es null
            {
                var numThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("El numero no puede ser null.", numThrow.Message);
            }
            cotizacion.Numero= 1;
            
            
            //Version es null
            {
                var versionThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La version no puede ser null.", versionThrow.Message);
            }
            cotizacion.Version= 2;
            
  
            //Titulo es null
            {
                var tituloThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("El titulo no puede estar vacio.", tituloThrow.Message);
            }
            cotizacion.Titulo = "Titulo CotizacionTest";
            
            //Descripcion es null
            {
                var descripcionThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La descripcion no puede estar vacia.", descripcionThrow.Message);
            }
            cotizacion.Descripcion = "Descripcion CotizacionTest .....";

            //Cliente es null
            {
                var clienteThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no tiene un cliente asignado.", clienteThrow.Message);
            }
            //No es responsabilidad de este UnitTest de comprobar la integridad
            // y funcionalidad de Cliente , por lo tanto se deja instanciado
            // con sus atributos en null
            cotizacion.Cliente =  new Cliente();
            
            // IList Servicios es null
            {
                var serviciosThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no tiene servicios asignados.", serviciosThrow.Message);
            }
            cotizacion.Servicios = new List<Servicio>();
            
            // Costo Total es 0
            {
                var costoTotalThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no puede tener un costo total de $0.", costoTotalThrow.Message);
            }
            cotizacion.CostoTotal = -20000;
            
            
            // Costo Total es negativo
            {
                var costoTotalThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no puede tener un costo negativo", costoTotalThrow.Message);
            }
            cotizacion.CostoTotal = 20000;


        }



        
    }
}