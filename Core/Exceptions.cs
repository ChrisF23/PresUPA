using System;

namespace Core
{
    /// <summary>
    /// Error del modelo
    /// </summary>
    public class ModelException : Exception
    {
        /// <inheritdoc />
        public ModelException(string message) : base(message)
        {
        }
    }
}