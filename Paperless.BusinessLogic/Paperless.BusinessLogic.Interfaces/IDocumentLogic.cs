using System;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic.Interfaces
{
	public interface IDocumentLogic
	{
		void SaveDocument(Document document);
		void PublishOCRJob(Document document);
	}
}

