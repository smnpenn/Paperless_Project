using System;
using Paperless.BusinessLogic.Interfaces;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic
{
	public class DocumentLogic : IDocumentLogic
	{
		// private readonly IDocumentRepository _documentRepository; for saving documents for later -> See FileStorageService
		private readonly IRabbitMQService _rabbitMQService;
        private readonly DocumentValidator _documentValidator;

		public DocumentLogic(IRabbitMQService rabbitMQService, DocumentValidator documentValidator)
		{
			_rabbitMQService = rabbitMQService;
            _documentValidator = documentValidator;
		}

        public void SaveDocument(Document document)
        {
            // TO-DO: FileStorageService Save Operation

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
    }
}

