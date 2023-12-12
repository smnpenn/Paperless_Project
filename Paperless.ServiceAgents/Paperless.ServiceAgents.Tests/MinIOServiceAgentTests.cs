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
    public class MinIOServiceAgentTests
    {
        [Test, Order(2)]
        public async Task UploadDocument_Test()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new MinIOOptions());

            var serviceProvider = new ServiceCollection()
            .AddSingleton<IOptions<MinIOOptions>>(options)
            .AddTransient<MinIOServiceAgent>()
            .BuildServiceProvider();

            var minIOService = serviceProvider.GetService<MinIOServiceAgent>();
            string filePath = "./files/LoremIpsumDolor.pdf";
            string fileName = "LoremIpsum";

            await minIOService.UploadDocument(filePath, fileName);

            var stat = await minIOService.GetDocument(fileName + ".pdf");

            Assert.NotNull(stat);
        }

        [Test, Order(1)]
        public async Task DeleteDocument_Test()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new MinIOOptions());

            var serviceProvider = new ServiceCollection()
            .AddSingleton<IOptions<MinIOOptions>>(options)
            .AddTransient<MinIOServiceAgent>()
            .BuildServiceProvider();

            var minIOService = serviceProvider.GetService<MinIOServiceAgent>();
            string filePath = "./files/LoremIpsumDolor.pdf";
            string fileName = "LoremIpsum";
            string objectName = fileName + ".pdf";
            await minIOService.UploadDocument(filePath, fileName);

            var stat = await minIOService.GetDocument(objectName);

            Assert.NotNull(stat);

            await minIOService.DeleteDocument(objectName);
            stat = await minIOService.GetDocument(objectName);

            Assert.Null(stat);
        }
    }
}
