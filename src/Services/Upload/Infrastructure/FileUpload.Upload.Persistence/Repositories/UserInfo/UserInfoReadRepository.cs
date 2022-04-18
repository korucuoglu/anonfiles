using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class UserInfoReadRepository : ReadRepository<Domain.Entities.UserInfo>, IUserInfoReadRepository
    {
        public UserInfoReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
