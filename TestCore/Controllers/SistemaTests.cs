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

            Persona p = new Persona()
            {
                Rut = "194460880",
                Email = "garojar@hotmail.com",
                Nombre = "German",
                Paterno = "Rojo",
                Materno = "Arce"
            };
            
            Cliente cliente = new Cliente()
            {
                Persona = p,
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
                var cambioEstado =
                    Assert.Throws<ModelException>(() => _sistema.CambiarEstado(null, EstadoCotizacion.Aprobada));
                Assert.Equal("El identificador ingresado fue nulo.", cambioEstado.Message);
                _output.WriteLine("id cotizacion Cambiar estado es null --> Success");

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
            
            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Usuario (OS_USXXX)
            //------------------------------------------------------------------------------
            
            // Añadir Usuario
            {
                var usuarioAnadir = 
                Assert.Throws<ArgumentNullException>(() => _sistema.Anadir((Persona) null, (string) null));
                Assert.Equal("La persona no debe ser null", usuarioAnadir.Message);
                _output.WriteLine("Persona del usuario es null --> Success");
                
                usuarioAnadir =
                Assert.Throws<ArgumentNullException>(() => _sistema.Anadir(p, (string) null));
                Assert.Equal("La password no debe ser null", usuarioAnadir.Message);
                _output.WriteLine("Password del usuario es null --> Success");
                
                
            }
            
            
            //Login Usuario
            {
                
                var loginUsuario =
                    Assert.Throws<ModelException>(() => _sistema.Login(null,null));
                Assert.Equal("Password no puede ser null", loginUsuario.Message);
                _output.WriteLine("Password es nulo en login --> Success");
                
                loginUsuario =
                Assert.Throws<ModelException>(() => _sistema.Login("100875713","pw123"));
                Assert.Equal("Usuario no encontrado", loginUsuario.Message);
                _output.WriteLine("Usuario no encontrado login --> Success");
                
            }


            //Anadir Usuario
            {
                var usuarioAnadir =
                Assert.Throws<ModelException>(() => _sistema.Anadir((Persona)null));
                Assert.Equal("Persona es null", usuarioAnadir.Message);
                _output.WriteLine("Persona es null en anadir usuario --> Success");
 
            }
            
            //Buscar Usuario
            {
                var usuarioBuscar =
                Assert.Throws<ModelException>(() => _sistema.BuscarPersona((string)null));
                Assert.Equal("El rut o email ingresado fue nulo.", usuarioBuscar.Message);
                _output.WriteLine("Rut ingresado para buscar usuario es null --> Success");
 
            }
            
            
            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Servicio (OS_SEXXX)
            //------------------------------------------------------------------------------
            
          
            
            
        }


    }
}