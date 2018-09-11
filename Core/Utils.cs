using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Core
{
    /// <summary>
    /// Clase que recopila todos los metodos comunes a la aplicacion.
    /// </summary>
    public sealed class Utils
    {
        
        public static readonly string[] SmtpServers = new string[]
        {
            "gmail",
            "live",
            "office365",
            "outlook"
        };

        public static string ToFormatedDate(DateTime dt)
        {
            return dt.ToString("D", 
                CultureInfo.CreateSpecificCulture("es-MX"));
        }

        /// <summary>
        /// Imprime un objeto en formato json.
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        


    }
}