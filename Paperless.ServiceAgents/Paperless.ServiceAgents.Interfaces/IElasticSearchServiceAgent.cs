using System;
namespace Paperless.ServiceAgents.Interfaces
{
	public interface IElasticSearchServiceAgent
	{
		Task<bool> IndexDocumentAsync<T>(string indexName, T document) where T : class;

        Task<IEnumerable<T>> SearchAsync<T>(string indexName, string searchTerm, string fieldName) where T : class;

        Task<bool> UpdateDocumentAsync<T>(string indexName, string documentId, T document) where T : class;

        Task<bool> DocumentExistsAsync(string indexName, string documentId);

        Task<bool> DeleteDocumentAsync(string indexName, string documentId);

        Task<IEnumerable<T>> FuzzySearchAsync<T>(string indexName, string searchTerm, string fieldName, int? limit) where T : class;

    }
}

