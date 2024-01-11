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

            type.DocumentCount = 0;
            _repo.CreateType(_mapper.Map<DAL.Entities.DocumentType>(type));
            return 0;
        }

        public int DeleteType(Int64 id)
        {
            return _repo.DeleteType(id);
        }

        public ICollection<DocumentType> GetTypes()
        {
            return _mapper.Map<
                    ICollection<DAL.Entities.DocumentType>,
                    ICollection<DocumentType>>(_repo.GetTypes());
        }

        public int UpdateType(Int64 id, DocumentType newType)
        {
            if (!_validator.Validate(newType).IsValid)
                return -1;

            int res = _repo.UpdateType(id, _mapper.Map<DAL.Entities.DocumentType>(newType));
            return res;
        }
    }
}
