using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentTypeRepository
    {
        DocumentType? GetDocumentTypeById(Int64? id);
        public ICollection<DocumentType> GetTypes();
        public void CreateType(DocumentType entity);
        public int UpdateType(Int64 id, DocumentType entity);
        public int DeleteType(Int64 id);
        
    }
}
