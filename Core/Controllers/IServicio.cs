using System;
using System.Collections.Generic;
using Core.Models;


namespace Core.Controllers
{
    public interface IServicio
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicio"></param>
        void EditarServicio(Servicio servicio);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicio"></param>
        /// <param name="nuevoEstado"></param>
        void CambiarEstado(Servicio servicio, EstadoServicio nuevoEstado);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicio"></param>
        void BorrarServicio(Servicio servicio);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busqueda"></param>
        void BuscarServicio(String busqueda);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void DesplegarServicio(String id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificadorCotizacion"></param>
        /// <returns></returns>
        IList<Servicio> ObtenerServiciosPorIDCotizacion(string identificadorCotizacion);

}
}