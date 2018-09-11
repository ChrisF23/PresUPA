using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace Core.Controllers
{
    /// <summary>
    /// Implementacion de los contratos de la interface ISistema.
    /// </summary>
    public sealed class Sistema : ISistema
    {
        // Patron Repositorio, generalizado via Generics
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/
        
        /// <inheritdoc />
        public string EmailUpa { get; }

        /// <summary>
        /// Repositorio de Personas.
        /// </summary>
        private readonly IRepository<Persona> _repositoryPersona;

        /// <summary>
        /// Repositorio de Usuarios.
        /// </summary>
        private readonly IRepository<Usuario> _repositoryUsuario;

        /// <summary>
        /// Repositorio de Cotizaciones.
        /// </summary>
        private readonly IRepository<Cotizacion> _repositoryCotizacion;

        /// <summary>
        /// Repositorio de Clientes.
        /// </summary>
        private readonly IRepository<Cliente> _repositoryCliente;

        /// <summary>
        /// Variable que guarda el numero de la ultima cotizacion ingresada a la base de datos. Usa 300 por defecto.
        /// </summary>
        private int _lastCotizacionNumber;

        /// <summary>
        /// Constructor: Inicializa los repositorios internos de la clase.
        /// </summary>
        public Sistema(
            IRepository<Persona> repositoryPersona, 
            IRepository<Usuario> repositoryUsuario, 
            IRepository<Cotizacion> repositoryCotizacion,
            IRepository<Cliente> repositoryCliente)
        {
            EmailUpa = "upaucnproyecto@outlook.com";
            
            // Setter!
            _repositoryPersona = repositoryPersona ??
                                 throw new ArgumentNullException("Se requiere el repositorio de personas");
            _repositoryUsuario = repositoryUsuario ??
                                 throw new ArgumentNullException("Se requiere repositorio de usuarios");
            _repositoryCotizacion = repositoryCotizacion ??
                                 throw new ArgumentNullException("Se requiere repositorio de cotizaciones");
            _repositoryCliente = repositoryCliente ??
                                    throw new ArgumentNullException("Se requiere repositorio de clientes");
           
            
            // Inicializacion del repositorio.
            _repositoryPersona.Initialize();
            _repositoryUsuario.Initialize();
            _repositoryCotizacion.Initialize();
            _repositoryCliente.Initialize();
            
            
            //Determinar el numero de la ultima cotizacion guardada.
            var x = _repositoryCotizacion.GetAll();
            if (x.Count > 0)
            {
                var numero = x.OrderByDescending(c => c.Numero).First().Numero;
                _lastCotizacionNumber = numero ?? 300;

            }
            else
            {
                //La lista no contiene elementos -> Usar 300 por defecto.
                _lastCotizacionNumber = 300;
            }

        }

        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cotizaciones (OS_COXXX)
        //------------------------------------------------------------------------------

        /// <inheritdoc />
        public void Anadir(Cotizacion cotizacion)
        {
            // Verificacion de nulidad
            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion es null");
            }

            //Si el numero de la cotizacion es null, entonces es una nueva cotizacion.
            //Si no lo es, entonces se conserva su numero.
            if (cotizacion.Numero == null)
            {
                cotizacion.Numero = ++_lastCotizacionNumber;
            }
            
            //Si la version de la cotizacion es null, entonces es la primera version.
            //Si no lo es, significa que es una nueva version.
            if (cotizacion.Version == null)
            {
                cotizacion.Version = 1;
            }
            else
            {
                cotizacion.Version += 1;
            }
            
            //Establece la fecha de creacion:
            cotizacion.FechaCreacion = DateTime.Now;
            
            //Asigna su identificador.
            cotizacion.Identificador = (cotizacion.Numero.ToString() +"v"+ cotizacion.Version.ToString());
            
            //Asigna el estado por defecto (borrador = 0).
            cotizacion.Estado = EstadoCotizacion.Borrador;
            
            _repositoryCotizacion.Add(cotizacion);
        }

        /// <inheritdoc />
        public void Borrar(string idCotizacion)
        {
            //Validacion de nulidad.
            if (idCotizacion == null)
                throw new ModelException("El identificador ingresado fue nulo.");
            
            if (string.IsNullOrEmpty(idCotizacion))
                throw new ModelException("El identificador ingresado esta vacio.");
            
            //Buscar y borrar.
            _repositoryCotizacion.Remove(BuscarCotizacion(idCotizacion));
        }

        /// <inheritdoc />
        public void Editar(Cotizacion cotizacion)
        {
            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion es null.");
            }
            //Se recibe por parametro la cotizacion ya editada.
            //Si la cotizacion recibida es distinta a la obtenida por su identificador,
            //Se procede a agregar la nueva version en la base de datos.

            Cotizacion comparar = BuscarCotizacion(cotizacion.Identificador);
            
            if (Validate.CompararCotizaciones(cotizacion, comparar))
            {
                //No hay cambios.
                throw new ModelException("No se detectaron cambios.");
            }
            
            //Cambios detectados. Guardar.
            Anadir(cotizacion);
        }

        /// <inheritdoc />
        public void CambiarEstado(string idCotizacion, EstadoCotizacion nuevoEstado)
        {
            //Verificacion de nulidad de idCotizacion se realiza en BuscarCotizacion.
            Cotizacion cotizacion = BuscarCotizacion(idCotizacion);
            
            /*
             * Reglas:
             * 1.- Una cotizacion empieza siempre como borrador.
             * 2.- Una cotizacion que esta en borrador, solo puede ser enviada o rechazada.
             * 3.- Una cotizacion enviada solo puede ser aprobada o rechazada.
             * 4.- Una cotizacion que ha sido enviada o rechazada, no puede volver a estar en borrador.
             * 5.- Una cotizacion que ha sido aprobada, solo puede ser terminada o rechazada.
             * 6.- Una cotizacion que esta terminada, no puede cambiar de estado.
             */


            switch (cotizacion.Estado)
            {
                case EstadoCotizacion.Borrador:
                {
                    if (nuevoEstado != EstadoCotizacion.Enviada && nuevoEstado != EstadoCotizacion.Rechazada)
                        throw new ModelException("Esta cotizacion solo puede ser enviada o rechazada!");
                    break;
                }
                case EstadoCotizacion.Enviada:
                {
                    if (nuevoEstado != EstadoCotizacion.Aprobada && nuevoEstado != EstadoCotizacion.Rechazada)
                        throw new ModelException("Esta cotizacion solo puede ser aprobada o rechazada!");
                    break;
                }
                case EstadoCotizacion.Rechazada:
                {
                    if (nuevoEstado != EstadoCotizacion.Aprobada && nuevoEstado != EstadoCotizacion.Rechazada)
                        throw new ModelException("Esta cotizacion solo puede ser aprobada o rechazada!");
                    break;
                }
                case EstadoCotizacion.Aprobada:
                {
                    if (nuevoEstado != EstadoCotizacion.Terminada && nuevoEstado != EstadoCotizacion.Rechazada)
                        throw new ModelException("Esta cotizacion solo puede ser terminada o rechazada!");
                    break;
                }
                case EstadoCotizacion.Terminada:
                {
                    throw new ModelException("Esta cotizacion no puede cambiar de estado!");
                }
            }

            //Si paso las reglas, actualizar.
            cotizacion.Estado = nuevoEstado;

            /*
             No se actualiza via el metodo Anadir de esta clase, ya que no se quiere anadir
             una nueva version de la cotizacion, si no, actualizar un dato de esta que
             solo les interesa a los usuarios del sistema y no a quien recibe la cotizacion.
             */
            
            //Actualizar cotizacion.
            _repositoryCotizacion.Add(cotizacion);
        }

        /// <inheritdoc />
        public Cotizacion BuscarCotizacion(string idCotizacion)
        {
            if (idCotizacion == null)
                throw new ModelException("El identificador ingresado fue nulo.");
            
            if (_repositoryCotizacion.GetAll().Count == 0)
            {
                throw new NullReferenceException("Repositorio de Cotizaciones se encuentra vacio");
            }
            
            //Obtener la cotizacion dado su identificador.
            Cotizacion cotizacion = _repositoryCotizacion.GetAll(c => c.Identificador == idCotizacion).FirstOrDefault();

            //No se encontro la cotizacion.
            if (cotizacion == null)
                throw new ModelException("No se encontro la cotizacion con el identificador ingresado.");
        
            //Se encontro; Retornar.
            return cotizacion;
        }
        
        public IList<Cotizacion> BuscarCotizaciones(string busqueda)
        {
            if (string.IsNullOrEmpty(busqueda))
                throw new ModelException("La busqueda no puede estar vacia!");

            if (_repositoryCotizacion.GetAll().Count == 0)
            {
                throw new NullReferenceException("Repositorio de Cotizaciones se encuentra vacio");
            }
            
            IList<Cotizacion> resultados = _repositoryCotizacion.GetAll(
                c => c.Titulo.Contains(busqueda) || c.Descripcion.Contains(busqueda) || 
                     c.Identificador.Contains(busqueda) || c.CostoTotal.ToString().Contains(busqueda) || 
                     Utils.ToFormatedDate(c.FechaCreacion).Contains(busqueda) || 
                     c.FechaCreacion.ToShortDateString().Contains(busqueda) || c.Estado.ToString().Contains(busqueda) ||
                     c.Cliente.Tipo.ToString().Contains(busqueda) || c.Cliente.Persona.Nombre.Contains(busqueda) ||
                     c.Cliente.Persona.Paterno.Contains(busqueda) || c.Cliente.Persona.Materno.Contains(busqueda) ||
                     c.Cliente.Persona.Rut.Contains(busqueda) || c.Cliente.Persona.Email.Contains(busqueda)
                     
            );
            
            return resultados;
        }
        
        /// <inheritdoc />
        public void EnviarCotizacion(Cotizacion cotizacionEnviar, string remitente, string emailPassword,
            string destinatario, MailMessage mailMessage)
        {
            if (String.IsNullOrEmpty(emailPassword))
            {
                throw new ModelException("La contrasena ingresada esta vacia.");
            }

            if (String.IsNullOrEmpty(remitente))
            {
                throw new ModelException("El Email del remitente esta vacio.");
            }

            if (String.IsNullOrEmpty(destinatario))
            {
                throw new ModelException("El Email del destinatario esta vacio.");
            }

            if (mailMessage == null)
            {
                throw new ModelException("El Email fue nulo.");
            }

            //Es necesario especificar el servidor!

            string servidor = null;

            //Si el remitente pertenece a los dominios de ucn.cl, usar servidor gmail.
            if (remitente.EndsWith("ucn.cl"))
            {
                servidor = "smtp.gmail.com";
            }

            //Si no, buscar en los servidores guardados.
            else
            {
                foreach (string server in Utils.SmtpServers)
                {
                    if (remitente.Contains(server))
                    {
                        servidor = "smtp." + server + ".com";
                        break;
                    }
                }
            }

            //Si no se pudo encontrar un servidor, lanzar excepcion.
            if (servidor == null)
                throw new SmtpException("El sistema no conoce el servidor remitente.");

            SmtpClient client = new SmtpClient(servidor, 587)
            {
                Credentials = new NetworkCredential(remitente, emailPassword),
                EnableSsl = true
            };

            mailMessage.From = new MailAddress(remitente);
            mailMessage.To.Add(destinatario);
            //mensaje.    IsBodyHtml = false;
            //mensaje.Body = "body";

            client.Send(mailMessage);
            
            //Si no lanzo excepcion al enviar el mensaje, cambiar el estado de la cotizacion a Enviada.
            cotizacionEnviar.Estado = EstadoCotizacion.Enviada;
        }

        /// <inheritdoc />
        public IList<Cotizacion> GetCotizaciones()
        {
            if (_repositoryCotizacion.GetAll().Count == 0)
            {
                throw new NullReferenceException("Repositorio de Cotizaciones se encuentra vacio");
            }
            else
            {
                return _repositoryCotizacion.GetAll();
            }
        }
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Usuario (OS_USXXX)
        //------------------------------------------------------------------------------

        /// <inheritdoc />
        public void Anadir(Persona persona, string password)
        {
            if (password == null)
            {
                throw new ModelException("La password no puede ser null.");
            }

            // Guardo o actualizo en el backend.
            Anadir(persona);

            // Busco si el usuario ya existe
            Usuario usuario = _repositoryUsuario.GetAll(u => u.Persona.Equals(persona)).FirstOrDefault();
            
            // Si no existe, lo creo
            if (usuario == null)
            {
                usuario = new Usuario()
                {
                    Persona = persona
                };
            }

            else
                throw new ModelException("El usuario ya existe.");
            
            // Hash del password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(password);
            
            // Almaceno en el backend
            _repositoryUsuario.Add(usuario);
        }

        //TODO: Metodo de prueba. Solo sirve para poder crear los usuarios de prueba al inicio de la App.
        /// <inheritdoc />
        public void Anadir(Usuario usuario)
        {
            // Guardo o actualizo en el backend.
            Anadir(usuario.Persona);
            
            // Busco si el usuario ya existe
            IList<Usuario> usuarios = _repositoryUsuario.GetAll(u => u.Persona.Rut.Equals(usuario.Persona.Rut));

            if (usuarios.Count > 1)
            {
                throw new ModelException("Esta Persona ya tiene un Usuario.");
            }
                        
            //(usuarios.Count == 0) -> No existe el usuario. Crearlo:

            // Hash del password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            
            // Almaceno en el backend
            _repositoryUsuario.Add(usuario);
        }

        public Usuario Login(string rutEmail, string password)
        {

            if (String.IsNullOrEmpty(password))
            {
                throw new ModelException("Password no puede ser null");
            }

            Persona persona = BuscarPersona(rutEmail);
            if (persona == null)
            {
                throw new ModelException("Usuario no encontrado");
            }
            
            //La persona, aunque sea la misma, sera distinta por el id de base entity.
            //Por eso usaremos el rut para comprobar.
            IList<Usuario> usuarios = _repositoryUsuario.GetAll(u => u.Persona.Rut.Equals(persona.Rut));
            if (usuarios.Count == 0)
            {
                throw new ModelException("Existe la Persona pero no tiene credenciales de acceso");
            }

            if (usuarios.Count > 1)
            {
                throw new ModelException("Mas de un usuario encontrado");
            }

            Usuario usuario = usuarios.Single();
            if (!BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                throw new ModelException("Contrasena incorrecta!");
            }

            return usuario;
        }

        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Persona (OS_PEXXX)
        //------------------------------------------------------------------------------
        
        /// <inheritdoc />
        public void Anadir(Persona persona)
        {
            // Verificacion de nulidad
            if (persona == null)
            {
                throw new ModelException("Persona es null");
            }

            // La validacion de los atributos ocurre en el repositorio.
            _repositoryPersona.Add(persona);
        }

        /// <inheritdoc />
        public Persona BuscarPersona(string rutEmail)
        {
            if (rutEmail == null)
                throw new ModelException("El rut o email ingresado fue nulo.");
            
            return _repositoryPersona.GetAll(p => p.Rut.Equals(rutEmail) || p.Email.Equals(rutEmail)).FirstOrDefault();
        }

        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Servicio (OS_SEXXX)
        //------------------------------------------------------------------------------

        /// <inheritdoc />
        public void Anadir(Servicio servicio, Cotizacion cotizacion)
        {
            if (servicio == null)
                throw new ModelException("El servicio ingresado fue nulo.");
            if (cotizacion == null)
                throw new ModelException("La cotizacion ingresada fue nula.");
            
            //Valido el servicio antes de anadirlo a la cotizacion:
            servicio.Validate();
            
            //Si la lista de servicios de la cotizacion es nula, inicializarla:
            if (cotizacion.Servicios == null)
                cotizacion.Servicios = new List<Servicio>();
            
            //Anadir servicio.
            cotizacion.Servicios.Add(servicio);
        }
        
        /// <inheritdoc />
        public void CambiarEstado(int indexServicio, Cotizacion cotizacion, EstadoServicio nuevoEstado)
        {
            /*
             * Reglas:
             * 1.- Un servicio empieza siempre como SinIniciar.
             * 2.- Un servicio SinIniciar solo puede cambiar a Rodaje o Cancelado.
             * 3.- Un servicio en rodaje solo puede cambiar a Postproduccion o Cancelado.
             * 4.- Un servicio en Postproduccion solo puede cambiar a revision o Cancelado.
             * 5.- Un servicio en Revision solo puede cambiar
             * 6.- Una cotizacion que esta terminada, no puede cambiar de estado.
             */
            
            switch (cotizacion.Servicios[indexServicio-1].Estado)
            {
                case EstadoServicio.SinIniciar:
                {
                    if (nuevoEstado != EstadoServicio.PreProduccion && nuevoEstado != EstadoServicio.Cancelado)
                        throw new ModelException("Este servicio solo puede cambiar a Preproduccion o ser Cancelado!");
                    break;
                }
                case EstadoServicio.PreProduccion:
                {
                    if (nuevoEstado != EstadoServicio.Rodaje && nuevoEstado != EstadoServicio.Cancelado)
                        throw new ModelException("Este servicio solo puede cambiar a Rodaje o ser Cancelado!");
                    break;
                }
                case EstadoServicio.Rodaje:
                {
                    if (nuevoEstado != EstadoServicio.PostProduccion && nuevoEstado != EstadoServicio.Cancelado)
                        throw new ModelException("Este servicio solo puede cambiar a Postproduccion o ser Cancelado!");
                    break;
                }
                case EstadoServicio.PostProduccion:
                {
                    if (nuevoEstado != EstadoServicio.Revision && nuevoEstado != EstadoServicio.Cancelado)
                        throw new ModelException("Este servicio solo puede cambiar a Revision o ser Cancelado!");
                    break;
                }
                case EstadoServicio.Revision:
                {
                    if (nuevoEstado != EstadoServicio.Entregado && nuevoEstado != EstadoServicio.Cancelado)
                        throw new ModelException("Este servicio solo puede ser Entregado o Cancelado!");
                    break;
                }
                case EstadoServicio.Entregado:
                {
                    throw new ModelException("Este servicio no puede cambiar de estado!");
                }
                case EstadoServicio.Cancelado:
                {
                    throw new ModelException("Este servicio no puede cambiar de estado!");
                }
            }

            //Si paso las reglas, actualizar.
            cotizacion.Servicios[indexServicio-1].Estado = nuevoEstado;

            /*
             No se actualiza via el metodo Anadir de esta clase, ya que no se quiere anadir
             una nueva version de la cotizacion, si no, actualizar un dato de esta que
             solo les interesa a los usuarios del sistema y no a quien recibe la cotizacion.
             */
            
            //Actualizar cotizacion.
            _repositoryCotizacion.Add(cotizacion);
        }

        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cliente (OS_CLXXX)
        //------------------------------------------------------------------------------

        /// <inheritdoc />
        public void Anadir(Persona persona, TipoCliente tipoCliente)
        {
            //Guardar o actualizar.
            Anadir(persona);
            
            //Verificar si existe el cliente.
            Cliente cliente = _repositoryCliente.GetAll(c => c.Persona.Equals(persona)).FirstOrDefault();
            
            //Crear el cliente si es que no existe:
            if (cliente == null)
            {
                cliente = new Cliente()
                {
                    Persona = persona,
                    Tipo = tipoCliente
                };
            }
            
            //Guardar cliente.
            _repositoryCliente.Add(cliente);
        }

        /// <inheritdoc />
        public Cliente BuscarCliente(string rut)
        {
            Persona persona = BuscarPersona(rut);

            if (persona == null)
                throw new ModelException("Cliente no encontrado.");

            //La persona existe.
            
            Cliente cliente = _repositoryCliente.GetAll(c => c.Persona.Equals(persona)).FirstOrDefault();

            if (cliente != null) return cliente;
            

            /*La persona existe, pero no esta registrada como cliente.
            Esto quiere decir que esa persona es uno de los usuarios que usan la aplicacion.*/
            Anadir(persona, TipoCliente.UnidadInterna);
            cliente = BuscarCliente(rut);
            
            //Ahora deberia retornar un cliente existente.
            return cliente;
        }
        
    }
}