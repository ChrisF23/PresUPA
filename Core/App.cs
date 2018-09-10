using System;
using System.Collections.Generic;
using System.Net.Mail;
using Core.Controllers;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

//TODO: Implementar la busqueda de cotizaciones (Consola, Sistema).
//TODO: Implementar el envio de cotizaciones por correo (Sistema).
//TODO: Completar los demas menus para cada usuario y sus operaciones. --
//TODO: -- El Menu Director (exceptuando la busqueda y el envio) se encuentra listo.
//TODO: Documentar.
//TODO: Eliminar funciones que nunca se usan (Hacerlo al final de todo!).
//TODO: Ingresar permanentemente un Usuario Director, Productor y Supervisor.
//TODO: Con completar la administracion de cotizaciones, el proyecto quedaria (en teoria) terminado.
//TODO: Crear una sobrecarga para el metodo ToString(). Para mejorar el despliegue de los objetos.

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
            
            //Creacion de Usuario de prueba:
            Persona personaDirector = new Persona()
            {
                Rut = "19691840K",
                Nombre = "Christian",
                Paterno = "Farias",
                Materno = "Aguila",
                Email = "christian.farias@alumnos.ucn.cl"
            };

            Usuario usuarioDirector = new Usuario()
            {
                Persona = personaDirector,
                Password = "1234",
                Tipo = TipoUsuario.Director
            };

           
            try
            {
                sistema.Anadir(usuarioDirector);
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            //Login:
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
                Console.WriteLine("Saliendo...");
                return;
            }
            
                
            //Menu:
            Console.WriteLine("--------------------------");
            Console.WriteLine("    P r e s U P A");
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
