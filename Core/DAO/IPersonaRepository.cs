using Core.Models;

namespace Core.DAO
{
    /// <summary>
    /// Operaciones especificas de un respositorio de personas.
    /// </summary>
    public interface IPersonaRepository : IRepository<Persona>
    {

        /// <summary>
        /// Busca a una persona por su RUT.
        /// </summary>
        /// <param name="rut">RUT</param>
        /// <returns>The Personas</returns>
        Persona GetByRut(string rut);

        /// <summary>
        /// Obtiene una persona por su RUT o Correo electronico.
        /// </summary>
        /// <param name="rutEmail"></param>
        /// <returns></returns>
        Persona GetByRutOrEmail(string rutEmail);
    }
}