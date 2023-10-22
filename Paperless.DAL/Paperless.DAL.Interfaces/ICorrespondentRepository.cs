using Paperless.DAL.Entities;

namespace Paperless.DAL.Interfaces
{
    public interface ICorrespondentRepository
    {
        Correspondent GetCorrespondentById(Int64 id);
        ICollection<Correspondent> GetCorrespondents();
        public void Create(Correspondent entity);
        public void Update(Correspondent entity);
        public void Delete(Correspondent entity);
    }
}
