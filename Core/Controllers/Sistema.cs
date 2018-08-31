using System;
using System.Collections.Generic;
using System.Linq;
using Core.DAO;
using Core.Models;

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

        /// <summary>
        /// Inicializa los repositorios internos de la clase.
        /// </summary>
        public Sistema(IRepository<Persona> repositoryPersona, IRepository<Usuario> repositoryUsuario)
        {
            // Setter!
            _repositoryPersona = repositoryPersona ??
                                 throw new ArgumentNullException("Se requiere el repositorio de personas");
            _repositoryUsuario = repositoryUsuario ??
                                 throw new ArgumentNullException("Se requiere repositorio de usuarios");

            // Inicializacion del repositorio.
            _repositoryPersona.Initialize();
            _repositoryUsuario.Initialize();
        }

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
        public void Save(Persona persona, string password)
        {
            // Guardo o actualizo en el backend.
            _repositoryPersona.Add(persona);

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

        /// <inheritdoc />
        public Persona Find(string rutEmail)
        {
            return _repositoryPersona.GetAll(p => p.Rut.Equals(rutEmail) || p.Email.Equals(rutEmail)).FirstOrDefault();
        }
    }
}