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
    public sealed class Sistema : IPersona, IUsuario, ICotizacion, ICliente, IServicio
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

        public void BuscarCliente(string busqueda)
        {
            throw new NotImplementedException();
        }

        public void DesplegarCliente(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Cliente> ObtenerClientes()
        {
            throw new NotImplementedException();
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

        /// <inheritdoc />
        public void BorrarCotizacion(Cotizacion cotizacion)
        {
            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion debe existir para ser eliminada");
            }
                _repositoryCotizacion.Remove(cotizacion);
        }

        /// <inheritdoc />
        public void EditarCotizacion(Cotizacion cotizacion)
        {
            if (cotizacion == null)
            {
                throw new ModelException("La cotizacion no debe ser null");
            }
            
            Cotizacion cotizacionEdit=
            _repositoryCotizacion.GetById(cotizacion.Id);

            if (cotizacionEdit == null)
            {
                throw new NullReferenceException("La Cotizacion a buscar no existe en el sistema");
            }
               
            Console.WriteLine("Modificar Titulo de la Cotizacion");
            cotizacionEdit.Titulo = Console.ReadLine();
            
            Console.WriteLine("Modificar Descripcion de la cotizacion");
            cotizacionEdit.Descripcion = Console.ReadLine();
        }

        /// <inheritdoc />
        public void CambiarEstadoCotizacion(Cotizacion cotizacion, EstadoCotizacion nuevoEstado)
        {
            if (cotizacion == null)
            {
                throw new ArgumentNullException("La cotizacion es null");
            }

            Cotizacion cotizacionEdit =
                _repositoryCotizacion.GetById(cotizacion.Id);
            
            if (cotizacionEdit == null)
            {
                throw new NullReferenceException("La Cotizacion a buscar no existe en el sistema");
            }

            cotizacionEdit.Estado = nuevoEstado;

        }

        public IList<Cotizacion> BuscarCotizacion(string busqueda)
        {
            if (String.IsNullOrEmpty(busqueda))
            {
                throw new ArgumentNullException("El string de busqueda es null");
            }
           
            
            //List que contiene las cotizaciones que tienen alguna coincidencia
            IList<Cotizacion> resultCotizacion= null;
            
            // Se asume que es un rut
            if (int.TryParse(busqueda, out int n))
            {
                return resultCotizacion = 
                    _repositoryCotizacion.GetAll(
                        c => c.Cliente.Persona.Rut.Contains(busqueda));
            }
            
            // Se asume que es un email
            if (busqueda.Contains('@'))
            {
                return resultCotizacion = 
                    _repositoryCotizacion.GetAll(
                        c => c.Cliente.Persona.Email.Contains(busqueda));
            }
            
           //Busqueda se encuentra en el titulo o Descripcion
            return resultCotizacion = 
                _repositoryCotizacion.GetAll(
                    ct => ct.Titulo.Contains(busqueda) || ct.Descripcion.Contains(busqueda) );
  
        }



        /// <inheritdoc />
        public IList<Cotizacion> ObtenerCotizaciones()
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
        
        
        
        
        
        /*
         * Operaciones de Sistema - Servicio.
         */
        
        /// <inheritdoc />
        public void GuardarServicio(Servicio servicio)
        {
            if (servicio == null)
            {
                throw new ModelException("El servicio es null");
            }
            
            
        }

        public void AñadirServicio(Servicio servicio)
        {
            throw new NotImplementedException();
        }

        public void EditarServicio(Servicio servicio)
        {
            throw new NotImplementedException();
        }

        public void CambiarEstado(Servicio servicio, EstadoServicio nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public void BorrarServicio(Servicio servicio)
        {
            throw new NotImplementedException();
        }

        public void BuscarServicio(string busqueda)
        {
            throw new NotImplementedException();
        }

        public void DesplegarServicio(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Servicio> ObtenerServiciosPorIDCotizacion(string identificadorCotizacion)
        {
            throw new NotImplementedException();
        }
    }
}