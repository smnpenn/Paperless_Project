using System;
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic.Entities;
using Paperless.DAL.Interfaces;
using AutoMapper;
using Paperless.ServiceAgents.Interfaces;
using Paperless.BusinessLogic.Exceptions;

namespace Paperless.BusinessLogic
{
	public class DocumentLogic : IDocumentLogic
	{
		private readonly IRabbitMQService _rabbitMQService;
        private readonly IMinIOServiceAgent _minIOService;
        private readonly DocumentValidator _documentValidator;
        private readonly IElasticSearchServiceAgent _elasticSearchServiceAgent;
        private IDocumentRepository _repo;
        IMapper _mapper;

		public DocumentLogic(IDocumentRepository repository, IMapper mapper, IRabbitMQService rabbitMQService, IMinIOServiceAgent minIOService, IElasticSearchServiceAgent elasticSearchServiceAgent)
		{
            _repo = repository;
            _mapper = mapper;
			_rabbitMQService = rabbitMQService;
            _minIOService = minIOService;
            _elasticSearchServiceAgent = elasticSearchServiceAgent;
            _documentValidator = new DocumentValidator();
		}

        public void SaveDocument(Document document, Stream fileStream)
        {
            var tempFileName = "temp_recv_file.pdf";

            using (var tempFileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.CopyTo(tempFileStream);
            }

            if (!_documentValidator.Validate(document).IsValid)
            {
                throw new DocumentLogicException("failed to validate document");
            }

            if (!File.Exists(tempFileName))
            {
                throw new DocumentLogicException("failed to create temporary file");
            }

            _minIOService.UploadDocument(tempFileName, document.Title);

            Document doc = _mapper.Map<Document>(_repo.Create(_mapper.Map<DAL.Entities.Document>(document)));
            if(doc.DocumentType != null)
                _repo.IncrementDocumentCount(doc.DocumentType);
            _rabbitMQService.SendDocumentToQueue(doc);
        }

        public void UpdateDocument(Int64 id, Document newDoc)
        {
            Document? doc = _mapper.Map<
                            DAL.Entities.Document?,
                            Document?>(_repo.GetDocumentById(id));

            if (doc == null) throw new DocumentLogicException($"{id} is not a document");

            if (newDoc == null || !_documentValidator.Validate(newDoc).IsValid) throw new DocumentLogicException("invalid document");

            if (newDoc.DocumentType != doc.DocumentType)
            {
                if (doc.DocumentType != null)
                    _repo.DecrementDocumentCount(doc.DocumentType);

                if (newDoc.DocumentType != null)
                    _repo.IncrementDocumentCount(newDoc.DocumentType);
            }

            Document updatedDoc = _mapper.Map<Document>(_repo.Update(id, _mapper.Map<DAL.Entities.Document>(newDoc)));
            _rabbitMQService.SendDocumentToQueue(updatedDoc);
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

        public bool DeleteDocument(long id)
        {
            try
            {
                var deleteTask = _elasticSearchServiceAgent.DeleteDocumentAsync("paperless-index", id.ToString());
                deleteTask.Wait();
                if (deleteTask.Result == false) return false;

                if (_repo.DeleteDocument(id) == false) return false;
            }
            catch (Exception ex)
            {
                // log error
                throw new DocumentLogicException("failed to delete document", ex);
            }
            
            return true;
        }

        public async Task<IEnumerable<Document>> SearchDocument(string searchTerm, int? limit)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term must be provided.");
            }

            var searchResults = await _elasticSearchServiceAgent.FuzzySearchAsync<Document>(
            indexName: "paperless-index",
            searchTerm: searchTerm,
            fieldNames: new[] { "content", "title", "path" }, // specified fields for search
            limit: limit);

            return searchResults;
        }
    }
}

