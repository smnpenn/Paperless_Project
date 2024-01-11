﻿using System;
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

        public int SaveDocument(Document document)
        {
            if (!_documentValidator.Validate(document).IsValid)
            {
                return -1;
            }
            if (!File.Exists(document.Path))
            {
                return -1;
            }

            string fileName = Path.GetFileNameWithoutExtension(document.Path);

            _minIOService.UploadDocument(document.Path, fileName);
            Document doc = _mapper.Map<Document>(_repo.Create(_mapper.Map<DAL.Entities.Document>(document)));
            if(doc.DocumentType != null)
                _repo.IncrementDocumentCount(doc.DocumentType);
            _rabbitMQService.SendDocumentToQueue(doc);

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

            if(newDoc.DocumentType != doc.DocumentType)
            {
                if (doc.DocumentType != null)
                    _repo.DecrementDocumentCount(doc.DocumentType);

                if (newDoc.DocumentType != null)
                    _repo.IncrementDocumentCount(newDoc.DocumentType);
            }

            Document updatedDoc = _mapper.Map<Document>(_repo.Update(id, _mapper.Map<DAL.Entities.Document>(newDoc)));
            _minIOService.UploadDocument(updatedDoc.Path, fileName);
            _rabbitMQService.SendDocumentToQueue(updatedDoc);
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

        public int DeleteDocument(long id)
        {
            _elasticSearchServiceAgent.DeleteDocumentAsync("paperless-index", id.ToString());
            return _repo.DeleteDocument(id);
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

