using System;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic.Interfaces
{
	public interface IDocumentLogic
	{
		int SaveDocument(Document document);
        int UpdateDocument(Int64 id, Document document);
        int DeleteDocument(Int64 id);
		public string? GetDocumentMetadata(Int64 id);
        void PublishOCRJob(Document document);
        ICollection<Document> GetDocuments();
        Document? GetDocumentById(Int64 id);
	}
}

