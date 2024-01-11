using System;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic.Interfaces
{
	public interface IDocumentLogic
	{
		void SaveDocument(Document document, Stream fileStream);
        void UpdateDocument(Int64 id, Document document);
        bool DeleteDocument(Int64 id);
		public string? GetDocumentMetadata(Int64 id);
        ICollection<Document> GetDocuments();
        Document? GetDocumentById(Int64 id);
        Task<IEnumerable<Document>> SearchDocument(string searchTerm, int? limit);
    }
}

