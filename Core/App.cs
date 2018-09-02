using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;

//TODO: Los servicios requieren un repositorio? Vea a la linea 58 de esta clase.

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
            Sistema sistema = Startup.BuildSistema();

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

            {
                //Crear persona:
                Persona persona = new Persona()
                {
                    Nombre = "Angel",
                    Paterno = "Farias",
                    Materno = "Aguila",
                    Rut = "142339882",
                    Email = "Angel.Farias@gmail.com"
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
                    CostoUnidad = 100000
                };
                
                Servicio servicio2 = new Servicio()
                {
                    Descripcion = "Animacion 2D de 2 Min",
                    Cantidad = 1,
                    CostoUnidad = 200000
                };

                IList<Servicio> servicios = new List<Servicio>();
                
                servicios.Add(servicio1);
                servicios.Add(servicio2);

                //Asignar los servicios a la cotizacion.
                cotizacion.Servicios = servicios;
                
                //Guardar la cotizacion en la base de datos.
                sistema.GuardarCotizacion(cotizacion);
                
                
                //Guardar los servicios en la base de datos.
                //(Ya que dependen de la cotizacion, solo se pueden guardar despues de haber guardado la cotizacion.)
                foreach (Servicio s in servicios)
                {
                    sistema.GuardarServicio(s);
                }
                
                //-------------------------------
                //Despliegue:
                //-------------------------------
                
                //Forma 1:
                {
                    Console.WriteLine(
                        "Mostrar los servicios de la cotizacion anterior, accediendo directamente a su atributo Servicios");
                    
                    IList<Servicio> serviciosCotizacionAnterior = cotizacion.Servicios;
                    foreach (Servicio s in serviciosCotizacionAnterior)
                    {
                        Console.WriteLine(Utils.ToJson(s));
                    }
                }
                
                //Forma 2:
                {
                    Console.WriteLine(
                        "Mostrar los servicios de la cotizacion anterior, mediante el repositorio de servicios.");

                    IList<Servicio> serviciosCotizacionAnterior =
                        sistema.ObtenerServiciosPorIDCotizacion(cotizacion.Identificador);

                    foreach (Servicio s in serviciosCotizacionAnterior)
                    {
                        Console.WriteLine(Utils.ToJson(s));
                    }
                }
            }
            
            Console.WriteLine("Fin de la aplicacion.");
        }
    }
}