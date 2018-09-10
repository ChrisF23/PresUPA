using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Models
{
    /// <summary>
    /// Clase que representa una cotizacion
    /// </summary>
    /// <remarks>
    /// Contiene informacion sobre una cotizacion de acuerdo al dominio
    /// </remarks>
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
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Cliente al cual le es asignada esta cotizacion.
        /// </summary>
        public Cliente Cliente { get; set; }
        
        /// <summary>
        /// Lista de los servicios que conforman la cotizacion
        /// </summary>
        public IList<Servicio> Servicios {get; set;}
        
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
        public void CalcularMiCostoTotal()
        {
            CostoTotal = 0;

            if (Servicios != null)
            {
                foreach (Servicio servicio in Servicios)
                    CostoTotal += servicio.CostoUnidad * servicio.Cantidad;
            }
        }

        private void ValidarMisServicios()
        {
            foreach (Servicio servicio in Servicios)
                servicio.Validate();
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
            
            if (Cliente == null)
            {
                throw new ModelException("La cotizacion no tiene un cliente asignado.");
            }

            if (Servicios == null)
            {
                throw new ModelException("La cotizacion no tiene servicios asignados.");
            }

            ValidarMisServicios();
            
            CalcularMiCostoTotal();
            
            if (CostoTotal <= 0)
            {
                throw new ModelException("La cotizacion debe tener un costo total superior a $0.");
            }
            
        }
        
        public override string ToString()
        {
            return
                "Identificador: " + Identificador + "\n"
                + "Titulo: " + Titulo + "\n"
                + "Descripcion: " + Descripcion + "\n"
                + "Fecha de Creacion: " + FechaCreacion + "\n"
                + "Cliente: " + Cliente.ToString() + "\n"
                + "Servicios: " + MisServiciosToString() + "\n"
                + "Costo Total: $" + CostoTotal;
        }

        public string MisServiciosToString()
        {
            string ts = "";
            if (Servicios != null)
            {
                int counter = 0;
                foreach (Servicio servicio in Servicios)
                {
                    ts = String.Concat(ts, "\nServicio ", ++counter, ":\n");
                    ts = String.Concat(ts, servicio.ToString(), "\n");
                }
            }

            return ts;
        }

    }
    /// <summary>
    /// Enumeracion con los distintos estados de la cotizacion
    /// </summary>
    public enum EstadoCotizacion
    {
        Borrador,
        Enviada,
        Aprovada,
        Rechazada,
        Terminada
    }

}