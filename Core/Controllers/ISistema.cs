using Core.Models;

namespace Core.Controllers
{
    /// <summary>
    /// Operaciones del sistema.
    /// </summary>
    public interface ISistema
    {
        /// <summary>
        /// Operacion de sistema: Almacena una persona en el sistema.
        /// </summary>
        /// <param name="persona">Persona a guardar en el sistema.</param>
        void Save(Persona persona);
    }
}