using System.Collections.Generic;
using Core.Models;

namespace Core.Controllers
{
    public interface ICliente
    {    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        void GuardarCliente(Cliente cliente);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busqueda"></param>
        void BuscarCliente(string busqueda);
        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="id"></param>
        void DesplegarCliente(string id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Cliente> ObtenerClientes();
    }
}