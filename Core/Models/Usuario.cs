using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    /// <summary>
    /// Usuario del sistema
    /// </summary>
    public class Usuario : BaseEntity
    {
        /// <summary>
        /// Persona que representa a este usuario
        /// </summary>
        [Required]
        public Persona Persona { get; set; }

        /// <summary>
        /// Contrasenia de acceso de la Persona
        /// </summary>
        [Required]
        public string Password { get; set; }
        
        /// <summary>
        /// Tipo de usuario representa los diferentes permisos
        /// que tiene en el sistema
        /// </summary>
        public TipoUsuario Tipo { get; set; }

        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
            if (Persona == null)
            {
                throw new ModelException("Se requiere la Persona");
            }

            if (String.IsNullOrEmpty(Password))
            {
                throw new ModelException("Se requiere el Password");
            }

            if (Tipo == null)
            {
                throw new ModelException("Se requiere un tipo asignado");
            }
        }
    }
    
    /// <summary>
    /// Enumeracion que contiene los tipos de usuarios
    /// </summary>
    public enum TipoUsuario
    {
        Director,
        Productor,
        Supervisor
    }
}