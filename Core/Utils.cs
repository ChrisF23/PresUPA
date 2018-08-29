using Newtonsoft.Json;

namespace Core
{
    /// <summary>
    /// Clase que recopila todos los metodos comunes a la aplicacion.
    /// </summary>
    public sealed class Utils
    {
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