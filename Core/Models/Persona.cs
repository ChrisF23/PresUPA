namespace Core.Models
{
    /// <summary>
    /// Clase que representa una Persona en el sistema de presupuesto.
    /// </summary>
    public class Persona : IModel
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
        public void Validate()
        {
            if (Rut == null)
            {
                throw new ModelException("Rut no puede ser null");
            }

            if (Nombre == null || Nombre.Length < 2)
            {
                throw new ModelException("Nombre no puede ser null o de tamanio inferior a 2");
            }

            if (Paterno == null || Paterno.Length < 2)
            {
                throw new ModelException("Apellido Paterno no puede ser null o tamanio inferior a 2");
            }
        }
        
    }
}