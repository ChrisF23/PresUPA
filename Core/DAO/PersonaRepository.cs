using System.Linq;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAO
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Persona"></typeparam>
    public class PersonaRepository : ModelRepository<Persona>, IPersonaRepository
    {
        /// <inheritdoc cref="ModelRepository{T}"/>
        public PersonaRepository(DbContext dbContext) : base(dbContext)
        {
            // Nothing here
        }

        /// <inheritdoc cref="IPersonaRepository.GetByRut"/>
        public Persona GetByRut(string rut)
        {
            return _dbContext.Set<Persona>().FirstOrDefault(p => p.Rut.Equals(rut));
        }
    }
}