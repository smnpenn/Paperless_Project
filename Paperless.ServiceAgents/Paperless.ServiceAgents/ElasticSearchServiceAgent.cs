using System;
using Paperless.ServiceAgents.Interfaces;
using Elasticsearch.Net;
using Nest;
using Microsoft.Extensions.Options;
using Paperless.ServiceAgents.Options;
using Microsoft.Extensions.Configuration;


namespace Paperless.ServiceAgents
{
	public class ElasticSearchServiceAgent : IElasticSearchServiceAgent
	{
        private readonly ElasticClient _client;

		public ElasticSearchServiceAgent(IConfiguration configuration)
		{
            var elasticSearchUrl = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL") ?? "https://localhost:9200";
            var elasticUsername = Environment.GetEnvironmentVariable("ELASTICSEARCH_USERNAME");
            var elasticPassword = Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD");
            var elasticFingerprint = Environment.GetEnvironmentVariable("ELASTICSEARCH_CERT_FINGERPRINT");

            var settings = new ConnectionSettings(new Uri(elasticSearchUrl))
                .CertificateFingerprint(elasticFingerprint)
                .BasicAuthentication(elasticUsername, elasticPassword)
                .DefaultIndex("paperless-index")
                .EnableApiVersioningHeader();
            _client = new ElasticClient(settings);
		}

        public async Task<bool> IndexDocumentAsync<T>(string indexName, T document) where T : class
        {
            var response = await _client.IndexDocumentAsync(document);
            if(response.IsValid)
            {
                Console.WriteLine("Document indexed successfully");
                return true;
            } else
            {
                Console.WriteLine($"Failed to index document: {response.OriginalException.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<T>> SearchAsync<T>(string indexName, string searchTerm, string fieldName) where T : class
        {
            var searchResponse = await _client.SearchAsync<T>(s => s
                .Index(indexName)
                .Query(q => q
                    .Match(m => m
                        .Field(fieldName)
                        .Query(searchTerm)
                    )
                )
            );

            return searchResponse.Documents;
        }
    }
}

