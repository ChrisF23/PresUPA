using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Cotizacion : BaseEntity
    {
        /// <summary>
        /// Identificador de la cotizacion.
        /// </summary>
        public string Identificador { get; set; }

        /// <summary>
        /// Version de la cotizacion.
        /// </summary>
        public string Version { get; set; }
        
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
        public DateTime fechaCreacion { get; set; }

        /// <summary>
        /// Rut del cliente al cual le es asignada esta cotizacion.
        /// </summary>
        public string RutCliente { get; set; }

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
        /// <param name="costosServicios"></param>
        public void AsignarServicios(IList<int> servicios)
        {
            this.CostoTotal = 0;

            foreach (int servicio in servicios)
            {
                //servicio.idCotizacion = this.identificador;
                this.CostoTotal += servicio;
            }
        }


        /// <inheritdoc cref="BaseEntity.Validate"/>
        public override void Validate()
        {
            if (String.IsNullOrEmpty(Version))
            {
                throw new ModelException("Version no puede ser null");
            }
            
            if (String.IsNullOrEmpty(Titulo))
            {
                throw new ModelException("El titulo no puede estar vacio.");
            }

            if (String.IsNullOrEmpty(Descripcion))
            {
                throw new ModelException("La descripcion no puede estar vacia.");
            }
            
        }
    }
    
    public enum EstadoCotizacion
    {
        Borrador,
        Enviada,
        Aprobada,
        Rechazada,
        Terminada
    };

}