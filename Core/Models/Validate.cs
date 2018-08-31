using System;

namespace Core.Models
{
    /// <summary>
    /// Clase que contiene funciones para validacion del modelo.
    /// </summary>
    public sealed class Validate
    {
        /// <summary>
        /// Metodo que valida un rut
        /// </summary>
        /// <param name="rut">Rut a validar</param>
        /// <exception cref="ModelException">Exception en caso de no ser valido</exception>
        public static void ValidarRut(string rut)
        {
            if (String.IsNullOrEmpty(rut))
            {
                throw new ModelException("Rut no valido");
            }

            try
            {
                int rutNumber = Convert.ToInt32(rut.Substring(0, rut.Length - 1));
                char dv = Convert.ToChar(rut.Substring(rut.Length - 1, 1));

                int m = 0;
                int s = 1;
                for (; rutNumber != 0; rutNumber /= 10)
                {
                    s = (s + rutNumber % 10 * (9 - m++ % 6)) % 11;
                }

                if (dv != Convert.ToChar(s != 0 ? s + 47 : 75))
                {
                    throw new ModelException("Rut no valido");
                }
            }
            catch (FormatException)
            {
                throw new ModelException("Rut no valido");
            }

        }
    }
}