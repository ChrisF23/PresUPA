using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Models;

namespace Core.DAO
{
    /// <summary>
    /// Patron Repository para reemplazar a DAO (mas simple).
    /// https://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    /// <typeparam name="T">Clase derivada de BaseEntity</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Inicializa el repositorio para guardar las entidades. Ej. crear la tabla SQL.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Guarda una entidad en el repositorio.
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        void Add(T entity);

        /// <summary>
        /// Remueve una entidad del repositorio.
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        void Remove(T entity);

        /// <summary>
        /// Obtiene todas las entidades en el sistema.
        /// </summary>
        /// <returns>the List of T</returns>
        IList<T> GetAll();

        /// <summary>
        /// Obtiene una entidad por su identificador
        /// </summary>
        /// <param name="id">identificador de la entidad</param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// Funcion de busqueda mediante LINQ
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IList<T> GetAll(Expression<Func<T, bool>> expression);
    }
}