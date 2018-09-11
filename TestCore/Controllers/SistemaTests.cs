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
             _output.WriteLine("TEST COTIZACIONES \n--------------------- ");
            
            /**
             * UnitTest Añadir Cotizacion
             */
            {
                var cotizacionAnadir = Assert.Throws<ModelException>(() => _sistema.Anadir((Cotizacion) null));
                Assert.Equal("La cotizacion es null", cotizacionAnadir.Message);
                _output.WriteLine("Cotizacion añadida es null --> Success");
            }

            /**
             * UnitTest Borrar Cotizacion
             */
            {
                var cotizacionBorrar = Assert.Throws<ModelException>(() => _sistema.Borrar(null));
                Assert.Equal("El identificador ingresado fue nulo.", cotizacionBorrar.Message);
                _output.WriteLine("id Cotizacion  es null --> Success");
            }


            /**
             * UniTest Editar Cotizacion
             */
            {
                var cotizacionEditar = Assert.Throws<ModelException>(() => _sistema.Editar((Cotizacion) null));
                Assert.Equal("La cotizacion es null.", cotizacionEditar.Message);
                _output.WriteLine("Cotizacion a editar es null --> Success");
            }

            List<Servicio> list = new List<Servicio>();
            list.Add(new Servicio()
            {
                Cantidad = 2,
                CostoUnidad = 3000,
                Descripcion = "test servicio"
            });
            
            /**
             * UnitTest BuscarCotizacion
             */
            {
                //id Cotizacion es null
                var cotizacionBuscar = 
                Assert.Throws<ModelException>(() => _sistema.BuscarCotizacion((string) null));
                Assert.Equal("El identificador ingresado fue nulo.", cotizacionBuscar.Message);
                _output.WriteLine("id Cotizacion en BuscarCotizacion es null --> Success");
                
                
                //Buscar repositorio vacio
                var cotizacionBuscarReferenceException =
                Assert.Throws<NullReferenceException>(() => _sistema.BuscarCotizacion("idtest"));
                Assert.Equal("Repositorio de Cotizaciones se encuentra vacio", cotizacionBuscarReferenceException.Message);
                _output.WriteLine("Repositorio cotizacion vacio --> Success");
                
                
                
                _sistema.Anadir(new Cotizacion()
                {
                    Titulo = "Testing",
                    Descripcion = "Cotizacion de testing",
                    Cliente = cliente,
                    CostoTotal = 2000000,
                    Servicios = list
                });
               
                
                // No se encuentra una cotizacion con esa id asociada
                cotizacionBuscar = Assert.Throws<ModelException>(() => _sistema.BuscarCotizacion("id"));
                Assert.Equal("No se encontro la cotizacion con el identificador ingresado.", cotizacionBuscar.Message);
                _output.WriteLine("id no existe en el repositorio cotizacion --> Success");
                
                // Se encuentra una cotizacion
                Assert.NotNull(_sistema.BuscarCotizacion("301v1"));
                _output.WriteLine("id no existe en el repositorio cotizacion --> Success");
                
                

            }
            
            
            /**
             * UnitTest CambiarEstadoCotizacion
             */
          {
                //id Cotizacion es null
                var cambioEstado =
                Assert.Throws<ModelException>(() => _sistema.CambiarEstado((string) null, EstadoCotizacion.Aprobada));
                Assert.Equal("El identificador ingresado fue nulo.", cambioEstado.Message);
                _output.WriteLine("id cotizacion Cambiar estado es null --> Success");
                
                //id cotizacion no existe en el repositorio
                cambioEstado =
                Assert.Throws<ModelException>(() => _sistema.CambiarEstado("id1test", EstadoCotizacion.Aprobada));
                Assert.Equal("No se encontro la cotizacion con el identificador ingresado.", cambioEstado.Message);
                _output.WriteLine("id no existe en el repositorio cotizacion  --> Success");
                
                // Tratar de cambiar cotizacion a un estado invalido
                cambioEstado =
                Assert.Throws<ModelException>(() => _sistema.CambiarEstado("301v1", EstadoCotizacion.Aprobada));
                Assert.Equal("Esta cotizacion solo puede ser enviada o rechazada!", cambioEstado.Message);
                _output.WriteLine("Estado cotizacion Borrador  limitado a enviada o rechazada  --> Success");
            }


          
            
            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Usuario (OS_USXXX)
            //------------------------------------------------------------------------------
            _output.WriteLine("TEST USUARIO \n--------------------- ");
            /*
             * UnitTest Añadir Usuario
             */
            {
                // Contraseña es null
                var usuarioAnadirParam =
                Assert.Throws<ModelException>(() => _sistema.Anadir(p, (string) null));
                Assert.Equal("La password no puede ser null.", usuarioAnadirParam.Message);
                _output.WriteLine("Password del usuario es null --> Success");
                
                // Persona es null     
                var usuarioAnadir = 
                Assert.Throws<ModelException>(() => _sistema.Anadir(null, "pwtest"));
                Assert.Equal("Persona es null", usuarioAnadir.Message);
                _output.WriteLine("Persona del usuario es null --> Success");
            }
            
            
            /*
             * UnitTest LoginUsuario
             */
            {
                // Contraseña null
                var loginUsuario =
                    Assert.Throws<ModelException>(() => _sistema.Login(null,null));
                Assert.Equal("Password no puede ser null", loginUsuario.Message);
                _output.WriteLine("Password es nulo en login --> Success");
                
                // Usuario no existe en el repositorio
                loginUsuario =
                Assert.Throws<ModelException>(() => _sistema.Login("100875713","pw123"));
                Assert.Equal("Usuario no encontrado", loginUsuario.Message);
                _output.WriteLine("Usuario no encontrado login --> Success");
                
                _sistema.Anadir(p);
                
                //Persona sin credenciales (no tiene instancia como usuario)
                loginUsuario =
                Assert.Throws<ModelException>(() => _sistema.Login("194460880","pwtest"));
                Assert.Equal("Existe la Persona pero no tiene credenciales de acceso", loginUsuario.Message);
                _output.WriteLine("Persona sin credenciales --> Success");
                
               //Add usuario al repositorio para la siguiente unitTest
                _sistema.Anadir(p,"1234");
                
                // Login con contraseña incorrecta
                loginUsuario =
                Assert.Throws<ModelException>(() => _sistema.Login("194460880","pwtest"));
                Assert.Equal("Contrasena incorrecta!", loginUsuario.Message);
                _output.WriteLine("Login contraseña incorrecta --> Success");
                
                //Se hace un login exitoso, asegurando que se retornara un usuario
                Assert.NotNull(_sistema.Login("194460880","1234"));
                _output.WriteLine("Login Existoso --> Success");
                
            }

            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Persona (OS_PEXXX)
            //------------------------------------------------------------------------------
            _output.WriteLine("TEST PERSONA \n--------------------- ");
            
            /*
             * UnitTest Añadir Persona
             */
            {
                //Añadir persona null
                var personaAnadir =
                Assert.Throws<ModelException>(() => _sistema.Anadir((Persona)null));
                Assert.Equal("Persona es null", personaAnadir.Message);
                _output.WriteLine("Persona es null al agregar al repositorio --> Success");
            }
            
            /*
             * UnitTest BuscarPersona
             */
            {
                // rutEmail es null
                var personaBuscar =
                Assert.Throws<ModelException>(() => _sistema.BuscarPersona((string)null));
                Assert.Equal("El rut o email ingresado fue nulo.", personaBuscar.Message);
                _output.WriteLine("Rut ingresado para buscar persona es null --> Success");
                
              
                // se busca la persona asegurando exito de busqueda
                Assert.NotNull(_sistema.BuscarPersona("194460880"));
                _output.WriteLine("Persona encontrada con exito--> Success");
          
            }
            
            
            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Servicio (OS_SEXXX)
            //------------------------------------------------------------------------------
            _output.WriteLine("TEST SERVICIOS \n--------------------- ");
            Servicio service = new Servicio();
           
            
            /*
             * UnitTest Añadir Servicio
             */
            {
                //Añadir servicio nulo
                var servicioAnadir =
                Assert.Throws<ModelException>(() => _sistema.Anadir((Servicio)null,(Cotizacion)null));
                Assert.Equal("El servicio ingresado fue nulo.", servicioAnadir.Message);
                _output.WriteLine("Añadir servicio nulo --> Success");
                
                //Añadir servicio a una cotizacion nula
                servicioAnadir =
                Assert.Throws<ModelException>(() => _sistema.Anadir(service,(Cotizacion)null));
                Assert.Equal("La cotizacion ingresada fue nula.", servicioAnadir.Message);
                _output.WriteLine("Añadir servicio a cotizacion nula --> Success");
                   
            }
            
            /*
             * UnitTest Cambiar Estado Servicio
             */
            {
                var personaAnadir =
                Assert.Throws<ModelException>(() => _sistema.Anadir((Persona)null));
                Assert.Equal("Persona es null", personaAnadir.Message);
                _output.WriteLine("Persona es null al agregar al repositorio --> Success");
            }
            
            
            //------------------------------------------------------------------------------
            //    Operaciones de Sistema: Cliente (OS_CLXXX)
            //------------------------------------------------------------------------------
            _output.WriteLine("TEST CLIENTE \n--------------------- ");
            
            /*
             * UnitTest añadir Cliente
             */
            {
                //Persona es nula al agregar como cliente
                var clienteAnadir =
                Assert.Throws<ModelException>(() => _sistema.Anadir((Persona) null,TipoCliente.Otro));
                Assert.Equal("Persona es null", clienteAnadir.Message);
                _output.WriteLine("Persona es null al añadir como cliente --> Success");
                
            }
           
            /**
             * UnitTest BuscarCliente
             */
            {
                //Persona es nula al agregar como cliente
                var clienteBuscar =
                    Assert.Throws<ModelException>(() => _sistema.BuscarCliente(null));
                Assert.Equal("El rut o email ingresado fue nulo.", clienteBuscar.Message);
                _output.WriteLine("Rut null en buscarCliente --> Success");

                //Persona retorna un cliente, asegurando exito de busqueda
                Assert.NotNull(_sistema.BuscarCliente("194460880"));
                _output.WriteLine("Busqueda exitosa BuscarCliente --> Success");
            }

        }


    }
}