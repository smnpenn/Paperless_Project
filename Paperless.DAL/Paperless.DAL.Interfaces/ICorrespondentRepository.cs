using Paperless.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.DAL.Interfaces
{
    public interface ICorrespondentRepository
    {
        Correspondent GetCorrespondentById(Int64 id);
        public void Create(Correspondent entity);
        public void Update(Correspondent entity);
        public void Delete(Correspondent entity);
    }
}
