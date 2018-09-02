using System.Collections.Generic;
using Core.Models;


namespace Core.Controllers
{
    public interface IServicio
    {

        void GuardarServicio(Servicio servicio);
        IList<Servicio> ObtenerServiciosPorIDCotizacion(string identificadorCotizacion);
    }
}