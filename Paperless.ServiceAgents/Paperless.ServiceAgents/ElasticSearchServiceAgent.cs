using System;
using Paperless.ServiceAgents.Interfaces;
using Elasticsearch.Net;
using Nest;
using Microsoft.Extensions.Options;
using Paperless.ServiceAgents.Options;

namespace Paperless.ServiceAgents
{
	public class ElasticSearchServiceAgent : IElasticSearchServiceAgent
	{
        private readonly ElasticClient _client;

		public ElasticSearchServiceAgent(IOptions<ElasticSearchOptions> options)
		{
            var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
                .CertificateFingerprint("CD:B1:33:17:88:D2:F2:42:8C:E4:A1:1C:23:0E:DE:3A:86:D7:BE:91:7C:70:E9:B9:A0:81:90:22:82:3D:E9:CD")
                .BasicAuthentication("elastic", "gwmdw9sSFnK0UyTNmjNO")
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

