using System.Collections.Generic;
using Core.Models;

namespace Core.Controllers
{
    /// <summary>
    /// Operaciones del sistema.
    /// </summary>
    public interface IUsuario
    {
        /// <summary>
        /// OS_US001: Guarda a un usuario en el sistema.
        /// </summary>
        /// <param name="persona"></param>
        /// <param name="password"></param>
        void Save(Persona persona, string password);

        /// <summary>
        /// OS_US002: Dados email y password, Verifica si el usuario existe y lo retorna.
        /// </summary>
        /// <param name="rutEmail">RUT o Correo Electronico</param>
        /// <param name="password">Contrasenia de acceso al sistema</param>
        /// <returns></returns>
        Usuario Login(string rutEmail, string password);

        
    }
}