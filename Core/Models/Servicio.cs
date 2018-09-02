using System;

namespace Core.Models
{
    public class Servicio : BaseEntity
    {
        public string IdentificadorCotizacion { get; set; }

        public string Descripcion { get; set; }

        public int CostoUnidad { get; set; }

        public int Cantidad { get; set; }


        public override void Validate()
        {
            
            if (String.IsNullOrEmpty(IdentificadorCotizacion))
            {
                throw new ModelException("Este servicio no tiene asignado un identificador de cotizacion.");
            }
            
            if (String.IsNullOrEmpty(Descripcion))
            {
                throw new ModelException("La descripcion no puede estar vacia.");
            }
            
            if (CostoUnidad <= 0)
            {
                throw new ModelException("El costo del servicio debe ser mayor a 0.");
            }

            if (Cantidad <= 0)
            {
                throw new ModelException("La cantidad del servicio debe ser al menos 1.");
            }
        }
    }
}