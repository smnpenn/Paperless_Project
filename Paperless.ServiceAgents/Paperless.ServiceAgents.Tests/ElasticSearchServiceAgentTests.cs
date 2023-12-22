using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Paperless.ServiceAgents.Options;

namespace Paperless.ServiceAgents.Tests
{
	public class ElasticSearchServiceAgentTests
	{

        private ElasticSearchServiceAgent CreateServiceAgent()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new ElasticSearchOptions
            {
                ConnectionString = "http://localhost:9200"
            });

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IOptions<ElasticSearchOptions>>(options)
                .AddSingleton<ElasticSearchServiceAgent>()
                .BuildServiceProvider();

            return serviceProvider.GetService<ElasticSearchServiceAgent>();
        }

        [Test]
        public async Task IndexDocumentAsync_ReturnsTrue_WhenDocumentIsIndexedSuccessfully()
        {
            // Arrange
            var service = CreateServiceAgent();
            var testDocument = new { Name = "Test Document", Content = "This is a test" };
            string indexName = "paperless-index";

            // Act
            bool result = await service.IndexDocumentAsync(indexName, testDocument);

            // Assert
            Assert.IsTrue(result);
        }
    }
}

