using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paperless.ServiceAgents.Interfaces;
using Microsoft.Extensions.Options;
using Tesseract;
using ImageMagick;

namespace Paperless.ServiceAgents
{
    public class OcrServiceAgent : IOcrServiceAgent
    {
        private readonly string tessDataPath;
        private readonly string language;

        public OcrServiceAgent(IOptions<OcrOptions> options)
        {
            this.tessDataPath = options.Value.TessDataPath;
            this.language = options.Value.Language;
        }

        public string PerformOcrPdf(Stream pdfStream)
        {
            var stringBuilder = new StringBuilder();

            using (var magickImages = new MagickImageCollection())
            {
                magickImages.Read(pdfStream);
                foreach (var magickImage in magickImages)
                {
                    // Set the resolution and format of the image (adjust as needed)
                    magickImage.Density = new Density(500, 500);
                    magickImage.Format = MagickFormat.Png;

                    // Perform OCR on the image
                    using (var tesseractEngine = new TesseractEngine(tessDataPath, language, EngineMode.Default))
                    {
                        using (var page = tesseractEngine.Process(Pix.LoadFromMemory(magickImage.ToByteArray())))
                        {
                            var extractedText = page.GetText();
                            stringBuilder.Append(extractedText);
                        }
                    }
                }
            }


            return stringBuilder.ToString();
        }
    }
}
