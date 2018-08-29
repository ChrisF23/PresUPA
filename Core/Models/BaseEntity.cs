namespace Core.Models
{
    /// <summary>
    /// Entidad base
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Validacion de los atributos
        /// </summary>
        public abstract void Validate();
    }
}