using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentTypeRepository
    {
        DocumentType GetDocumentTypeById(Int64 id);
        public int CreateType(DocumentType entity);
        public void UpdateType(DocumentType entity);
        public void DeleteType(DocumentType entity);
    }
}
