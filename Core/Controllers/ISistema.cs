using System.Collections.Generic;
using System.Net.Mail;
using Core.Models;

namespace Core.Controllers
{
    /// <summary>
    /// Interface del Sistema. Contiene los contratos de las Operaciones de Sistema.
    /// </summary>
    public interface ISistema
    {
        /// <summary>
        /// Email por defecto para enviar correos.
        /// </summary>
        string EmailUpa { get; }
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cotizaciones (OS_COXXX)
        //------------------------------------------------------------------------------

        /// <summary>
        /// OS_CO001: Almacena a una cotizacion en el sistema.
        /// </summary>
        /// <param name="cotizacion">La cotizacion a anadir.</param>
        void Anadir(Cotizacion cotizacion);
        
        /// <summary>
        /// OS_CO002: Borra una cotizacion del sistema.
        /// </summary>
        /// <param name="idCotizacion">El nuevo estado de la cotizacion.</param>
        void Borrar(string idCotizacion);
        
        /// <summary>
        /// OS_CO003: Si la cotizacion fue editada, anade una nueva version al sistema.
        /// </summary>
        /// <param name="cotizacion">Una copia editada de la cotizacion original.</param>
        void Editar(Cotizacion cotizacion);
        
        /// <summary>
        /// OS_CO004: Cambia el estado de una cotizacion, siguiendo ciertas reglas especificadas en la implementacion.
        /// </summary>
        /// <param name="idCotizacion">El identificador de la cotizacion.</param>
        /// <param name="nuevoEstado">El nuevo estado de la cotizacion.</param>
        void CambiarEstado(string idCotizacion, EstadoCotizacion nuevoEstado);
        
        /// <summary>
        /// OS_CO005: Obtiene la cotizacion que tenga el identificador entregado.
        /// </summary>
        /// <param name="idCotizacion">El identificador de la cotizacion.</param>
        /// <returns></returns>
        Cotizacion BuscarCotizacion(string idCotizacion);

        /// <summary>
        /// OS_CO006: Dado el string busqueda, realiza una busqueda de cotizaciones almacenadas
        /// en el sistema y retorna todas las coincidencias.
        /// [Incompleto: la busqueda es Case Sensitive...]
        /// </summary>
        /// <param name="busqueda">string a buscar.</param>
        /// <returns></returns>
        IList<Cotizacion> BuscarCotizaciones(string busqueda);
        
        /// <summary>
        /// OS_CO007: Envia una cotizacion al Email del cliente que esta asignado a dicha cotizacion.
        /// </summary>
        /// <param name="cotizacionEnviar">La cotizacion a enviar.</param>
        /// <param name="remitente">El Email del remitente.</param>
        /// <param name="emailPassword">La contrasena del Email del remitente.</param>
        /// <param name="destinatario">El Email del destinatario.</param>
        /// <param name="mailMessage">El mensaje de correo a enviar.</param>
        void EnviarCotizacion(Cotizacion cotizacionEnviar, string remitente, string emailPassword, string destinatario, MailMessage mailMessage);

        /// <summary>
        /// OS_CO008: Obtiene todas las cotizaciones almacenadas en el sistema.
        /// </summary>
        /// <returns></returns>
        IList<Cotizacion> GetCotizaciones();

        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Usuario (OS_USXXX)
        //------------------------------------------------------------------------------
        
        /// <summary>
        /// OS_US001: Guarda a un usuario en el sistema, dada la Persona y Contrasena.
        /// </summary>
        /// <param name="persona">La persona que representa al Usuario.</param>
        /// <param name="password">La contrasena del Usuario.</param>
        void Anadir(Persona persona, string password);

        /// <summary>
        /// OS_US001b: Guarda a un usuario (precreado) en el sistema.
        /// [Solo con motivos de pruebas]
        /// </summary>
        /// <param name="cotizacion">Cotizacion a anadir.</param>
        void Anadir(Usuario cotizacion);
        
        /// <summary>
        /// OS_US002: Dados email y password, Verifica si el usuario existe y lo retorna.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <param name="password">Contrasenia de acceso al sistema</param>
        /// <returns></returns>
        Usuario Login(string rutEmail, string password);
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Persona (OS_PEXXX)
        //------------------------------------------------------------------------------
        
        /// <summary>
        /// OS_PE001: Almacena una persona en el sistema.
        /// </summary>
        /// <param name="persona">Persona a guardar en el sistema.</param>
        void Anadir(Persona persona);

        /// <summary>
        /// OS_PE002: Busqueda de una persona por rut o correo electronico.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <returns>La persona si existe</returns>
        Persona BuscarPersona(string rutEmail);
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Servicio (OS_SVXXX)
        //------------------------------------------------------------------------------

        /// <summary>
        /// OS_SV001: Guarda un servicio en la cotizacion.
        /// </summary>
        /// <param name="servicio">El servicio a anadir.</param>
        /// <param name="cotizacion">La cotizacion a la cual anadir el servicio.</param>
        void Anadir(Servicio servicio, Cotizacion cotizacion);
    
        /// <summary>
        /// OS_SV002: Cambia el estado de un servicio, siguiendo ciertas reglas especificadas en la implementacion.
        /// </summary>
        /// <param name="indexServicio">El indice del servicio (+1) en la lista de servicios.</param>
        /// <param name="cotizacion">La cotizacion que contiene el servicio.</param>
        /// <param name="nuevoEstado">El nuevo estado del servicio.</param>
        void CambiarEstado(int indexServicio, Cotizacion cotizacion, EstadoServicio nuevoEstado);
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cliente (OS_CLXXX)
        //------------------------------------------------------------------------------

        /// <summary>
        /// OS_CL001: Anade un nuevo cliente al sistema.
        /// </summary>
        /// <param name="persona">Persona que representa al cliente.</param>
        /// <param name="tipoCliente">El tipo de cliente.</param>
        void Anadir(Persona persona, TipoCliente tipoCliente);
       
        /// <summary>
        /// OS_CL002: Busca al cliente segun su rut.
        /// [Agiliza la insercion de un cliente a una cotizacion]
        /// </summary>
        /// <param name="rut">Rut del cliente a buscar.</param>
        /// <returns></returns>
        Cliente BuscarCliente(string rut);
        
    }
}