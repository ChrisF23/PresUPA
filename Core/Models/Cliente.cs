using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Cliente : BaseEntity
    {

        public Persona Persona { get; set; }
        
        public TipoCliente Tipo { get; set; }
        
        public override void Validate()
        {
            if (Persona == null)
            {
                throw new ModelException("Se requiere la Persona");
            }

        }
    }

    public enum TipoCliente
    {
        Otro,
        UnidadInterna
    }
}