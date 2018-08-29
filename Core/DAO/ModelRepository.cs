using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAO
{
    /// <summary>
    /// Implementacion del IRepository Generico basado en EntityFramework Core.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelRepository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Referencia a la base de datos.
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Tipo especifico de la clase.
        /// </summary>
        private readonly Type _type = typeof(T);

        /// <summary>
        /// Constructor. Requiere el DbContext de la base de datos.
        /// </summary>
        /// <param name="dbContext">Acceso a la base de datos</param>
        public ModelRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException("Se requiere el DbContext!");
        }
        
        /// <inheritdoc />
        public void Initialize()
        {
            // throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public IList<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }
    }
}