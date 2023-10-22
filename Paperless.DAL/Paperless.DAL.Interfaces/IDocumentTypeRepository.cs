using Paperless.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentTypeRepository
    {
        DocumentType GetDocumentTypeById(Int64 id);
        public void Create(DocumentType entity);
        public void Update(DocumentType entity);
        public void Delete(DocumentType entity);
    }
}
