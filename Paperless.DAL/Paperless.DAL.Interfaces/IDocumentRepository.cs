using Paperless.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.DAL.Interfaces
{
    public interface IDocumentRepository
    {
        Document GetDocumentById(Int64 id);
        public void Create(Document entity);
        public void Update(Document entity);
        public void Delete(Document entity);
    }
}
