using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentTypeRepository
    {
        DocumentType GetDocumentTypeById(Int64 id);
        public void Create(DocumentType entity);
        public void Update(DocumentType entity);
        public void Delete(DocumentType entity);
    }
}
