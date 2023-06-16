using GT.Trace.Packaging.Infra.Services;
using GT.Trace.Packaging.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Packaging.Tests
{
    [TestClass]
    public class LabelParserServiceTests
    {
        private LabelParserService? _labelParser;

        [TestInitialize]
        public void TestInitialize()
        {
            // Configurar cualquier configuración o dependencias necesarias para las pruebas
            // Ejemplo: Crear mocks o instancias con valores necesarios
            var loggerMock = new Mock<ILogger<LabelParserService>>();
            var configurationMock = new Mock<IConfigurationRoot>();

            _labelParser = new LabelParserService(loggerMock.Object, configurationMock.Object);
        }

        [TestMethod]
        public void TryParseNewWBFormatTest()
        {
            // Arrange
            string value = @"[)>06SWB9728455P147-2238ZGT1T872442T073T16023";

            // Act
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            bool result = _labelParser.TryParseNewWBFormat(value, out Label? labelData);
            #pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Assert
            Assert.IsTrue(result);
        }
    }
}
