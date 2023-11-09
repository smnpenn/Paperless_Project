using System;
namespace Paperless.BusinessLogic.Interfaces
{
	public interface IRabbitMQService
	{
		void SendDocumentToQueue(string documentData);
	}
}

