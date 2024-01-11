using AutoMapper;
using Paperless.BusinessLogic.Entities;
using Paperless.BusinessLogic.Interfaces;
using Paperless.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.BusinessLogic
{
    public class DocumentTypeLogic : IDocumentTypeLogic
    {
        IDocumentTypeRepository _repo;
        IMapper _mapper;
        DocumentTypeValidator _validator;
        public DocumentTypeLogic(IDocumentTypeRepository repository, IMapper mapper) 
        { 
            _repo = repository;
            _mapper = mapper;
            _validator = new DocumentTypeValidator();

        }

        public int CreateType(DocumentType type)
        {
            if (!_validator.Validate(type).IsValid)
                return -1;

            int res = _repo.CreateType(_mapper.Map<DAL.Entities.DocumentType>(type));
            return res;
        }

        public int DeleteType(Int64 id)
        {
            throw new NotImplementedException();
        }

        public ICollection<DocumentType> GetTypes()
        {
            throw new NotImplementedException();
        }

        public int UpdateType(Int64 id, DocumentType type)
        {
            throw new NotImplementedException();
        }
    }
}
