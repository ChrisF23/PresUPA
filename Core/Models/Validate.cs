using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

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

            /*
            
            //No funciona:
            //[+] Elimina los guiones que se ingresen.
            rut = rut.Replace("-", "");
            
            //[+] Se asegura de que la K siempre este en mayuscula.
            rut = rut.ToUpper();
            
             */
            
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



        /// <summary>
        /// Metodo que valida un email
        /// </summary>
        /// <param name="email"></param>
        public static void ValidarEmail(string email)
        {
            try
            {

                MailAddress validate = new MailAddress(email);

            }
            catch (FormatException)
            {
                throw new ModelException("El email tiene un formato invalido");
            }

        }

        /// <summary>
        /// Metodo que compara dos cotizaciones y ve si algunos
        /// atributos especificos son iguales.
        /// </summary>
        /// <param name="cot1"></param>
        /// <param name="cot2"></param>
        /// <returns></returns>
        public static bool CompararCotizaciones(Cotizacion cot1, Cotizacion cot2)
        {
            
            //Retorna true si los siguientes atributos son iguales.
            if (cot1.Titulo.Equals(cot2.Titulo) &&
                cot1.Descripcion.Equals(cot2.Descripcion) &&
                cot1.Numero.Equals(cot2.Numero) &&
                cot1.Version.Equals(cot2.Version) &&
                cot1.Cliente.Equals(cot2.Cliente) &&
                cot1.Servicios.Equals(cot2.Servicios))
            {
                return true;
            }
            //Retorna false si alguno es distinto.
            return false;

        }

    }
}