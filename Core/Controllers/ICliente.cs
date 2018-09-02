using System.Collections.Generic;
using Core.Models;

namespace Core.Controllers
{
    public interface ICliente
    {
        void GuardarCliente(Cliente cliente);
        //void BuscarCliente(string busqueda);
        //IList<Cliente> ObtenerClientes();
    }
}