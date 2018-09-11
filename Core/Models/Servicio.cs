using System;

namespace Core.Models
{    
    /// <summary>
    /// Clase que representa un Servicio en una Cotizacion.
    /// </summary>
    public class Servicio : BaseEntity
    {
        /// <summary>
        /// Descripcion del servicio 
        /// </summary>
        public string Descripcion { get; set; }
        
        /// <summary>
        /// Costo precio unitario del servicio
        /// </summary>
        public int CostoUnidad { get; set; }
        
        /// <summary>
        /// Cantidad de veces que se presta el servicio
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Estado de progreso del servicio
        /// </summary>
        public EstadoServicio Estado { get; set; }
        
        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
            
            if (String.IsNullOrEmpty(Descripcion))
            {
                throw new ModelException("La descripcion no puede estar vacia.");
            }
            
            if (CostoUnidad < 0)
            {
                throw new ModelException("El costo del servicio no puede ser un valor negativo.");
            }

            if (CostoUnidad == 0)
            {
                throw new ModelException("El costo del servicio no puede ser 0");
            }

            if (Cantidad < 0)
            {
                throw new ModelException("La cantidad del servicio no puede ser negativo.");
            }

            if (Cantidad == 0)
            {
                throw new ModelException("La cantidad del servicio debe ser al menos 1");
            }
            

        }

        /// <summary>
        /// Retorna la representacion del objeto Servicio a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                "Descripcion: " + Descripcion + "\n"
                + "Cantidad: " + Cantidad + "\n"
                + "Costo unidad: $" + CostoUnidad + "\n"
                + "Sub total: $" + (CostoUnidad*Cantidad);
        }

        /// <summary>
        /// Retorna un resumen de la representacion del objeto Servicio a string.
        /// </summary>
        /// <returns></returns>
        public string ToStringBrief()
        {
            return "Descripcion: " + Descripcion + "\n"
                   + "Estado: " + Estado;
        }
    }
    
    /// <summary>
    /// Enumeracion que contiene los estados de un servicio
    /// </summary>
    public enum EstadoServicio
    {
        SinIniciar,
        PreProduccion,
        Rodaje,
        PostProduccion,
        Revision,
        Entregado,
        Cancelado
    }
  
}