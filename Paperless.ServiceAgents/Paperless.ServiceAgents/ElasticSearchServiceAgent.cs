using System;
using Paperless.ServiceAgents.Interfaces;
using Elasticsearch.Net;
using Nest;
using Microsoft.Extensions.Options;
using Paperless.ServiceAgents.Options;
using Microsoft.Extensions.Configuration;
using Paperless.ServiceAgents.Exceptions;
using Microsoft.Extensions.Logging;



namespace Paperless.ServiceAgents
{
	public class ElasticSearchServiceAgent : IElasticSearchServiceAgent
	{
        private readonly ElasticClient _client;

        private readonly IElasticClient _clientES;

        private readonly ILogger _logger;

        public ElasticSearchServiceAgent(IElasticClient client, ILogger<ElasticSearchServiceAgent> logger)
        {
            _clientES = client ?? throw new ArgumentNullException(nameof(_clientES));
            _logger = logger;
        }

        public ElasticSearchServiceAgent(ILogger<ElasticSearchServiceAgent> logger)
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
            _logger = logger;
		}

        // index document

        public async Task<bool> IndexDocumentAsync<T>(string indexName, T document) where T : class
        {
            try
            {
                var response = await _client.IndexDocumentAsync(document);
                if (response.IsValid)
                {
                    _logger?.LogInformation("Document index successfully");
                    return true;
                }
                else
                {
                    _logger?.LogError($"Failed to index document: {response.OriginalException.Message}");
                    return false;
                }
            } catch(Exception ex)
            {
                _logger?.LogError($"Failed to index document");
                throw new ElasticSearchExceptions("Indexing was not successful", ex);
            }
        }
        
        // Update documents

        public async Task<bool> DocumentExistsAsync(string indexName, string documentId)
        {
            try
            {
                var response = await _client.DocumentExistsAsync(DocumentPath<object>.Id(documentId), d => d.Index(indexName));
                _logger?.LogInformation($"Successfully fetched document");
                return response.Exists;
            } catch(Exception ex)
            {
                _logger?.LogError($"Failed to fetch document");
                throw new ElasticSearchExceptions("Fetching document was not successful", ex);
            }
        }

        public async Task<bool> UpdateDocumentAsync<T>(string indexName, string documentId, T document) where T : class
        {
            try
            {
                var response = await _client.UpdateAsync<T>(documentId, u => u.Index(indexName).Doc(document).Refresh(Refresh.True));

                if (response.IsValid)
                {
                    _logger?.LogInformation("Document updated successfully");
                    return true;
                }
                else
                {
                    _logger?.LogError($"Response was not valid: {response.OriginalException.Message}");
                    throw new ElasticSearchExceptions("Updating document was not successful");
                }
            }catch (Exception ex)
            {
                _logger?.LogError($"{ex.Message}");
                throw new ElasticSearchExceptions("Updating document was not successful", ex);
            }
        }

        // Search document

        public async Task<IEnumerable<T>> SearchAsync<T>(string indexName, string searchTerm, string fieldName) where T : class
        {
            try
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
                _logger?.LogInformation($"Found document based on searchstring");
                return searchResponse.Documents;
            } catch ( Exception ex )
            {
                _logger?.LogError($"{ex.Message}");
                throw new ElasticSearchExceptions("Search was not successful", ex);
            }
        }


        // Delete document

        public async Task<bool> DeleteDocumentAsync(string indexName, string documentId)
        {
            try
            {
                var response = await _client.DeleteAsync(DocumentPath<object>.Id(documentId), d => d.Index(indexName));

                if (response.IsValid)
                {
                    Console.WriteLine("Document deleted successfully");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to delete document: {response.OriginalException.Message}");
                    return false;
                }
            }catch ( Exception ex )
            {
                _logger?.LogError($"{ex.Message}");
                throw new ElasticSearchExceptions("Deleting document was not successful", ex);
            }
        }


        public async Task<IEnumerable<T>> FuzzySearchAsync<T>(string indexName, string searchTerm, string[] fieldNames, int? limit) where T : class
        {
            try
            {
                var searchResponse = await _client.SearchAsync<T>(s => s
                    .Index(indexName)
                    .Query(q => q
                        .MultiMatch(mm => mm
                            .Fields(fields => fields.Fields(fieldNames))
                                .Query(searchTerm)
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(Fuzziness.Auto)
                             )
                         )
                    .Size(limit ?? 10) // Default limit
                 );
                _logger?.LogInformation($"Found document based on searchstring");
                return searchResponse.Documents;
            } catch(Exception ex)
            {
                _logger?.LogError($"{ex.Message}");
                throw new ElasticSearchExceptions("FuzzySearch was not successful", ex);
            }
        }
    }
}

