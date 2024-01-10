using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface IDocTagRepository
    {
        DocTag GetDocTagById(Int64 id);
        ICollection<DocTag> GetDocTags();
        public Int64 Create(DocTag entity);
        public void Update(DocTag entity);
        public void Delete(DocTag entity);
    }
}
