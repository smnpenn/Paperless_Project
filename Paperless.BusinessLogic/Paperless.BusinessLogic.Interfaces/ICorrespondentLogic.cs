namespace Paperless.BusinessLogic.Interfaces
{
    public interface ICorrespondentLogic
    {
        public Entities.Correspondent GetCorrespondent(long id);
        public ICollection<Entities.Correspondent> GetCorrespondents();
        public long? CreateCorrespondent(Entities.Correspondent newCorrespondent);
        public void DeleteCorrespondent(long id);

    }
}
