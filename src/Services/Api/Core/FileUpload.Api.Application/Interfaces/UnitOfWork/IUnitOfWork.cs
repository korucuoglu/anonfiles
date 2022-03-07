using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Domain.Common;
using System;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IReadRepository<T> ReadRepository<T>() where T : BaseEntity;
        IWriteRepository<T> WriteRepository<T>() where T : BaseEntity;

        Task<int> SaveChangesAsync();
    }
}
