using System;
using System.Collections.Generic;
using Core;
using Core.Controllers;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace TestCore.Controllers
{
    /// <summary>
    /// Test del sistema
    /// </summary>
    public class SistemaTests
    {
        
        /// <summary>
        /// Logger de la clase
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="output"></param>
        public SistemaTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        /// <summary>
        /// Test principal de la clase.
        /// </summary>
        [Fact]
        public void AllMethodsTest()
        {
            _output.WriteLine("Starting Sistema test ...");
            Sistema _sistema = Startup.BuildSistema();

            // Insert null
            {
                Assert.Throws<ModelException>(() => _sistema.Anadir((Persona) null));
            }

            Cliente cliente = new Cliente()
            {
                Persona = new Persona()
                {
                    Rut = "194460880",
                    Email = "garojar@hotmail.com",
                    Nombre = "German",
                    Paterno = "Rojo",
                    Materno = "Arce"
                },
                Tipo = TipoCliente.UnidadInterna


            };





            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Cotizaciones (OS_COXXX)
            //------------------------------------------------------------------------------

            // Añadir cotizacion
            {
                var cotizacionAnadir = Assert.Throws<ModelException>(() => _sistema.Anadir((Cotizacion) null));
                Assert.Equal("La cotizacion es null", cotizacionAnadir.Message);
                _output.WriteLine("Cotizacion añadida es null --> Success");
            }

            //Borrar cotizacion
            {
                var cotizacionBorrar = Assert.Throws<ModelException>(() => _sistema.Borrar(null));
                Assert.Equal("El identificador ingresado fue nulo.", cotizacionBorrar.Message);
                _output.WriteLine("id Cotizacion  es null --> Success");
            }


            //Editar cotizacion
            {
                var cotizacionEditar = Assert.Throws<ModelException>(() => _sistema.Editar((Cotizacion) null));
                Assert.Equal("La cotizacion es null.", cotizacionEditar.Message);
                _output.WriteLine("Cotizacion a editar es null --> Success");
            }

            // Cambiar estado cotizacion
            {
                var cambioEstado =
                    Assert.Throws<ModelException>(() => _sistema.CambiarEstado(null, EstadoCotizacion.Aprobada));
                Assert.Equal("El identificador ingresado fue nulo.", cambioEstado.Message);
                _output.WriteLine("id cotizacion Cambiar estado es null --> Success");
            }

            // Buscar Cotizacion
            {
                var cotizacionBuscar = Assert.Throws<ModelException>(() => _sistema.BuscarCotizacion((string) null));
                Assert.Equal("El identificador ingresado fue nulo.", cotizacionBuscar.Message);
                _output.WriteLine("id Cotizacion en BuscarCotizacion es null --> Success");

                List<Servicio> list = new List<Servicio>();
                list.Add(new Servicio()
                {
                    Cantidad = 2,
                    CostoUnidad = 3000,
                    Descripcion = "test servicio"
                });
                
                _sistema.Anadir(new Cotizacion()
                {
                    Titulo = "Testing",
                    Descripcion = "Cotizacion de testing",
                    Cliente = cliente,
                    CostoTotal = 2000000,
                    Servicios = list
                });
               
                

                cotizacionBuscar = Assert.Throws<ModelException>(() => _sistema.BuscarCotizacion("id"));
                Assert.Equal("No se encontro la cotizacion con el identificador ingresado.", cotizacionBuscar.Message);
                _output.WriteLine("id no existe en el repositorio cotizacion --> Success");

            }
            
        }


    }
}