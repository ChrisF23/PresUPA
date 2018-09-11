using System.Collections.Generic;
using System.Net.Mail;
using Core.Models;

namespace Core.Controllers
{
    public interface ISistema
    {
        string EmailUpa { get; }
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cotizaciones (OS_COXXX)
        //------------------------------------------------------------------------------

        /// <summary>
        /// OS_CO001: Almacena a una cotizacion en el sistema.
        /// </summary>
        /// <param name="cotizacion"></param>
        void Anadir(Cotizacion cotizacion);
        
        /// <summary>
        /// OS_CO002: Borra una cotizacion del sistema
        /// </summary>
        /// <param name="idCotizacion"></param>
        void Borrar(string idCotizacion);
        
        /// <summary>
        /// OS_CO003: Edita los datos de una cotizacion
        /// </summary>
        /// <param name="cotizacion"></param>
        void Editar(Cotizacion cotizacion);
        
        /// <summary>
        /// OS_CO004: Cambia el estado de una cotizacion
        /// </summary>
        /// <param name="idCotizacion"></param>
        /// <param name="nuevoEstado"></param>
        void CambiarEstado(string idCotizacion, EstadoCotizacion nuevoEstado);
        
        /// <summary>
        /// OS_CO005: Buscar cotizacion almacenada en el sistema
        /// </summary>
        /// <param name="idCotizacion"></param>
        /// <returns></returns>
        Cotizacion BuscarCotizacion(string idCotizacion);



        IList<Cotizacion> BuscarCotizaciones(string busqueda);
       
        /*
        void EnviarCotizacion(Cotizacion cotizacion);
        void EnviarCotizacion(Cotizacion cotizacion, string emailDestino);
         */
        
        /// <summary>
        /// OS_CO006: Obtiene todas las cotizaciones almacenadas en el sistema.
        /// </summary>
        /// <returns></returns>
        IList<Cotizacion> GetCotizaciones();

        //void Enviar(string idCotizacion);
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Usuario (OS_USXXX)
        //------------------------------------------------------------------------------
        
        /// <summary>
        /// OS_US001: Guarda a un usuario en el sistema.
        /// </summary>
        /// <param name="persona"></param>
        /// <param name="password"></param>
        void Anadir(Persona persona, string password);

        //TODO
        void Anadir(Usuario cotizacion);
        
        /// <summary>
        /// OS_US002: Dados email y password, Verifica si el usuario existe y lo retorna.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <param name="password">Contrasenia de acceso al sistema</param>
        /// <returns></returns>
        Usuario Login(string rutEmail, string password);
        
        /// <summary>
        /// Operacion de sistema: Almacena una persona en el sistema.
        /// </summary>
        /// <param name="persona">Persona a guardar en el sistema.</param>
        void Anadir(Persona persona);

        /// <summary>
        /// Obtiene todas las personas del sistema.
        /// </summary>
        /// <returns>The IList of Persona</returns>
        IList<Persona> GetPersonas();
        
        /// <summary>
        /// Busqueda de una persona por rut o correo electronico.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <returns>La persona si existe</returns>
        Persona BuscarPersona(string rutEmail);
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Servicio (OS_SEXXX)
        //------------------------------------------------------------------------------

        void Anadir(Servicio servicio, Cotizacion cotizacion);
   
        void EditarServicio(Servicio servicio);
    
        void CambiarEstado(int index, EstadoServicio nuevoEstado);

        void Borrar(int index, string idCotizacion);
        
        void Desplegar(int index, string idCotizacion);

        void DesplegarTodos(string idCotizacion);
        
        IList<Servicio> GetServicios(string idCotizacion);
        
        //------------------------------------------------------------------------------
        //    Operaciones de Sistema: Cliente (OS_CLXXX)
        //------------------------------------------------------------------------------

        void Anadir(Persona persona, TipoCliente tipoCliente);
       
        /// <summary>
        /// OS_CL002: Busca al cliente segun su rut.
        /// Agiliza la insercion de un cliente a una cotizacion.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        Cliente BuscarCliente(string rut);

        void Desplegar(string rut);

        void EnviarEmail(Cotizacion cotizacionEnviar, string remitente, string emailPassword, string destinatario, MailMessage mailMessage);
        
        IList<Cliente> GetClientes();
    }
}