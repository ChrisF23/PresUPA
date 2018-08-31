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

        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
        }
    }
}