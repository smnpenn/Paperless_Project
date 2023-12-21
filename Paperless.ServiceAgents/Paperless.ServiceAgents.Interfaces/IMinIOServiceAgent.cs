using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minio.DataModel;

namespace Paperless.ServiceAgents.Interfaces
{
    public interface IMinIOServiceAgent
    {
        Task UploadDocument(string filePath, string fileName);
        Task<Stream> GetDocument(string objectName);
        Task DeleteDocument(string objectName);

    }
}
