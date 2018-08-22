using System;
using System.Collections;
using System.Linq;
using System.Text;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAO
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : IModel
    {
        /// <summary>
        /// Referencia a la base de datos.
        /// </summary>
        protected readonly DbContext _dbContext;

        /// <summary>
        /// Tipo de la clase.
        /// </summary>
        protected readonly Type _type = typeof(T);

        /// <inheritdoc />
        public Repository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
        }

        /// <inheritdoc />
        public void Initialize()
        {
            // Creacion del SQL de creacion de la tabla.
            StringBuilder sb = new StringBuilder("CREATE TABLE ");
            
            sb.Append( _type.Name.ToLower()); // Nombre de la tabla (minuscula)
            sb.Append(" (\n");

            // Ciclo para las propiedades de la clase
            var properties = _type.GetProperties();
            foreach (var property in properties)
            {
                sb.Append(" ").Append(property.Name.ToLower());
                
                // Si el tipo es string, se usa "text"
                if (property.PropertyType == typeof(string))
                {
                    sb.Append(" text");
                }
                
                // FIXME: Agregar los tipos que faltan

                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");

                // Console.WriteLine(property.Name + " --> " + Type.GetTypeCode(property.PropertyType));
            }
            sb.Append(");");
            
            Console.WriteLine(sb.ToString());
            _dbContext.Database.ExecuteSqlCommand(sb.ToString());

        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            // Validation
            entity.Validate();
            
            // Creacion del SQL de insercion
            StringBuilder sb = new StringBuilder("INSERT INTO ");
            sb.Append( _type.Name.ToLower()).Append(" (\n");

            var properties = _type.GetProperties();
            foreach (var property in properties)
            {
                sb.Append(" ").Append(property.Name.ToLower());
                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");                
            }

            sb.Append(") VALUES (\n");
            foreach (var property in properties)
            {
                sb.Append(" '").Append(property.GetValue(entity,null)).Append("'");
                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");                
            }
            
            sb.Append(");");
            
            Console.WriteLine(sb.ToString());
            
            _dbContext.Database.ExecuteSqlCommand(sb.ToString());
        }

        /// <inheritdoc />
        public IEnumerable All()
        {
            throw new NotImplementedException();
        }
    }
}