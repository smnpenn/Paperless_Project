using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        List<Document> GetDocuments();
        Document? GetDocumentById(Int64 id);
        public Document Create(Document entity);
        public Document? Update(Int64 id, Document entity);
        public int DeleteDocument(Int64 id);
        public void IncrementDocumentCount(Int64? count);
        public void DecrementDocumentCount(Int64? count);
    }
}
