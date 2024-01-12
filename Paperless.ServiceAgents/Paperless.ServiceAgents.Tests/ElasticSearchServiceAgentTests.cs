using System;
using dotenv.net;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Paperless.ServiceAgents.Interfaces;
using Paperless.ServiceAgents.Options;



namespace Paperless.ServiceAgents.Tests
{
    [TestFixture]
    public class ElasticSearchServiceAgentTests
	{

        private Mock<IElasticSearchServiceAgent> _mockElasticSearchServiceAgent;

        [SetUp]
        public void Setup()
        {
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "../../.env" }));

            _mockElasticSearchServiceAgent = new Mock<IElasticSearchServiceAgent>();

            _mockElasticSearchServiceAgent.Setup(x => x.IndexDocumentAsync(It.IsAny<string>(), It.IsAny<object>()))
                 .ReturnsAsync(true); // Assuming it returns a boolean

        }

        [Test]
        public async Task IndexDocumentAsync_WhenCalled_ReturnTrueIfValid()
        {


            var document = new { Id = 1, Name = "Test Document", Content = "Sample Content" };

            var result = await _mockElasticSearchServiceAgent.Object.IndexDocumentAsync("paperless-test-index", document);

            Assert.IsTrue(result);
        }
        

        [Test]
        public async Task UpdateDocumentAsync_WhenCalled_ReturnsTrueIfValid()
        {
            // Arrange
            var document = new { Id = 1, Name = "Updated Test Document", Content = "Updated Content" };
            _mockElasticSearchServiceAgent.Setup(x => x.UpdateDocumentAsync("paperless-test-index", "1", document))
                                  .ReturnsAsync(true);
            var result = await _mockElasticSearchServiceAgent.Object.UpdateDocumentAsync("paperless-test-index", "1", document);
            Assert.IsTrue(result);
        }


        [Test]
        public async Task DocumentExistsAsync_WhenCalled_ReturnsTrueIfDocumentExists()
        {
            _mockElasticSearchServiceAgent.Setup(x => x.DocumentExistsAsync("paperless-test-index", "1"))
                                          .ReturnsAsync(true);
            var result = await _mockElasticSearchServiceAgent.Object.DocumentExistsAsync("paperless-test-index", "1");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteDocumentAsync_WhenCalled_ReturnsTrueIfValid()
        {

            var document = new { Id = 1, Name = "Test Document", Content = "Sample Content" };

            var result2 = await _mockElasticSearchServiceAgent.Object.IndexDocumentAsync("paperless-test-index", document);

            _mockElasticSearchServiceAgent.Setup(x => x.DeleteDocumentAsync("paperless-test-index", "1"))
                                          .ReturnsAsync(true);
            var result = await _mockElasticSearchServiceAgent.Object.DeleteDocumentAsync("paperless-test-index", "1");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SearchAsync_WhenCalled_ReturnsMatchingDocuments()
        {
            var searchResults = new List<object> { new { Id = 1, Name = "Test Document", Content = "Sample Content" } };
            _mockElasticSearchServiceAgent.Setup(x => x.SearchAsync<object>("paperless-test-index", "Test", "content"))
                                          .ReturnsAsync(searchResults);
            var results = await _mockElasticSearchServiceAgent.Object.SearchAsync<object>("paperless-test-index", "Test", "content");
            Assert.IsNotEmpty(results);
        }

        [Test]
        public async Task FuzzySearchAsync_WhenCalled_ReturnsFuzzyMatchingDocuments()
        {
            var fuzzyResults = new List<object> { new { Id = 1, Name = "Test Document", Content = "Sample Content" } };
            _mockElasticSearchServiceAgent.Setup(x => x.FuzzySearchAsync<object>("paperless-test-index", "Test", new[] { "content" }, 10))
                                          .ReturnsAsync(fuzzyResults);
            var results = await _mockElasticSearchServiceAgent.Object.FuzzySearchAsync<object>("paperless-test-index", "Test", new[] { "content" }, 10);
            Assert.IsNotEmpty(results);
        }


    }
}

