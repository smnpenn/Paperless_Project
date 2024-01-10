namespace Paperless.BusinessLogic.Interfaces
{
    public interface IDocTagLogic
    {
        public Entities.DocTag GetDocTag(long id);
        public ICollection<Entities.DocTag> GetDocTags();
        public long? CreateDocTag(Entities.DocTag newCorrespondent);
        public void DeleteDocTag(long id);
    }
}
