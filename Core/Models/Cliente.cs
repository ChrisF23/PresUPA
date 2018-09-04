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
        
       
        public override void Validate()
        {
            if (Persona == null)
            {
                throw new ModelException("Se requiere la Persona");
            }

            if (Tipo == null)
            {
                throw new ModelException("Se requiere un tipo asignado");
            }

        }
    }

    public enum TipoCliente
    {
        Otro,
        UnidadInterna
    }
}