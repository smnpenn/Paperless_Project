using System;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic.Interfaces
{
	public interface IDocumentLogic
	{
		void SaveDocument(Document document);
		int DeleteDocument(Int64 id);
		void PublishOCRJob(Document document);
        ICollection<Document> GetDocuments();
        Document? GetDocumentById(Int64 id);
	}
}

