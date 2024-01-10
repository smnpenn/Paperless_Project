using System;
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic.Entities;
using Paperless.DAL.Interfaces;
using AutoMapper;
using Paperless.ServiceAgents.Interfaces;

namespace Paperless.BusinessLogic
{
	public class DocumentLogic : IDocumentLogic
	{
		private readonly IRabbitMQService _rabbitMQService;
        private readonly IMinIOServiceAgent _minIOService;
        private readonly DocumentValidator _documentValidator;
        private IDocumentRepository _repo;
        IMapper _mapper;

		public DocumentLogic(IDocumentRepository repository, IMapper mapper, IRabbitMQService rabbitMQService, IMinIOServiceAgent minIOService)
		{
            _repo = repository;
            _mapper = mapper;
			_rabbitMQService = rabbitMQService;
            _minIOService = minIOService;
            _documentValidator = new DocumentValidator();
		}

        public int SaveDocument(Document document)
        {
            if (!_documentValidator.Validate(document).IsValid)
                return -1;

            if(!File.Exists(document.Path)) 
                return -1;

            string fileName = Path.GetFileNameWithoutExtension(document.Path);

            _minIOService.UploadDocument(document.Path, fileName);
            _repo.Create(_mapper.Map<DAL.Entities.Document>(document));
            _rabbitMQService.SendDocumentToQueue(document);

            return 0;
        }

        public int UpdateDocument(Int64 id, Document newDoc)
        {
            Document? doc = _mapper.Map<
                            DAL.Entities.Document?,
                            Document?>(_repo.GetDocumentById(id));

            if (doc == null)
                return -1;

            if (!_documentValidator.Validate(newDoc).IsValid)
                return -1;

            if (!File.Exists(newDoc.Path))
                return -1;
            string fileName = Path.GetFileNameWithoutExtension(newDoc.Path);

            _repo.Update(id, _mapper.Map<DAL.Entities.Document>(newDoc));
            _minIOService.UploadDocument(newDoc.Path, fileName);
            _rabbitMQService.SendDocumentToQueue(newDoc);
            return 0;
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

        public string? GetDocumentMetadata(Int64 id)
        {
            Document? doc = _mapper.Map<
                            DAL.Entities.Document?,
                            Document?>(_repo.GetDocumentById(id));

            if (doc == null)
                return null;

            //TODO: get actual Tags List
            string metadata = doc.Title + "\n" + "Tags: " + doc.Tags;

            return metadata;
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

