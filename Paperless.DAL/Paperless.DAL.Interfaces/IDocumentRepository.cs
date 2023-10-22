using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        Document GetDocumentById(Int64 id);
        public void Create(Document entity);
        public void Update(Document entity);
        public void Delete(Document entity);
    }
}
