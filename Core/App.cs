using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

//TODO: Los servicios requieren un repositorio? Vea a la linea 58 de esta clase.
//TODO: Realizar un orden y coordinar como se llevara a cabo el main
namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public class App
    {

        /// <summary>
        /// Punto de entrada de la aplicacion.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ModelException"></exception>
        private static void Main(string[] args)
        {
            Console.WriteLine("Iniciando la aplicacion...");
            ISistema sistema = Startup.BuildSistema();
            

            /* Codigo de ejemplo:
            Console.WriteLine("Creating Persona ..");
            {
                Persona persona = new Persona()
                {
                    Rut = "130144918",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga",
                    Email = "durrutia@ucn.cl"
                };

                Console.WriteLine(persona);
                Console.WriteLine(Utils.ToJson(persona));

                // Save in the repository
                sistema.Save(persona);
            }

            Console.WriteLine("Finding personas ..");
            {
                IList<Persona> personas = sistema.GetPersonas();
                Console.WriteLine("Size: " + personas.Count);

                foreach (Persona persona in personas)
                {
                    Console.WriteLine("Persona = " + Utils.ToJson(persona));
                }
            }
            */

            /* Mas codigo de ejemplo:
            //Crear persona:
            Persona persona = new Persona()
            {
                Nombre = "Angel",
                Paterno = "Farias",
                Materno = "Aguila",
                Rut = "142339882",
                Email = "Angel.Farias@gmail.com"
            };

            Persona persona2 = new Persona()
            {
                Nombre = "German",
                Paterno = "Rojo",
                Materno = "Arce",
                Rut = "194460880",
                Email = "garojar97@gmail.com"
            };



            //Crear cliente.

            Cliente cliente = new Cliente()
            {
                Persona = persona,
                Tipo = TipoCliente.UnidadInterna
            };

            //Crear cotizacion.

            Cotizacion cotizacion = new Cotizacion()
            {
                Cliente = cliente,
                Titulo = "Video resumen 10 años Cemp UCN",
                Descripcion = "Grabación, edición y postproducción de video de 3 a 4 minutos " +
                              "sobre actividad de los 10 años del Cemp UCN. El valor incluye 2 revisiones previa entrega, postproducción " +
                              "de imagen y audio, gráficas de presentación inicio y cierre. Los valores por este trabajo " +
                              "son de acuerdo a tarifa especial para unidades pertenecientes a la UCN",
                //fechaCreacion = DateTime.Now,
            };

            //Crear los servicios.

            Servicio servicio1 = new Servicio()
            {
                Descripcion = "Video de 3 a 4 Min",
                Cantidad = 1,
                CostoUnidad = 100000,
                Estado = EstadoServicio.Pausa
            };

            Servicio servicio2 = new Servicio()
            {
                Descripcion = "Animacion 2D de 2 Min",
                Cantidad = 1,
                CostoUnidad = 200000,
                Estado = EstadoServicio.Pausa
            };

            IList<Servicio> servicios = new List<Servicio>();

            Console.WriteLine(cotizacion.Estado);
            
            Console.WriteLine(servicio1.Estado.ToString());
            servicios.Add(servicio1);
            servicios.Add(servicio2);

            //Asignar los servicios a la cotizacion.
            cotizacion.Servicios = servicios;

            //Guardar la cotizacion en la base de datos.
            sistema.GuardarCotizacion(cotizacion);



            //-------------------------------
            //Despliegue:
            //-------------------------------
            {

                IList<Cotizacion> listCotizaciones = sistema.ObtenerCotizaciones();
                foreach (Cotizacion c in listCotizaciones)
                {
                    Console.WriteLine(Utils.ToJson(c));
                    IList<Servicio> serviciosCotizacionAnterior = cotizacion.Servicios;
                    foreach (Servicio s in serviciosCotizacionAnterior)
                    {
                        Console.WriteLine(Utils.ToJson(s));
                        Console.WriteLine(s.Estado);

                    }
                }
            }
            Console.WriteLine("Fin de la aplicacion.");
            */
            
    
            //Creacion de Usuarios de prueba:

            Console.WriteLine("[1] "+EstadoCotizacion.Borrador);

            
            Persona pdirector = new Persona()
            {
                Rut = "19691840K",
                Nombre = "Luis",
                Paterno = "Perez",
                Email = "luis.p@gmail.com"
            };
            
            Usuario director = new Usuario()
            {
                Persona = pdirector,
                Password = "1234",
                Tipo = TipoUsuario.Director
            };
            
            sistema.Anadir(director);
            
            
            
            
            //Login:
            Console.WriteLine("Ingrese su rut o email: ");
            string rutEmail = Console.ReadLine();
            Console.WriteLine("Ingrese su contrasena: ");
            string password = Console.ReadLine();

            Usuario usuario = sistema.Login(rutEmail, password);          
                
            //Menu:
            Console.WriteLine("--------------------------");
            Console.WriteLine("    P r e s U P A    ");
            Console.WriteLine("--------------------------");

            
      

            if (usuario.Tipo == TipoUsuario.Director)
                Consola.MenuDirector(sistema, usuario);
            else if (usuario.Tipo == TipoUsuario.Productor)
                Consola.MenuProductor(sistema, usuario);
            else if (usuario.Tipo == TipoUsuario.Productor)
                Consola.MenuSupervisor(sistema, usuario);
            else
                throw new ModelException("Tipo de usuario no reconocido.");

        }

        

       

        

    }
}
