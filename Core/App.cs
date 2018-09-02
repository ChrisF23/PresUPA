using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;

//TODO: Crear la clase Cliente y asignarla como atributo a Cotizacion.

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
               
                //Crear cotizacion.
                
                Cotizacion cotizacion = new Cotizacion()
                {
                    //TODO: Luego de crear la clase cliente, cambiar la siguiente linea por: Cliente = cliente;
                    FKRutCliente = "1234567890",
                    Titulo = "Video resumen 10 años Cemp UCN",
                    Descripcion = "Grabación, edición y postproducción de video de 3 a 4 minutos " +
                                  "sobre actividad de los 10 años del Cemp UCN. El valor incluye 2 revisiones previa entrega, postproducción " +
                                  "de imagen y audio, gráficas de presentación inicio y cierre. Los valores por este trabajo " +
                                  "son de acuerdo a tarifa especial para unidades pertenecientes a la UCN",
                    //fechaCreacion = DateTime.Now,
                    //Estado = EstadoCotizacion.Borrador,
                    
                       
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
                
                cotizacion.AsignarServicios(servicios);
                
                //sistema.Save();
                sistema.GuardarCotizacion(cotizacion);
                
                Console.WriteLine(Utils.ToJson(cotizacion));

            }

            Console.WriteLine("Fin de la aplicacion.");
        }
    }
}