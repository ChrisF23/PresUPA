using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{    
    /// <summary>
    /// Clase que representa un cliente
    /// </summary>
    /// <remarks>
    /// Esta clase se encuentra en el modelo de dominio
    /// y representa al cliente en cada cotizacion
    /// </remarks>>
    public class Cliente : BaseEntity
    {
        /// <summary>
        /// Persona que representa al cliente
        /// </summary>
        public Persona Persona { get; set; }
        
        /// <summary>
        /// Tipo de cliente asignado por el enum TipoCliente
        /// </summary>
        public TipoCliente Tipo { get; set; }
        
        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
            if (Persona == null)
            {
                throw new ModelException("Se requiere la Persona");
            }
        }

        public override string ToString()
        {
            return Persona.ToString() +
                   "\nTipo: " + Tipo;
        }

    }

    public enum TipoCliente
    {
        Otro,
        UnidadInterna
    }
}