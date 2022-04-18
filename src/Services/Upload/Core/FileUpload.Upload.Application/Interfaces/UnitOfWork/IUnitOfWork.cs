using FileUpload.Upload.Application.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IFileReadRepository FileReadRepository();
        IFileWriteRepository FileWriteRepository();
        IUserInfoReadRepository UserInfoReadRepository();
        IUserInfoWriteRepository UserInfoWriteRepository();

        ICategoryReadRepository CategoryReadRepository();
        ICategoryWriteRepository CategoryWriteRepository();

        Task<int> SaveChangesAsync();
    }
}
