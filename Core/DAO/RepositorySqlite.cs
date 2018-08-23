using System;
using System.Collections;
using System.Linq;
using System.Text;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAO
{
    /// <summary>
    /// Implementacion del IRepository generico.
    /// </summary>
    /// <typeparam name="T">Clase que implementa IModel</typeparam>
    public class RepositorySqlite<T> : IRepository<T> where T : IModel
    {
        /// <summary>
        /// Referencia a la base de datos.
        /// </summary>
        protected readonly SqliteDbContext _sqliteDbContext;

        /// <summary>
        /// Tipo (generico) de la clase.
        /// </summary>
        protected readonly Type _type = typeof(T);

        /// <summary>
        /// Construccion del repositorio conectado a la base de datos.
        /// </summary>
        public RepositorySqlite(SqliteDbContext sqliteDbContext)
        {
            _sqliteDbContext = sqliteDbContext ?? throw new ArgumentException("Se requiere el contexto Sqlite");
        }
        
        /// <inheritdoc />
        public void Initialize()
        {
            // Creacion del SQL de creacion de la tabla.
            StringBuilder sb = new StringBuilder("CREATE TABLE ");
            
            sb.Append(_type.Name.ToLower()); // Nombre de la tabla (minuscula)
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
                
                // FIXME: Agregar los tipos que faltan (Int, Char, Boolean, etc)

                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");

                // Console.WriteLine(property.Name + " --> " + Type.GetTypeCode(property.PropertyType));
            }
            sb.Append(");");
            
            // Console.WriteLine(sb.ToString());
            _sqliteDbContext.Database.ExecuteSqlCommand(sb.ToString());

        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            // Validacion de nulidad
            if (entity == null)
            {
                throw new ArgumentException("Entidad a guardar es null");
            }
            
            // Validacion del IModel
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
            
            // Console.WriteLine(sb.ToString());
            
            // Ejecucion de la query en Sqlite
            _sqliteDbContext.Database.ExecuteSqlCommand(sb.ToString());
        }

        /// <inheritdoc />
        public IEnumerable All()
        {
            throw new NotImplementedException();
        }
        
    }
}