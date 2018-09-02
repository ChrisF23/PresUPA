using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;

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

            // crear servicios, crear cotizacion.
            {
                //Crear persona (Cliente).

                Persona persona = new Persona()
                {
                    Rut = "130144918",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga",
                    Email = "durrutia@ucn.cl"
                };
                
                //Crear cotizacion.
                
                Cotizacion cotizacion = new Cotizacion()
                {
                    RutCliente = persona.Rut,
                    Titulo = "Video resumen 10 años Cemp UCN",
                    Descripcion = "Grabación, edición y postproducción de video de 3 a 4 minutos " +
                                  "sobre actividad de los 10 años del Cemp UCN. El valor incluye 2 revisiones previa entrega, postproducción " +
                                  "de imagen y audio, gráficas de presentación inicio y cierre. Los valores por este trabajo " +
                                  "son de acuerdo a tarifa especial para unidades pertenecientes a la UCN",
                    fechaCreacion = DateTime.Now,

                };
                
                //Crear los servicios.
                
                int servicio1 = 10000;
                int servicio2 = 12000;
                int servicio3 = 9000;
                int servicio4 = 15000;

                IList<int> servicios = new List<int>();
                
                servicios.Add(servicio1);
                servicios.Add(servicio2);
                servicios.Add(servicio3);
                servicios.Add(servicio4);

                //Asignar los servicios a la cotizacion.
                
                cotizacion.AsignarServicios(servicios);
                
                //sistema.Save();
                
                Console.WriteLine(Utils.ToJson(persona));
                Console.WriteLine(Utils.ToJson(cotizacion));

            }

            Console.WriteLine("Fin de la aplicacion.");
        }
    }
}