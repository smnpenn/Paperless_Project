using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using Paperless.DAL.Interfaces;
using AutoMapper;

namespace Paperless.BusinessLogic
{
    public class CorrespondentLogic : ICorrespondentLogic
    {
        private readonly IRabbitMQService _rabbitMQService;
        ICorrespondentRepository _repo;
        IMapper _mapper;

        public CorrespondentLogic(ICorrespondentRepository repository, IMapper mapper, IRabbitMQService rabbitMQService)
        {
            _repo = repository;
            _mapper = mapper;
            _rabbitMQService = rabbitMQService;
        }

        public Correspondent GetCorrespondent(long id)
        {
            return _mapper.Map<Correspondent>(_repo.GetCorrespondentById(id));
        }

        public ICollection<Correspondent> GetCorrespondents()
        {
            return _mapper.Map<
                ICollection<DAL.Entities.Correspondent>,
                ICollection<Correspondent>>( _repo.GetCorrespondents());
        }

        public void SendDocumentToQueueForOCR(string documentData)
        {
            _rabbitMQService.SendDocumentToQueue(documentData);
        }
    }
}
