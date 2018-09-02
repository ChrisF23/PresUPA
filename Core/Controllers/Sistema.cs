using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DAO;
using Core.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core.Controllers
{
    
    /// <summary>
    /// Implementacion de la interface ISistema.
    /// </summary>
    public sealed class Sistema : IPersona, IUsuario, ICotizacion, ICliente, IServicio
    {
        // Patron Repositorio, generalizado via Generics
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/
        private readonly IRepository<Persona> _repositoryPersona;

        private readonly IRepository<Usuario> _repositoryUsuario;

        private readonly IRepository<Cotizacion> _repositoryCotizacion;

        private readonly IRepository<Cliente> _repositoryCliente;
        
        private readonly IRepository<Servicio> _repositoryServicio;
        
        
        private int LastCotizacionNumber;
        
        /// <summary>
        /// Inicializa los repositorios internos de la clase.
        /// </summary>
        public Sistema(
            IRepository<Persona> repositoryPersona, 
            IRepository<Usuario> repositoryUsuario, 
            IRepository<Cotizacion> repositoryCotizacion,
            IRepository<Cliente> repositoryCliente,
            IRepository<Servicio> repositoryServicio)
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
            _repositoryServicio = repositoryServicio ??
                                 throw new ArgumentNullException("Se requiere repositorio de servicios");
            
            // Inicializacion del repositorio.
            _repositoryPersona.Initialize();
            _repositoryUsuario.Initialize();
            _repositoryCotizacion.Initialize();
            _repositoryCliente.Initialize();
            _repositoryServicio.Initialize();
            
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

        /*
         * Operaciones de Sistema - Persona.
         */
        
        /// <inheritdoc />
        public void Save(Persona persona)
        {
            // Verificacion de nulidad
            if (persona == null)
            {
                throw new ModelException("Persona es null");
            }
            
            // Saving the Persona en el repositorio.
            // La validacion de los atributos ocurre en el repositorio.
            _repositoryPersona.Add(persona);
        }
        
        /// <inheritdoc />
        public IList<Persona> GetPersonas()
        {
            return _repositoryPersona.GetAll();
        }
        
        /// <inheritdoc />
        public Persona Find(string rutEmail)
        {
            return _repositoryPersona.GetAll(p => p.Rut.Equals(rutEmail) || p.Email.Equals(rutEmail)).FirstOrDefault();
        }
        
        /*
         * Operaciones de Sistema - Usuario.
         */
        
        /// <inheritdoc />
        public void Save(Persona persona, string password)
        {
            
            // Guardo o actualizo en el backend.
            Save(persona);

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
        
        /// <inheritdoc />
        public Usuario Login(string rutEmail, string password)
        {
            Persona persona = Find(rutEmail);
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

        /*
         * Operaciones de Sistema - Cliente.
         */
        
        public void GuardarCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ModelException("Cliente es null");
            }
            
            _repositoryCliente.Add(cliente);
            
        }
        
        /*
         * Operaciones de Sistema - Cotizacion.
         */
        
        /// <inheritdoc />
        public void GuardarCotizacion(Cotizacion cotizacion)
        {
            // Verificacion de nulidad
            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion es null");
            }

            //Si el numero de la cotizacion es null, entonces es una nueva cotizacion.
            //Si no lo es, entonces se conserva.
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
            
            //Calcula el costo total de la cotizacion.
            cotizacion.CalcularCostoTotal();
            
            //Asigna los servicios a la cotizacion.
            cotizacion.AsignarServicios();
            
            //Asigna el estado por defecto (borrador = 0).
            cotizacion.Estado = EstadoCotizacion.Borrador;
            
            _repositoryCotizacion.Add(cotizacion);
        }

        public IList<Cotizacion> ObtenerCotizaciones()
        {
            return _repositoryCotizacion.GetAll();
        }

        
        /*
         * Operaciones de Sistema - Servicio
         */
        
        public IList<Servicio> ObtenerServiciosPorIDCotizacion(string identificadorCotizacion)
        {
            return _repositoryServicio.GetAll(s => s.IdentificadorCotizacion.Equals(identificadorCotizacion)).ToList();
        }

        public void GuardarServicio(Servicio servicio)
        {
            if (servicio == null)
                throw new ModelException("El servicio es null");
            
            _repositoryServicio.Add(servicio);
        }
    }
}