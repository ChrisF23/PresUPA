using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    /// <summary>
    /// Clase que representa una Persona en el sistema de presupuesto.
    /// </summary>
    public class Persona : BaseEntity
    {
        /// <summary>
        /// Identificador unico.
        /// </summary>
        [Required]
        public string Rut { get; set; }

        /// <summary>
        /// Primer y segundo (optativo) nombre de la persona.
        /// </summary>
        [Required]
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido paterno.
        /// </summary>
        [Required]
        public string Paterno { get; set; }

        /// <summary>
        /// Apellido materno.,
        /// </summary>
        public string Materno { get; set; }

        /// <summary>
        /// Correo Electronico de la persona
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
            if (String.IsNullOrEmpty(Rut))
            {
                throw new ModelException("Rut no puede ser null");
            }

            // Validacion del RUT
            Models.Validate.ValidarRut(Rut);

            if (String.IsNullOrEmpty(Nombre))
            {
                throw new ModelException("Nombre no puede ser null o vacio");
            }

            if (String.IsNullOrEmpty(Paterno))
            {
                throw new ModelException("Apellido Paterno no puede ser null o vacio");
            }

            if (String.IsNullOrEmpty(Email))
            {
                throw new ModelException("Email no puede ser null o vacio");
            }
        }
    }
}