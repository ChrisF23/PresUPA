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
                _output.WriteLine("Identificador es null --> Success");
            }
            cotizacion.Identificador = "v2.1";
            
            //Numero es null
            {
                var numThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("El numero no puede ser null.", numThrow.Message);
                _output.WriteLine("Numero es null --> Success");
            }
            cotizacion.Numero= 1;
            
            
            //Version es null
            {
                var versionThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La version no puede ser null.", versionThrow.Message);
                _output.WriteLine("Version es null --> Success");
            }
            cotizacion.Version= 2;
            
  
            //Titulo es null
            {
                var tituloThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("El titulo no puede estar vacio.", tituloThrow.Message);
                _output.WriteLine("Titulo es null --> Success");
            }
            cotizacion.Titulo = "Titulo CotizacionTest";
            
            //Descripcion es null
            {
                var descripcionThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La descripcion no puede estar vacia.", descripcionThrow.Message);
                _output.WriteLine("Descripcion es null --> Success");
            }
            cotizacion.Descripcion = "Descripcion CotizacionTest .....";

            //Cliente es null
            {
                var clienteThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no tiene un cliente asignado.", clienteThrow.Message);
                _output.WriteLine("Cliente es null --> Success");
            }
            //No es responsabilidad de este UnitTest de comprobar la integridad
            // y funcionalidad de Cliente , por lo tanto se deja instanciado
            // con sus atributos en null
            cotizacion.Cliente =  new Cliente();
            
            // IList Servicios es null
            {
                var serviciosThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no tiene servicios asignados.", serviciosThrow.Message);
                _output.WriteLine("List<Servicios> es null --> Success");
            }
            cotizacion.Servicios = new List<Servicio>();
            
            // Costo Total es 0
            {
                var costoTotalThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no puede tener un costo total de $0.", costoTotalThrow.Message);
                _output.WriteLine("CostoTotal es 0 --> Success");
            }
            cotizacion.CostoTotal = -20000;
            
            
            // Costo Total es negativo
            {
                var costoTotalThrow = Assert.Throws<ModelException>(() => cotizacion.Validate());
                Assert.Equal("La cotizacion no puede tener un costo negativo", costoTotalThrow.Message);
                _output.WriteLine("CostoTotal es negativo --> Success");
            }
            cotizacion.CostoTotal = 20000;
            _output.WriteLine(Utils.ToJson(cotizacion));


        }



        
    }
}