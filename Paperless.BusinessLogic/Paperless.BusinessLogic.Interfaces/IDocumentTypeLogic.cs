using Paperless.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.BusinessLogic.Interfaces
{
    public interface IDocumentTypeLogic
    {
        int CreateType(DocumentType type);
        int DeleteType(Int64 id); 
        int UpdateType(Int64 id, DocumentType type);
        ICollection<DocumentType> GetTypes();
    }
}
