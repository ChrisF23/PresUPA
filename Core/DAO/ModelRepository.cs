using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        protected readonly DbContext DbContext;

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
            DbContext = dbContext ?? throw new ArgumentException("Se requiere el DbContext!");
        }
        
        /// <inheritdoc />
        public void Initialize()
        {
            // throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            // Validacion de la entidad antes de ingresar a la bd
            entity.Validate();

            // Si ya tengo id, solo es necesario actualizar.
            DbContext.Entry(entity).State = entity.Id == 0 ?EntityState.Added : EntityState.Modified;            
            DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public void Remove(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            DbContext.SaveChanges();
        }
        
        /// <inheritdoc />
        public IList<T> GetAll()
        {
            return DbContext.Set<T>().ToList();
        }

        /// <inheritdoc />
        public T GetById(int id)
        {
            return DbContext.Set<T>().Find(id);
        }

        /// <inheritdoc />
        public IList<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return DbContext.Set<T>().Where(expression).ToList();
        }
    }
}