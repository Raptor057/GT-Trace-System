using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using GT.Trace.Common;
using System;
using GT.Trace.Packaging.App.UseCases.PackUnit;

namespace GT.Trace.Packaging.AppTests.Packaging.App.UseCases.PackUnit
{
    [TestClass()]
    public class PackUnitRequestTests
    {
        [TestMethod()]
        public void CanCreateTest()
        {
            // Arrange
            string scannerInput = @"[)>06SWB9728455P147-2238ZGT1T872442T073T16023";
            string hostname = "LA";
            ErrorList errors;

            // Act
            bool result = PackUnitRequest.CanCreate(scannerInput, hostname, out errors);


            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(errors.IsEmpty);
            //Assert.Fail();

            // Print the results
            Console.WriteLine($"CanCreateTest - Result: {result}, Errors: {errors.Count}");
        }

        [TestMethod()]
        public void CreateTest()
        {
            //Assert.Fail();
            // Arrange
            string scannerInput = "123";
            string hostname = "QRCode";
            int? palletSize = 10;
            int? containerSize = 5;
            string lineCode = "LC123";
            string poNumber = "PO456";

            // Act
            PackUnitRequest result = PackUnitRequest.Create(scannerInput, hostname, palletSize, containerSize, lineCode, poNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(scannerInput, result.ScannerInput);
            Assert.AreEqual(hostname, result.Hostname);
            Assert.AreEqual(palletSize, result.PalletSize);
            Assert.AreEqual(containerSize, result.ContainerSize);
            Assert.AreEqual(lineCode, result.LineCode);
            Assert.AreEqual(poNumber, result.PoNumber);

            Console.WriteLine($"Resultado de CreateTest: \n{result.ScannerInput}\n{result.Hostname}\n{result.PalletSize}\n{result.PalletSize}\n{result.ContainerSize}\n{result.LineCode}\n{result.PoNumber}");
        }

        [TestMethod()]
        [ExpectedException(typeof(ErrorList))]
        public void Create_InvalidInput_ThrowsErrorListException()
        {
            // Arrange
            string? scannerInput = null; // Invalid input
            string hostname = "QRCode";
            int? palletSize = 10;
            int? containerSize = 5;
            string lineCode = "LC123";
            string poNumber = "PO456";

            // Act
            PackUnitRequest result = PackUnitRequest.Create(scannerInput, hostname, palletSize, containerSize, lineCode, poNumber);
            Console.WriteLine($"El resultado de PackUnitRequest es:\n{result.ScannerInput}\n{result.Hostname}\n{result.PalletSize}\n{result.ContainerSize}\n{result.LineCode}\n{result.PoNumber}");
            // Assert (Exception expected)
        }
    }
}