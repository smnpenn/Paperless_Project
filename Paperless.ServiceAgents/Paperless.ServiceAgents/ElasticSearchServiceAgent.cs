using System;
using Paperless.ServiceAgents.Interfaces;
using Elasticsearch.Net;
using Nest;

namespace Paperless.ServiceAgents
{
	public class ElasticSearchServiceAgent : IElasticSearchServiceAgent
	{
        private readonly ElasticClient _client;

		public ElasticSearchServiceAgent(string connectionString)
		{
            var settings = new ConnectionSettings(new Uri(connectionString))
                .DefaultIndex("my-index-001");
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

