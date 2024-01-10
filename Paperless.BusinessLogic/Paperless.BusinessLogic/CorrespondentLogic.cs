﻿using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using Paperless.DAL.Interfaces;
using AutoMapper;

namespace Paperless.BusinessLogic
{
    public class CorrespondentLogic : ICorrespondentLogic
    {
        ICorrespondentRepository _repo;
        IMapper _mapper;

        public CorrespondentLogic(ICorrespondentRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        public long? CreateCorrespondent(Correspondent newCorrespondent)
        {
            return _repo.Create(_mapper.Map<DAL.Entities.Correspondent>(newCorrespondent));
        }

        public void DeleteCorrespondent(long id)
        {
            _repo.Delete(id);
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
    }
}
