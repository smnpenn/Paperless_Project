using System;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic.Interfaces
{
	public interface IRabbitMQService
	{
		void SendDocumentToQueue(Document document);

		Document RetrieveOCRJob();

	}
}

