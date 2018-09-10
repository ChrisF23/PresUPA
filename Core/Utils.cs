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

        public static readonly string SmtpServerGmail = "gmail";
        public static readonly string SmtpServerOutlook = "live";
        public static readonly string SmtpServerOffice365 = "office365";

        /*
        public static readonly string[,] SmptServersAndPorts = new string[,]
        {
            {SmptServerGmail, "587"},
            {SmptServerOutlook, "587"},
            {SmptServerHotmail, "465"},
            {SmptServerOffice365, "587"}
        };
        */
        
        public static readonly string[] SmtpServers = new string[]
        {
            SmtpServerGmail,
            SmtpServerOutlook,
            SmtpServerOffice365
        };
        



    }
}