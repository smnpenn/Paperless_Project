using System;
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic.Entities;
using Paperless.DAL.Interfaces;
using AutoMapper;

namespace Paperless.BusinessLogic
{
	public class DocumentLogic : IDocumentLogic
	{
		// private readonly IDocumentRepository _documentRepository; for saving documents for later -> See FileStorageService
		private readonly IRabbitMQService _rabbitMQService;
        private readonly DocumentValidator _documentValidator;
        private IDocumentRepository _repo;
        IMapper _mapper;

		public DocumentLogic(IDocumentRepository repository, IMapper mapper, IRabbitMQService rabbitMQService)
		{
            _repo = repository;
            _mapper = mapper;
			_rabbitMQService = rabbitMQService;
            _documentValidator = new DocumentValidator();
		}

        public void SaveDocument(Document document)
        {
            // TO-DO: FileStorageService Save Operation

        }

        public ICollection<Document> GetDocuments()
        {
            return _mapper.Map<
                    ICollection<DAL.Entities.Document>,
                    ICollection<Document>>(_repo.GetDocuments());
        }

        public Document? GetDocumentById(Int64 id) 
        { 
            return _mapper.Map<
                    DAL.Entities.Document?, 
                    Document?>(_repo.GetDocumentById(id));

        }

        public void PublishOCRJob(Document document)
        {
            ValidateDocument(document);
            _rabbitMQService.SendDocumentToQueue(document);
        }

        private void ValidateDocument(Document document)
        {
            var validationResult = _documentValidator.Validate(document);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
                throw new ArgumentException($"Document validation failed: {validationErrors}");
            }
        }

        public int DeleteDocument(long id)
        {
            return _repo.DeleteDocument(id);
        }
    }
}

