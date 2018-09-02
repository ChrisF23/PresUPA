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
        
        /*
        void BorrarCotizacion(Cotizacion cotizacion);
        void EditarCotizacion(Cotizacion cotizacion);
        void CambiarEstadoCotizacion(Cotizacion cotizacion, EstadoCotizacion nuevoEstado);
        IList<Cotizacion> BuscarCotizacion(string busqueda);
        void EnviarCotizacion(Cotizacion cotizacion);
        void EnviarCotizacion(Cotizacion cotizacion, string emailDestino);
        IList<Cotizacion> ObtenerCotizaciones();
        */
    }
}