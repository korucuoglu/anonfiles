using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Common;
using System;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {

        IReadRepository<T> ReadRepository<T>() where T : BaseEntity;
        IFileReadRepository FileReadRepository();
        IFileWriteRepository FileWriteRepository();
        IUserInfoReadRepository UserInfoReadRepository();
        IUserInfoWriteRepository UserInfoWriteRepository();

        ICategoryReadRepository CategoryReadRepository();
        ICategoryWriteRepository CategoryWriteRepository();

        Task<int> SaveChangesAsync();
    }
}
