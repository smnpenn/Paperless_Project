using Paperless.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.DAL.Interfaces
{
    public interface IDocTagRepository
    {
        DocTag GetDocTagById(Int64 id);
        public void Create(DocTag entity);
        public void Update(DocTag entity);
        public void Delete(DocTag entity);
    }
}
