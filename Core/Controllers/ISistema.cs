using Core.Models;

namespace Core.Controllers
{
    /// <summary>
    /// Operaciones del sistema.
    /// </summary>
    public interface ISistema
    {
        /// <summary>
        /// Almacena una persona en el sistema.
        /// </summary>
        /// <param name="persona"></param>
        void Save(Persona persona);
    }
}