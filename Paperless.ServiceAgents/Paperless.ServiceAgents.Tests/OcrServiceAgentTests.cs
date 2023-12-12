using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Paperless.ServiceAgents.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.ServiceAgents.Tests
{
    public class OcrServiceAgentTests
    {
        [Test]
        public void PerformOcr_InputPdf_ReturnsRecognizedText()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new OcrOptions());

            var serviceProvider = new ServiceCollection()
            .AddSingleton<IOptions<OcrOptions>>(options)
            .AddTransient<OcrServiceAgent>()
            .BuildServiceProvider();

            // Arrange
            var ocrService = serviceProvider.GetService<OcrServiceAgent>();
            string inputPdfPath = "./files/LoremIpsumDolor.pdf"; // Provide an existing input PDF file path

            // Act
            string recognizedText = ocrService.PerformOcrPdf(File.OpenRead(inputPdfPath));

            // Assert
            Assert.IsNotEmpty(recognizedText);
        }
    }
}
