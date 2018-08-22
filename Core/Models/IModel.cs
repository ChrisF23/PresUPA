namespace Core.Models
{
    /// <summary>
    /// Interface base para las clases del Models (Dominio).
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Validacion de los atributos
        /// </summary>
        void Validate();
    }
}