using Paperless.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperless.DAL.Interfaces
{
    public interface IUserInfoRepository
    {
        UserInfo GetUserInfoById(Int64 id);
        public void Create(UserInfo entity);
        public void Update(UserInfo entity);
        public void Delete(UserInfo entity);
    }
}
