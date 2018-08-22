namespace Core.Models
{
    /// <summary>
    /// Clase que representa una Persona en el sistema de presupuesto.
    /// </summary>
    public class Persona
    {
        
        /// <summary>
        /// Identificador unico.
        /// </summary>
        public string Rut { get; set; }
        
        /// <summary>
        /// Primer y segundo (optativo) nombre de la persona.
        /// </summary>
        public string Nombre { get; set; }
        
        /// <summary>
        /// Apellido paterno.
        /// </summary>
        public string Paterno { get; set; }
        
        /// <summary>
        /// Apellido materno.
        /// </summary>
        public string Materno { get; set; }

        /// <summary>
        /// Validacion de los atributos de la clase.
        /// </summary>
        public void validate()
        {
            
        }
        
    }
}