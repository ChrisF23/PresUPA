using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Inicializa los repositorios internos de la clase.
        /// </summary>
        public Sistema(IRepository<Persona> repositoryPersona)
        {
            // Setter!
            _repositoryPersona = repositoryPersona ?? throw new ArgumentException("Se requiere el repositorio de personas");

            // Inicializacion del repositorio.
            _repositoryPersona.Initialize();
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
    }
}