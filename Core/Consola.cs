using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using Core.Controllers;
using Core.Models;
using SQLitePCL;

namespace Core
{
    public class Consola
    {
        public static void MenuDirector(ISistema sistema, Usuario usuario)
        {
             
            string input = "...";
            while (input != null && !input.Equals("0"))
            {
                Console.WriteLine("\n>Menu principal");
                Console.WriteLine("[1] Administrar Cotizaciones");
                //Console.WriteLine("[2] Administrar Clientes");
                //Console.WriteLine("[3] Administrar Usuarios");
                Console.WriteLine("[0] Cerrar Sesion");

                input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        MenuAdministrarCotizaciones(sistema, usuario);
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "0":
                        Console.WriteLine("\nCerrando Sesion...");
                        break;
                    default:
                        continue;
                }
            }
        }
        
        public static void MenuAdministrarCotizaciones(ISistema sistema, Usuario usuario)
        {
            string input = "...";
            while (input != null && !input.Equals("0"))
            {
                Console.WriteLine("\n>Administrar Cotizaciones");
                Console.WriteLine("[1] Anadir Cotizacion");
                Console.WriteLine("[2] Borrar Cotizacion");
                Console.WriteLine("[3] Editar Cotizacion");
                Console.WriteLine("[4] Cambiar Estado de Cotizacion");
                Console.WriteLine("[5] Buscar Cotizacion");
                Console.WriteLine("[6] Ver cotizaciones");
                Console.WriteLine("[7] Enviar Cotizacion");
                Console.WriteLine("[0] Volver al menu anterior");

                input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                    {
                        FormularioNuevaCotizacion(sistema);
                        break;
                    }
                    case "2":
                    {
                        FormularioBorrarCotizacion(sistema);
                        break;
                    }
                    case "3":
                    {
                        FormularioEditarCotizacion(sistema);
                        break;
                    }
                    case "4":
                    {
                        FormularioCambiarEstadoCotizacion(sistema);
                        break;
                    }
                    case "5":
                        //TODO: Implementar Busqueda (En el mejor de los casos, Metabusqueda).
                        FormularioBuscarCotizaciones(sistema);
                        break;
                    
                    case "6":
                    {
                        MostrarCotizacionesParaDirector(sistema);
                        break;
                    }
                    case "7":
                    {
                        FormularioEnviarCotizacion(sistema, usuario);
                        break;
                    }
                    case "0":
                        return;
                    default:
                        continue;
                }
            }
        }


        public static void FormularioBuscarCotizaciones(ISistema sistema)
        {
            Console.WriteLine(">Ingrese algun termino de busqueda:");
            string input = Console.ReadLine();

            try
            {
                IList<Cotizacion> resultados = sistema.BuscarCotizaciones(input);

                if (resultados.Count == 0)
                {
                    Console.WriteLine("\nNo se encontraron resultados para su busqueda.");
                    return;
                }

                Console.WriteLine("\nSe encontraron " + resultados.Count +
                                  " cotizaciones que coinciden con su busqueda:");
                
                foreach (Cotizacion resultado in resultados)
                {
                    Console.WriteLine("\n----------------");
                    Console.WriteLine(resultado.ToString());
                }
                Console.WriteLine("----------------\n");                
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Muestra todas las cotizaciones que se encuentran en el sistema.
        /// </summary>
        /// <param name="sistema"></param>
        public static void MostrarCotizacionesParaDirector(ISistema sistema)
        {
            Console.WriteLine("Mostrando Cotizaciones...\n");
            try
            {
                IList<Cotizacion> cotizaciones = sistema.GetCotizaciones();
                foreach (Cotizacion cotizacion in cotizaciones)
                {
                    Console.WriteLine("\n--------------------------");
                    Console.WriteLine(cotizacion.ToString());
                }
                Console.WriteLine("--------------------------\n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        /// <summary>
        /// Muestra solo las cotizaciones que han sido aprovadas.
        /// </summary>
        /// <param name="sistema"></param>
        public static void MostrarCotizacionesParaProductor(ISistema sistema)
        {
            Console.WriteLine("Mostrando Cotizaciones...");
            foreach (Cotizacion cotizacion in sistema.GetCotizaciones())
                if (cotizacion.Estado == EstadoCotizacion.Aprobada)
                Console.WriteLine(Utils.ToJson(cotizacion));
        }
        
        /// <summary>
        /// Muestra solo las cotizaciones que han sido terminadas.
        /// </summary>
        /// <param name="sistema"></param>
        public static void MostrarCotizacionesParaSupervisor(ISistema sistema)
        {
            Console.WriteLine("Mostrando Cotizaciones...");
            foreach (Cotizacion cotizacion in sistema.GetCotizaciones())
                if (cotizacion.Estado == EstadoCotizacion.Terminada)
                    Console.WriteLine(Utils.ToJson(cotizacion));
        }
        
        public static void FormularioBorrarCotizacion(ISistema sistema)
        {
            Console.WriteLine("\n>Borrar Cotizacion");

            Console.WriteLine("Ingrese el identificador de la cotizacion que desea borrar:");
            string identificador = Console.ReadLine();

            try
            {
                sistema.Borrar(identificador);
                Console.WriteLine("Cotizacion borrada!");
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public static void FormularioCambiarEstadoCotizacion(ISistema sistema)
        {
            
            Console.WriteLine("\n>Cambiar Estado de Cotizacion");
            
            Console.WriteLine("Ingrese el identificador de la cotizacion que desea editar:");
            string identificador = Console.ReadLine();

            EstadoCotizacion estadoAntiguo;
            
            try
            {
                estadoAntiguo = sistema.BuscarCotizacion(identificador).Estado;   
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
            while (true)
            {
                
                //NOTA: Cambiar manualmente al estado "Enviada" no esta permitido.
                Console.WriteLine("");
                Console.WriteLine("Ingrese el nuevo estado de la cotizacion:");
                Console.WriteLine("[1] " + EstadoCotizacion.Borrador);
                Console.WriteLine("[2] " + EstadoCotizacion.Aprobada);
                Console.WriteLine("[3] " + EstadoCotizacion.Rechazada);
                Console.WriteLine("[4] " + EstadoCotizacion.Terminada);
                Console.WriteLine("[0] Cancelar cambio de estado");
                
                string input = Console.ReadLine();

                EstadoCotizacion estadoNuevo = EstadoCotizacion.Borrador;    //Inicia en borrador por defecto.
                
                switch (input)
                {
                    case "1":
                        estadoNuevo = EstadoCotizacion.Borrador;
                        break;
                    case "2":
                        estadoNuevo = EstadoCotizacion.Aprobada;
                        break;
                    case "3":
                        estadoNuevo = EstadoCotizacion.Rechazada;
                        break;
                    case "4":
                        estadoNuevo = EstadoCotizacion.Terminada;
                        break;
                    case "0":
                        break;
                    default:
                        continue;
                }

                //Si se ingresa 0, se cancela la operacion y se retorna al menu anterior.
                if (input.Equals("0"))
                    break;
                
                //Si el estado es el mismo, lanza un mensaje y vuelve al inicio del while.
                if (estadoNuevo == estadoAntiguo)
                {
                    Console.WriteLine("La cotizacion ya se encuentra en este estado.");
                    continue;
                }

                //Si se escogio una opcion posible, se intenta cambiar el estado.
                try
                {
                    sistema.CambiarEstado(identificador, estadoNuevo);
                    Console.WriteLine("Se ha cambiado el estado de la cotizacion.");
                }
                catch (ModelException e)
                {
                    Console.WriteLine(e.Message);
                }

                break;
            }
        }

        public static void FormularioEditarCotizacion(ISistema sistema)
        {
            
            Console.WriteLine("\n>Editar Cotizacion");
            
            Console.WriteLine("Ingrese el identificador de la cotizacion que desea editar:");
            string identificador = Console.ReadLine();

            Cotizacion original;
            try
            {
                original = sistema.BuscarCotizacion(identificador);
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            //Se crea una copia parcial de la cotizacion a editar.
            Cotizacion copy = new Cotizacion()
            {
                Identificador = original.Identificador,
                Titulo = original.Titulo,
                Descripcion = original.Descripcion,
                Numero = original.Numero,
                Version = original.Version,
                Cliente = original.Cliente,
                Servicios = original.Servicios,
                CostoTotal = original.CostoTotal,    
            };
            
            bool exit = false;
            while (true)
            {
               
                Console.WriteLine(copy.ToString());
                
                Console.WriteLine("\n>Edicion de Cotizacion");
                Console.WriteLine("[1] Editar titulo");
                Console.WriteLine("[2] Editar descripcion");
                Console.WriteLine("[3] Cambiar cliente");
                Console.WriteLine("[4] Editar servicios");
                Console.WriteLine("[5] Guardar cambios y volver");
                Console.WriteLine("[0] Cancelar cambios y volver");
                
                string input = Console.ReadLine();
               
                switch (input)
                {
                    case "1":
                    {
                        Console.WriteLine("Ingrese el nuevo titulo:");
                        copy.Titulo = Console.ReadLine();
                        break;
                    }
                    case "2":
                    {
                        Console.WriteLine("Ingrese la nueva descripcion:");
                        copy.Descripcion = Console.ReadLine();
                        break;
                    }
                    case "3":
                    {
                        Console.WriteLine("Ingrese los datos del nuevo cliente:");
                        Cliente nuevoCliente = FormularioNuevoCliente(sistema);
                        
                        while (nuevoCliente == null)
                        {
                            Console.WriteLine("Hubo un error al ingresar los datos del nuevo cliente...");
                            Console.WriteLine("[1] Intentar otra vez");
                            Console.WriteLine("[Otro] Mantener cliente anterior");

                            input = Console.ReadLine();

                            if (input == "1")
                                nuevoCliente = FormularioNuevoCliente(sistema);
                            else
                                break;
                        }

                        if (nuevoCliente != null)
                        {
                            copy.Cliente = nuevoCliente;
                            Console.WriteLine("Cliente actualizado.");
                        }
                        else
                            Console.WriteLine("Se ha cancelado el cambio de cliente.");
                        break;
                    }
                    case "4":
                    {
                        Console.WriteLine("Cantidad de Servicios: " + copy.Servicios.Count);
                        int index = 0;
                        foreach (Servicio s in copy.Servicios)
                        {
                            Console.WriteLine("\n------------------");
                            Console.WriteLine("Indice: " + (++index));
                            Console.WriteLine(s.ToString());
                        }
                        Console.WriteLine("------------------\n");

                        //TODO: Terminar esto!
                        while (true)
                        {
                            Console.WriteLine(">Ingrese el indice del servicio que desea editar");
                            Console.WriteLine("[Otro] Cancelar");
                            int editIndex = 0;
                            try
                            {
                                editIndex = int.Parse(Console.ReadLine());
                                
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                continue;
                            }

                            if (editIndex >= 1 && editIndex <= copy.Servicios.Count)
                            {
                                Console.WriteLine(">Editar Servicio");
                                Console.WriteLine("[1] Cambiar Servicio");
                                Console.WriteLine("[2] Borrar Servicio");
                                Console.WriteLine("[Otro] Cancelar");

                                input = Console.ReadLine();

                                switch (input)
                                {
                                    case "1":
                                        copy.Servicios[editIndex - 1] = FormularioNuevoServicio();
                                        Console.WriteLine("Servicio cambiado.");
                                        break;
                                    case "2":
                                        copy.Servicios.RemoveAt(editIndex - 1);
                                        Console.WriteLine("Servicio borrado.");
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            
                            break;
                        }


                        break;
                    }
                    case "5":
                    {
                        try
                        {
                            sistema.Editar(copy);
                            Console.WriteLine("Se han guardado los cambios en una nueva version de esta cotizacion.");
                        }
                        catch (ModelException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        exit = true;
                        break;
                    }
                    case "0":
                    {
                        exit = true;
                        break;
                    }
                    default:
                        continue;
                }

                if (exit)
                    break;
                
                
                //Recalcular costo total de la copia:
                copy.CalcularMiCostoTotal();
            }
            
        }

        public static void FormularioNuevaCotizacion(ISistema sistema)
        {
            
            Console.WriteLine("\n>Anadir Cotizacion");
            
            Console.WriteLine("Ingrese el titulo de la cotizacion:");
            string titulo = Console.ReadLine();
            
            Console.WriteLine("Ingrese la descripcion de la cotizacion:");
            string descripcion = Console.ReadLine();

            //Obtener un nuevo cliente y anadirlo a la cotizacion.
            Cliente cliente = FormularioNuevoCliente(sistema);
            while (cliente == null)
            {
                Console.WriteLine("Hubo un error al ingresar el Cliente...");
                Console.WriteLine("[1] Intentar otra vez");
                Console.WriteLine("[Otro] Cancelar cotizacion");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        cliente = FormularioNuevoCliente(sistema);
                        break;
                    default:
                        return;    //Volver al menu anterior.
                }
            }

            //Crea la cotizacion con los datos obtenidos.
            Cotizacion nuevaCotizacion = new Cotizacion()
            {
                Titulo = titulo,
                Descripcion = descripcion,
                Cliente = cliente
            };
            
            Console.WriteLine(">>Anadir Servicios");
            
            //Obtener los servicios y anadirlos a la cotizacion.
            while (true)
            {
                string input = "...";

                Servicio nuevoServicio = FormularioNuevoServicio();

                //Intentar anadir el servicio a la cotizacion.
                try
                {
                    sistema.Anadir(nuevoServicio, nuevaCotizacion);
                    
                    Console.WriteLine("Servicio anadido:\n");
                    Console.WriteLine(nuevoServicio.ToString());
                    
                    Console.WriteLine("\n[1] Anadir nuevo servicio");
                    Console.WriteLine("[Otro] Terminar de anadir servicios");

                    input = Console.ReadLine();

                    if (input != null && input.Equals("1"))
                    {
                        //Repite el ciclo.
                        continue;
                    }
                    //Sale del while.
                    break;
                }
                catch (ModelException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Hubo un error al ingresar el servicio...");
                    Console.WriteLine("[1] Intentar otra vez");
                    Console.WriteLine("[Otro] Cancelar cotizacion");

                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            continue;    //Repite el ciclo.
                        default:
                            return;    //Volver al menu anterior.
                    }
                }
            }
            
            
            
            //Crear cotizacion:
            try
            {
                sistema.Anadir(nuevaCotizacion); //Almacena la cotizacion creada en el formulario.
                Console.WriteLine("Se ha anadido una nueva cotizacion.");
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Cancelando cotizacion...");
            }
        }

        public static Servicio FormularioNuevoServicio()
        {
            Console.WriteLine("\n>Anadir Servicio");

            Console.WriteLine("Ingrese una descripcion para el servicio:");
            string descripcion = Console.ReadLine();
            
            Console.WriteLine("Ingrese el costo de este servicio:");
            int costoUnidad;
            try
            {
                costoUnidad = int.Parse(Console.ReadLine());

            }
            catch (Exception e)
            {
                costoUnidad = 0;
            }

            Console.WriteLine("Ingrese la cantidad de este servicio:");
            int cantidad;
            try
            {
                cantidad = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                cantidad = 0;
            }
            
            return new Servicio()
            {
                Descripcion = descripcion,
                CostoUnidad = costoUnidad,
                Cantidad = cantidad
            };
        }

        /// <summary>
        /// Retorna un nuevo cliente. Si ocurre algun error al ingresar los datos, retorna nulo.
        /// </summary>
        /// <param name="sistema"></param>
        /// <returns></returns>
        public static Cliente FormularioNuevoCliente(ISistema sistema)
        {

            TipoCliente tipoCliente = TipoCliente.Otro;

            Console.WriteLine("\n>Anadir Cliente");

            Console.WriteLine("Ingrese el rut del cliente:");
            string rut = Console.ReadLine();

            //Comprobacion con la base de datos. Si existe el cliente con tal rut, usarlo.
            try
            {
                Cliente cliente = sistema.BuscarCliente(rut);
                Console.WriteLine("Utilizando Cliente preregistrado.");
                return cliente;
            }
            catch (ModelException)
            {
                //No se encontro un cliente; Seguir.
            }

            Console.WriteLine("Ingrese el nombre del cliente:");
            string nombre = Console.ReadLine();

            Console.WriteLine("Ingrese el apellido paterno del cliente:");
            string apellidoP = Console.ReadLine();

            Console.WriteLine("Ingrese el apellido materno del cliente (Opcional):");
            string apellidoM = Console.ReadLine();

            Console.WriteLine("Ingrese el correo del cliente:");
            string correo = Console.ReadLine();

            Console.WriteLine("Pertenece el cliente a la unidad interna? [s/n]:");
            string interno = Console.ReadLine();

            if (interno != null && interno.ToLower().Equals("s"))
            {
                tipoCliente = TipoCliente.UnidadInterna;
            }

            Persona nuevaPersona = new Persona()
            {
                Rut = rut,
                Nombre = nombre,
                Paterno = apellidoP,
                Materno = apellidoM,
                Email = correo
            };

            try
            {
                sistema.Anadir(nuevaPersona, tipoCliente);
                Console.WriteLine("Se ha anadido un nuevo Cliente.");

                return sistema.BuscarCliente(nuevaPersona.Rut);

            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);    //Solo mostrara el primer error que ocurra.
            }
            
            return null;
        }

        public static void FormularioEnviarCotizacion(ISistema sistema, Usuario usuario)
        {
            
            Console.WriteLine("\n>Enviar Cotizacion");
            
            Cotizacion cotizacionEnviar;

            IList<Cotizacion> cotizaciones = null;
            
            try
            {
                cotizaciones = sistema.GetCotizaciones();
            }
            catch (ModelException e)
            {
                Console.WriteLine(e);
                return;    //Volver si no hay cotizaciones u ocurre otro error.
            }
            
            //Despliegue resumido de las cotizaciones:
            foreach(Cotizacion c in cotizaciones)
            {
                Console.WriteLine("\n---------------");
                Console.WriteLine(c.ToStringBrief());
            }
            Console.WriteLine("---------------\n");

            Console.WriteLine("Ingrese el identificador de la cotizacion que desea enviar:");
            string identificador = Console.ReadLine();

            //Obtener cotizacion con el identificador ingresado:
            try
            {
                cotizacionEnviar = sistema.BuscarCotizacion(identificador);
            }
            catch (ModelException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
            //Seleccion del remitente:
            
            string remitente = null;
            string input = "...";
            while (true)
            {
                Console.WriteLine("\n>Seleccione el Email del remitente:");
                Console.WriteLine("[1] Usar Email de UPA");
                Console.WriteLine("[2] Usar mi Email");
                Console.WriteLine("[0] Cancelar envio");

                input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        remitente = sistema.EmailUpa;
                        break;
                    case "2":
                        remitente = usuario.Persona.Email;
                        break;
                    case "0":
                        return;
                    default:
                        continue;
                }
                
                break;
            }
            
            //Ingresar contrasena del Email del remitente:

            Console.WriteLine("\n>Ingrese la contrasena del Email para enviar:");
            Console.WriteLine("Email: " + remitente);
            Console.Write("Contrasena: ");

            string emailPassword = Console.ReadLine();
            
            //Destinatario:
            string destinatario = cotizacionEnviar.Cliente.Persona.Email;
            
            
            //Guardando todos los servicios de la cotizacion:
            string servicios = null;

            foreach (Servicio servicio in cotizacionEnviar.Servicios)
            {
                servicios +=
                    "<p><b>" + servicio.Descripcion + @"<b></p>
                    <p>Cantidad: " + servicio.Cantidad + @"</p>
                    <p>Costo Unidad: $" + servicio.CostoUnidad + @"</p>
                    <p><b>Sub total: $" + (servicio.Cantidad * servicio.CostoUnidad) + @"<b></p><br>";
            }

            string plantillaFondo =
                "Nos encontramos a su disposición para consultas y comentarios. Atentamente suyo,";

            string nombreDirector =
                usuario.Persona.Nombre + " " + usuario.Persona.Paterno + " " + usuario.Persona.Materno;
            
            //Creacion del Email:
            MailMessage mailMessage = new MailMessage()
            {
                Subject ="Cotizacion UPA N°" + cotizacionEnviar.Numero + 
                         " Version " + cotizacionEnviar.Version,
                IsBodyHtml = true,
                Body = @"<html>
                        <body>
                        <p><b>Fecha: " + Utils.ToFormatedDate(cotizacionEnviar.FechaCreacion)+ @"</b></p>
                        <h1>Titulo: " + cotizacionEnviar.Titulo + @"</h1>
                        <h2>Descripcion: " + cotizacionEnviar.Descripcion + @"</h2>
                        <h2>Servicios:</h2>" + servicios +
                       "<p><b>Costo Total: $" + cotizacionEnviar.CostoTotal + @"</b></p>
                        <br><p>" + plantillaFondo + @"</p>
                        <p><b>"+ nombreDirector+@"</b></p>
                        <p>Director de proyectos <b>UPA Periodismo UCN</b></p>
                        <p>"+remitente+@"</p>
                        </body>
                        </html>
                        "
            };

            Console.WriteLine();
            
            try
            {
                sistema.EnviarEmail(cotizacionEnviar, remitente, emailPassword, destinatario, mailMessage);
                Console.WriteLine("Se ha enviado la cotizacion!");
            }
            catch (Exception e)
            {
                //Puede ser una SmtpException o un ModelException
                Console.WriteLine(e.Message);
                Console.WriteLine("Se ha cancelado el envio debido a un problema.");
            }
        }
        
        public static void MenuProductor(ISistema sistema, Usuario usuario)
        {
            string input = "...";
            while (input != null && !input.Equals("0"))
            {
                Console.WriteLine("\n>Menu principal");
                Console.WriteLine("[1] Administrar Servicios");
                Console.WriteLine("[0] Salir");

                input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        break;
                    default:
                        continue;
                }
            }
        }
        
        public static void MenuSupervisor(ISistema sistema, Usuario usuario)
        {
            string input = "...";
            while (input != null && !input.Equals("0"))
            {
                Console.WriteLine("\n>Menu principal");
                Console.WriteLine("[1] Ver Cotizaciones");
                Console.WriteLine("[0] Salir");

                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        break;
                    default:
                        continue;
                }
            }
        }

    }
} 