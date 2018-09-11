using System;
using System.Collections.Generic;
using System.Net.Mail;
using Core.Controllers;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

//TODO: Documentar.
//TODO: Eliminar funciones que nunca se usan (Hacerlo al final de todo!).
//TODO: Con completar la administracion de cotizaciones, el proyecto quedaria (en teoria) terminado.

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
            
            //NOTA: Use estos usuarios de prueba:
            
            Usuario usuarioDirector = new Usuario()
            {
                Persona = new Persona()
                {
                    Rut = "194460880",
                    Nombre = "German",
                    Paterno = "Rojas",
                    Materno = "Arce",
                    Email = "garojar97@gmail.com"
                },
                Password = "1234",
                Tipo = TipoUsuario.Director
            };
            
            Usuario usuarioProductor = new Usuario()
            {
                Persona = new Persona()
                {
                    Rut = "19691840K",
                    Nombre = "Christian",
                    Paterno = "Farias",
                    Materno = "Aguila",
                    Email = "christian.farias@alumnos.ucn.cl"
                },
                Password = "1234",
                Tipo = TipoUsuario.Productor
            };
            
            Usuario usuarioSupervisor = new Usuario()
            {
                Persona = new Persona()
                {
                    Rut = "130144918",
                    Nombre = "Diego",
                    Paterno = "Urrutia",
                    Materno = "Astorga",
                    Email = "durrutia@ucn.cl"
                },
                Password = "1234",
                Tipo = TipoUsuario.Supervisor
            };
            
           
            try
            {
                sistema.Anadir(usuarioDirector);
                sistema.Anadir(usuarioProductor);
                sistema.Anadir(usuarioSupervisor);
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            int intentos = 3;
            //Login: Si falla 3 veces en ingresar, el programa termina.
            while (intentos > 0)
            {
                Console.WriteLine("\n(Intentos: {0})", intentos);
                Console.WriteLine("Ingrese su rut o email: ");
                string rutEmail = Console.ReadLine();
                Console.WriteLine("Ingrese su contrasena: ");
                string password = Console.ReadLine();

                Usuario usuario = null;

                try
                {
                    usuario = sistema.Login(rutEmail, password);
                }
                catch (ModelException e)
                {
                    Console.WriteLine(e.Message);
                    intentos--;
                    continue;    //Vuelve al while.
                }

                intentos = 3;
                
                //Mostrar menu segun usuario:
                Console.WriteLine("\n--------------------------");
                Console.WriteLine("    P r e s U P A");
                Console.WriteLine("--------------------------");

                //Es necesario pasar al usuario por parametro.
                switch (usuario.Tipo)
                {
                    case TipoUsuario.Director:
                        Consola.MenuDirector(sistema, usuario);
                        break;
                    case TipoUsuario.Productor:
                        Consola.MenuProductor(sistema, usuario);
                        break;
                    case TipoUsuario.Supervisor:
                        Consola.MenuSupervisor(sistema, usuario);
                        break;
                }
                
            }

            Console.WriteLine("\nPrograma terminado.");
        }

        

       

        

    }
}
