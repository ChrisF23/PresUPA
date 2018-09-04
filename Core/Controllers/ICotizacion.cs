using System.Collections.Generic;
using Core.Models;

namespace Core.Controllers
{
    public interface ICotizacion
    {
        /// <summary>
        /// OS_CO001: Almacena a una cotizacion en el sistema.
        /// </summary>
        /// <param name="cotizacion"></param>
        void GuardarCotizacion(Cotizacion cotizacion);
        
        /// <summary>
        /// Borra una cotizacion del sistema
        /// </summary>
        /// <param name="cotizacion"></param>
        void BorrarCotizacion(Cotizacion cotizacion);
        
        /// <summary>
        /// Edita los datos de una cotizacion
        /// </summary>
        /// <param name="cotizacion"></param>
        void EditarCotizacion(Cotizacion cotizacion);
        
        /// <summary>
        /// Cambia el estado de una cotizacion
        /// </summary>
        /// <param name="cotizacion"></param>
        /// <param name="nuevoEstado"></param>
        void CambiarEstadoCotizacion(Cotizacion cotizacion, EstadoCotizacion nuevoEstado);
        
        /// <summary>
        /// Buscar cotizacion almacenada en el sistema
        /// </summary>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        IList<Cotizacion> BuscarCotizacion(string busqueda);
        /*
        
        
        
        
        void EnviarCotizacion(Cotizacion cotizacion);
        void EnviarCotizacion(Cotizacion cotizacion, string emailDestino);
         */
        IList<Cotizacion> ObtenerCotizaciones();
       
    }
}