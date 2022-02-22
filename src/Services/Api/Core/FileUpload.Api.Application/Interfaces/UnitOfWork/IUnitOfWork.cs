using FileUpload.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
