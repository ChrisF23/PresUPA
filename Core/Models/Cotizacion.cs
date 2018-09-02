using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Cotizacion : BaseEntity
    {
        /// <summary>
        /// Identificador de la cotizacion, determinado por el numero de la cotizacion y su version.
        /// </summary>
        
        [Required]
        public string Identificador { get; set; }
        
        /// <summary>
        /// Numero de la cotizacion.
        /// </summary>
        [Required]
        public int? Numero { get; set; }

        /// <summary>
        /// Version de la cotizacion. Su valor por defecto es 1.
        /// </summary>
        [Required]
        public int? Version { get; set; }
        
        /// <summary>
        /// Titulo de la cotizacion.
        /// </summary>
        public string Titulo { get; set; }
        
        /// <summary>
        /// Descripcion breve del contexto de la cotizacion.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Fecha de creacion de esta cotizacion.
        /// </summary>
        //public DateTime fechaCreacion { get; set; }

        /// <summary>
        /// Rut del cliente al cual le es asignada esta cotizacion.
        /// </summary>
        public string FKRutCliente { get; set; }

        //public IList<Servicio> Servicios {get; set;}
        
        /// <summary>
        /// Costo total del la cotizacion.
        /// </summary>
        public int CostoTotal { get; set; }
        
        /// <summary>
        /// Estado en el cual se encuentra la cotizacion.
        /// </summary>
        public EstadoCotizacion Estado { get; set; }


        /// <summary>
        /// Calcula y asigna el costo total de esta cotizacion.
        /// </summary>
        public void AsignarServicios(IList<Servicio> servicios)
        {
            this.CostoTotal = 0;

            foreach (Servicio servicio in servicios)
            {
                servicio.FKIdentificadorCotizacion = this.Identificador;
                this.CostoTotal += (servicio.CostoUnidad*servicio.Cantidad);
            }
        }


        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
            if (Identificador == null)
            {
                throw new ModelException("El identificador no puede ser null.");
            }
            
            if (Numero == null)
            {
                throw new ModelException("El numero no puede ser null.");
            }
            
            if (Version == null)
            {
                throw new ModelException("La version no puede ser null.");
            }
            
            if (String.IsNullOrEmpty(Titulo))
            {
                throw new ModelException("El titulo no puede estar vacio.");
            }

            if (String.IsNullOrEmpty(Descripcion))
            {
                throw new ModelException("La descripcion no puede estar vacia.");
            }
            
            if (String.IsNullOrEmpty(FKRutCliente))
            {
                throw new ModelException("La cotizacion no tiene un cliente asignado.");
            }

            if (CostoTotal == 0)
            {
                throw new ModelException("La cotizacion no puede tener un costo total de $0.");
            }

            

        }
    }
   
    public enum EstadoCotizacion
    {
        Borrador,
        Enviada,
        Aprovada,
        Rechazada,
        Terminada
    }

}