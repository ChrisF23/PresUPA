using Core;
using Core.Models;
using Xunit;

namespace TestCore.Models
{
    /// <summary>
    /// Test de validacioness
    /// </summary>
    public sealed class ValidateTests
    {

        /// <summary>
        /// Validacion correcta del rut
        /// </summary>
        /// <param name="rut"></param>
        [Theory]
        [InlineData("130144918")]
        [InlineData("113446498")]
        [InlineData("132204810")]
        [InlineData("12345670K")]
        public void ValidateRutCorrecto(string rut)
        {
            Validate.ValidarRut(rut);
        }

        /// <summary>
        /// Se expera una ModelException en estos casos
        /// </summary>
        /// <param name="rut"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("Esto no es un RUT valido")]
        [InlineData("13220A810")]
        [InlineData("12345678Z")]
        [InlineData("1234567-Z")]
        public void ValidateRutLanzarException(string rut)
        {
            Assert.Throws<ModelException>(() => Validate.ValidarRut(rut));
        }
    }
}