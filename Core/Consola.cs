using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;

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
                Console.WriteLine("[2] Administrar Clientes");
                Console.WriteLine("[3] Administrar Usuarios");
                Console.WriteLine("[0] Salir");

                input = Console.ReadLine();
                
                if (input == null) continue;
                if (input.Equals("1"))
                {
                    MenuAdministrarCotizaciones(sistema, usuario);
                }
                else if (input.Equals("2"))
                {
                }
                else if (input.Equals("3"))
                {
                    
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
                Console.WriteLine("[4] Buscar Cotizacion");
                Console.WriteLine("[4] Ver cotizaciones");
                
                Console.WriteLine("[0] Salir");

                input = Console.ReadLine();
                
                if (input == null) continue;
                if (input.Equals("1"))
                {
                    sistema.Anadir(FormularioNuevaCotizacion()); //Almacena la cotizacion creada en el formulario.
                }
                else if (input.Equals("2"))
                {
                    Console.WriteLine("\n>Borrar Cotizacion");
            
                    Console.WriteLine("Ingrese el identificador de la cotizacion que desea borrar:");
                    string identificador = Console.ReadLine();
                    
                    sistema.Borrar(identificador);
                }
                else if (input.Equals("3"))
                {
                    Console.WriteLine("\n>Editar Cotizacion");
            
                    Console.WriteLine("Ingrese el identificador de la cotizacion que desea borrar:");
                    string identificador = Console.ReadLine();

                    Cotizacion cotizacion = sistema.BuscarCotizacion(identificador);

                    FormularioEditarCotizacion(cotizacion);
                }
                else if (input.Equals("4"))
                {
                    
                }

            }
        }

        //TODO: Completar!
        public static void FormularioCambiarEstadoCotizacion()
        {
            Console.WriteLine("Ingrese el nuevo estado de la cotizacion:");
            Console.WriteLine("[1] "+EstadoCotizacion.Borrador);
            Console.WriteLine("[1] "+EstadoCotizacion.Enviada);
            Console.WriteLine("[1] "+EstadoCotizacion.Aprovada);
            Console.WriteLine("[1] "+EstadoCotizacion.Rechazada);
            Console.WriteLine("[1] "+EstadoCotizacion.Terminada);
            
            
            //sistema.cambiarEstado(idCotizacion, nuevoEstado);
        }


        public static void FormularioEditarCotizacion(Cotizacion cotizacion)
        {
            string input = "...";
            while (input != null && !input.Equals("0"))
            {
                Console.WriteLine("\n>Edicion de Cotizacion");
                
                Console.WriteLine(Utils.ToJson(cotizacion));

                Console.WriteLine("[1] Editar titulo");
                Console.WriteLine("[2] Editar descripcion");
                Console.WriteLine("[3] Editar servicios");
                Console.WriteLine("[4] Guardar cambios");
                Console.WriteLine("[0] Cancelar cambios y volver");
                
                input = Console.ReadLine();
                
                //TODO: Cambiar los ifs por switchs?

                if (input == null) continue;
                if (input.Equals("1"))
                {
                    Console.WriteLine("Ingrese el nuevo titulo:");
                    cotizacion.Titulo = Console.ReadLine();
                } else if (input.Equals("2"))
                {
                    Console.WriteLine("Ingrese la nueva descripcion:");
                    cotizacion.Titulo = Console.ReadLine();
                }else if (input.Equals("3"))
                {
                    Console.WriteLine("Cantidad de Servicios: "+ cotizacion.Servicios.Count);
                    int cntr = 0;
                    foreach (Servicio s in cotizacion.Servicios)
                    {
                        Console.WriteLine("Indice: "+(cntr++));
                        Console.WriteLine(Utils.ToJson(s));
                    }

                    Console.WriteLine("Ingrese el indice del servicio que desea editar");
                    //TODO: Terminar esto!

                }else if (input.Equals("4"))
                {
                   //sistema.anadir(cotizacion);
                }else if (input.Equals("0"))
                {
                    break;
                }

            }
            
        }

        public static Cotizacion FormularioNuevaCotizacion()
        {
            
            Console.WriteLine("\n>Anadir Cotizacion");
            
            Console.WriteLine("Ingrese el titulo de la cotizacion:");
            string titulo = Console.ReadLine();
            
            Console.WriteLine("Ingrese la descripcion de la cotizacion:");
            string descripcion = Console.ReadLine();

            Console.WriteLine("Anada al Cliente:");
            Cliente cliente = FormularioNuevoCliente();

            List<Servicio> servicios = new List<Servicio>();

            Console.WriteLine("Anada los servicios:");

            while (true)
            {
                string input = "...";
                Servicio servicio = FormularioNuevoServicio();
                servicios.Add(servicio);
                
                //Desplegar Servicio:
                Console.WriteLine("Servicio: "+Utils.ToJson(servicio));
                
                Console.WriteLine("[1] Anadir otro servicio");
                Console.WriteLine("[Otro] Terminar de anadir servicios");
                input = Console.ReadLine();

                if (input != null && input.Equals("1"))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            
            //Crear cotizacion:
            
            return new Cotizacion()
            {
                Titulo = titulo,
                Descripcion = descripcion,
                Cliente = cliente,
                Servicios = servicios
            };
        }

        public static Servicio FormularioNuevoServicio()
        {
            Console.WriteLine(">Anadir Servicio");

            Console.WriteLine("Ingrese una descripcion para el servicio:");
            string descripcion = Console.ReadLine();
            
            Console.WriteLine("Ingrese el costo de este servicio:");
            int costoUnidad = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Ingrese la cantidad de este servicio:");
            int cantidad = int.Parse(Console.ReadLine());
            
            return new Servicio()
            {
                Descripcion = descripcion,
                CostoUnidad = costoUnidad,
                Cantidad = cantidad
            };
        }

        public static Cliente FormularioNuevoCliente()
        {
            TipoCliente tipoCliente = TipoCliente.Otro;
            
            Console.WriteLine(">Anadir Cliente");
            
            Console.WriteLine("Ingrese el rut del cliente:");
            string rut = Console.ReadLine();
            
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
            
            Persona pCliente = new Persona()
            {
                Rut = rut,
                Nombre = nombre,
                Paterno = apellidoP,
                Materno = apellidoM,
                Email = correo
            };
            
            return new Cliente()
            {
                Persona = pCliente,
                Tipo = tipoCliente
            };
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
                
                if (input == null) continue;
                if (input.Equals("1"))
                {
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

                if (input == null) continue;
                if (input.Equals("1"))
                {
                }
            }
        }
        
        
        
        
    }
}