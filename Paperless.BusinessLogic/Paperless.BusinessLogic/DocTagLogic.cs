using AutoMapper;
using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using Paperless.DAL.Interfaces;

namespace Paperless.BusinessLogic
{
    public class DocTagLogic : IDocTagLogic
    {
        IDocTagRepository _repo;
        IMapper _mapper;

        public DocTagLogic(IDocTagRepository repo, IMapper mapper) 
        { 
            _repo = repo;
            _mapper = mapper;
        }

        public long? CreateDocTag(DocTag newCorrespondent)
        {
            return _repo.Create(_mapper.Map<DAL.Entities.DocTag>(newCorrespondent));
        }

        public void DeleteDocTag(long id)
        {
            _repo.Delete(new DAL.Entities.DocTag { Id = id });
        }

        public DocTag GetDocTag(long id)
        {
            return _mapper.Map<DocTag>(_repo.GetDocTagById(id));
        }

        public ICollection<DocTag> GetDocTags()
        {
            return _mapper.Map<
                ICollection<DAL.Entities.DocTag>,
                ICollection<DocTag>>(_repo.GetDocTags());
        }
    }
}
