using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Domain.Common;
using System;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IReadRepository<T> ReadRepository<T>() where T : BaseEntity;
        IWriteRepository<T> WriteRepository<T>() where T : BaseEntity;

        Task<int> SaveChangesAsync();
    }
}
