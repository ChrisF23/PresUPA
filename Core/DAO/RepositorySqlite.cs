using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
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
            
            sb.Append(_type.Name); // Nombre de la tabla
            sb.Append(" (\n");

            // Ciclo para las propiedades de la clase
            PropertyInfo[] properties = _type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                sb.Append(" ").Append(property.Name);
                
                TypeCode typeCode = Type.GetTypeCode(property.PropertyType);

                switch (typeCode)
                {
                    case TypeCode.String:
                        sb.Append(" TEXT");
                        break;
                    case TypeCode.Int16: // short
                    case TypeCode.Int32: // integer
                    case TypeCode.Int64: // long
                        sb.Append(" INTEGER");
                        break;
                    case TypeCode.DateTime:
                        sb.Append(" REAL");
                        break;
                    default:
                        throw new NotSupportedException("Tipo no soportado: " + typeCode);
                }
                
                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");

            }
            sb.Append(");");

            // TODO: Remover mensajes de debug
            Console.WriteLine(sb.ToString());
            
            // Ejecucion del SQL en el backend
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
            sb.Append( _type.Name).Append(" (\n");

            PropertyInfo[] properties = _type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                sb.Append(" ").Append(property.Name);
                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");                
            }

            sb.Append(") VALUES (\n");
            foreach (PropertyInfo property in properties)
            {
                sb.Append(" ");
                
                TypeCode typeCode = Type.GetTypeCode(property.PropertyType);
                switch (typeCode)
                {
                    case TypeCode.String:
                        sb.Append("'").Append(property.GetValue(entity, null)).Append("'");
                        break;
                    case TypeCode.Int16: // short
                    case TypeCode.Int32: // integer
                    case TypeCode.Int64: // long
                        sb.Append(property.GetValue(entity, null));
                        break;
                    case TypeCode.DateTime:
                        sb.Append(((DateTime) property.GetValue(entity, null)).Ticks);
                        break;
                    default:
                        throw new NotSupportedException("Tipo no soportado: " + typeCode);
                }
                
                
                if (!properties.LastOrDefault().Equals(property))
                {
                    sb.Append(",");
                }

                sb.Append("\n");                
            }
            
            sb.Append(");");
            
            // TODO: Eliminar mensaje de debug
            Console.WriteLine(sb.ToString());
            
            // Ejecucion de la query en Sqlite
            _sqliteDbContext.Database.ExecuteSqlCommand(sb.ToString());
        }

        /// <inheritdoc />
        public IList<T> GetAll()
        {
            // List of T
            List<T> list = new List<T>();
            
            // RAW sql
            string sql = "SELECT * FROM " + _type.Name + ";";

            // Using raw querys
            using (DbCommand command = _sqliteDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                _sqliteDbContext.Database.OpenConnection();
                using (DbDataReader reader = command.ExecuteReader())
                {
                    // Esquema de la tabla
                    DataTable schemaTable = reader.GetSchemaTable();

                    // Si hay tuplas en el resultado
                    if (reader.HasRows)
                    {
                        // Por cada tupla encontrada
                        while (reader.Read())
                        {
                            // Constructor generico vacio
                            T instance = Activator.CreateInstance<T>();
                            
                            // Agrego a la lista
                            list.Add(instance);
                            
                            // Para cada atributo en la columnas se traspasan al nombre
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // Nombre, tipo y valor
                                string name = reader.GetName(i);                                
                                string type = reader.GetDataTypeName(i);
                                object value = reader.GetValue(i);
                                
                                // Console.WriteLine($"Row: {name} of type: {type} has value: {value}");
                                
                                // Reflection ?!?
                                PropertyInfo propertyInfo = _type.GetProperty(name);
                                
                                TypeCode typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                                switch (typeCode)
                                {
                                        case TypeCode.Int16:
                                        case TypeCode.Int32:
                                        case TypeCode.Int64:
                                        case TypeCode.String:
                                            propertyInfo.SetValue(instance, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                                            break;
                                        case TypeCode.DateTime:
                                            long time = Convert.ToInt64(value);
                                            propertyInfo.SetValue(instance, Convert.ChangeType(new DateTime(time), propertyInfo.PropertyType), null);
                                            break;
                                        default:
                                            throw new NotSupportedException("Tipo no soportado: " + typeCode);
                                }
                                
                            }
                            
                        }
                    }
                }
            }

            return list;
        }
        
    }
}