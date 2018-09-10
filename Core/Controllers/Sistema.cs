using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore.Internal;
//TODO : Implementar las operaciones restantes y verificar la funcionalidad
namespace Core.Controllers
{
    
    /// <summary>
    /// Implementacion de la interface ISistema.
    /// </summary>
    public sealed class Sistema : ISistema
    {
        // Patron Repositorio, generalizado via Generics
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/
        private readonly IRepository<Persona> _repositoryPersona;

        private readonly IRepository<Usuario> _repositoryUsuario;

        private readonly IRepository<Cotizacion> _repositoryCotizacion;

        private readonly IRepository<Cliente> _repositoryCliente;
        
        
        
        private int LastCotizacionNumber;
        
        /// <summary>
        /// Inicializa los repositorios internos de la clase.
        /// </summary>
        public Sistema(
            IRepository<Persona> repositoryPersona, 
            IRepository<Usuario> repositoryUsuario, 
            IRepository<Cotizacion> repositoryCotizacion,
            IRepository<Cliente> repositoryCliente)
        {
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
                LastCotizacionNumber = numero ?? 259;

            }
            else
            {
                //La lista no contiene elementos -> Usar 259 por defecto.
                LastCotizacionNumber = 259;
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
                cotizacion.Numero = ++LastCotizacionNumber;
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

        //TODO Metodo que permite asignar un nuevo identificador a la cotizacion.
        /// <inheritdoc />
        public void Editar(Cotizacion cotizacion)
        {
            //Se recibe por parametro la cotizacion ya editada.
            //Si la cotizacion recibida es distinta a la obtenida por su identificador,
            //Se procede a agregar la nueva version en la base de datos.

            Cotizacion comparar = BuscarCotizacion(cotizacion.Identificador);
            
            if (Validate.CompararCotizaciones(cotizacion, comparar))
            {
                Console.WriteLine("No hay cambios");
                return;
            }
            
            Console.WriteLine("Cambios detectados");

            Anadir(cotizacion);
        }

        /// <inheritdoc />
        public void CambiarEstado(string idCotizacion, EstadoCotizacion nuevoEstado)
        {
            //Verificacion de nulidad de idCotizacion se realiza en BuscarCotizacion.
            Cotizacion cotizacion = BuscarCotizacion(idCotizacion);
            //Enums nunca son nulos.
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
            
            //Obtener la cotizacion dado su identificador.
            Cotizacion cotizacion = _repositoryCotizacion.GetAll(c => c.Identificador == idCotizacion).FirstOrDefault();

            //No se encontro la cotizacion.
            if (cotizacion == null)
                throw new ModelException("No se encontro la cotizacion con el identificador ingresado.");
        
            //Se encontro; Retornar.
            return cotizacion;
        }

       
        /// <inheritdoc />
        IList<Cotizacion> ISistema.GetCotizaciones()
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
            // Guardo o actualizo en el backend.
            Anadir(persona);

            // Busco si el usuario ya existe
            Usuario usuario = _repositoryUsuario.GetAll(u => u.Persona.Equals(persona)).FirstOrDefault();
            
            // Si no existe, lo creo
            if (usuario == null)
            {
                usuario = new Usuario()
                {
                    Persona =  persona
                };
            }
            
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
            var usuarioCopy = usuario;
            Usuario usuario2 = _repositoryUsuario.GetAll(u => u.Persona.Equals(usuarioCopy.Persona)).FirstOrDefault();
            
            // Si no existe, lo creo
            if (usuario2 != null)
            {
                throw new ModelException("Esta persona ya tiene una cuenta.");
            }
            
            // Hash del password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            
            // Almaceno en el backend
            _repositoryUsuario.Add(usuario);
        }

        public Usuario Login(string rutEmail, string password)
        {

            Persona persona = BuscarPersona(rutEmail);
            if (persona == null)
            {
                throw new ModelException("Usuario no encontrado");
            }
            
            IList<Usuario> usuarios = _repositoryUsuario.GetAll(u => u.Persona.Equals(persona));
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
                throw new ModelException("Password no coincide");
            }

            return usuario;
        }

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
        public IList<Persona> GetPersonas()
        {
            return _repositoryPersona.GetAll();
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
        public void EditarServicio(Servicio servicio)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc />
        public void CambiarEstado(int index, EstadoServicio nuevoEstado)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Borrar(int index, string idCotizacion)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Desplegar(int index, string idCotizacion)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DesplegarTodos(string idCotizacion)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IList<Servicio> GetServicios(string idCotizacion)
        {
            throw new NotImplementedException();
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
        
        /// <inheritdoc />
        public void Desplegar(string rut)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        IList<Cliente> ISistema.GetClientes()
        {
            throw new NotImplementedException();
        }
        
    }
}