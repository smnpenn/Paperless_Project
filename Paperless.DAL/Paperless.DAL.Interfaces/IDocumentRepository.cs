using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        List<Document> GetDocuments();
        Document? GetDocumentById(Int64 id);
        public void Create(Document entity);
        public int Update(Int64 id, Document entity);
        public int DeleteDocument(Int64 id);
    }
}
