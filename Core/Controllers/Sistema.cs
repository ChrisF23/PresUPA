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
            
            //Calcula el costo total de la cotizacion.
            cotizacion.CalcularCostoTotal();
            
            //Asigna los servicios a la cotizacion.
            cotizacion.AsignarServicios();
            
            //Asigna el estado por defecto (borrador = 0).
            cotizacion.Estado = EstadoCotizacion.Borrador;
            
            _repositoryCotizacion.Add(cotizacion);
        }

        public void Borrar(string idCotizacion)
        {
            throw new NotImplementedException();
            //_repositoryCotizacion.Remove(cotizacion);
        }

        public void Editar(string idCotizacion)
        {
            throw new NotImplementedException();
        }

        public void CambiarEstado(string idCotizacion, EstadoCotizacion nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Cotizacion BuscarCotizacion(string idCotizacion)
        {
            throw new NotImplementedException();
        }

       

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

        //TODO: FIX ME.
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

        public IList<Persona> GetPersonas()
        {
            return _repositoryPersona.GetAll();
        }

        public Persona BuscarPersona(string rutEmail)
        {
            return _repositoryPersona.GetAll(p => p.Rut.Equals(rutEmail) || p.Email.Equals(rutEmail)).FirstOrDefault();
        }

        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Servicio (OS_SEXXX)
        //------------------------------------------------------------------------------

        
        public void Anadir(Servicio servicio, string idCotizacion)
        {
            throw new NotImplementedException();
        }

        public void Anadir(List<Servicio> servicios, string idCotizacion)
        {
            throw new NotImplementedException();
        }

        public void EditarServicio(Servicio servicio)
        {
            throw new NotImplementedException();
        }

        public void CambiarEstado(int index, EstadoServicio nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public void Borrar(int index, string idCotizacion)
        {
            throw new NotImplementedException();
        }

        public void Desplegar(int index, string idCotizacion)
        {
            throw new NotImplementedException();
        }

        public void DesplegarTodos(string idCotizacion)
        {
            throw new NotImplementedException();
        }

        public IList<Servicio> GetServicios(string idCotizacion)
        {
            throw new NotImplementedException();
        }

        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cliente (OS_CLXXX)
        //------------------------------------------------------------------------------

        
        public void Anadir(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ModelException("Cliente es null");
            }
            
            _repositoryCliente.Add(cliente);
        }

        public void Buscar(string rutEmail)
        {
            throw new NotImplementedException();
        }
        
        public void Desplegar(string rut)
        {
            throw new NotImplementedException();
        }

        IList<Cliente> ISistema.GetClientes()
        {
            throw new NotImplementedException();
        }
        
    }
}