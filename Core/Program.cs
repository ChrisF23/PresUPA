using System;

namespace Core
{
    class App
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting ..");
            
            ModelException modelException = new ModelException("Error en el modelo");
            throw modelException;
        }
    }
}    