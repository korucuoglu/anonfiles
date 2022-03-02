using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        Task<bool> Commit();

    }
}
