using System;

namespace Core
{
    /// <summary>
    /// Error del modelo
    /// </summary>
    public class ModelException : Exception
    {
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="message">mensaje del error</param>
        public ModelException(string message) : base(message)
        {
        }
    }
}