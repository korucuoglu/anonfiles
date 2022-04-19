using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class UserInfoWriteRepository : WriteRepository<Domain.Entities.UserInfo>, IUserInfoWriteRepository
    {
        public UserInfoWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
