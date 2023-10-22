using Paperless.DAL.Entities;

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
