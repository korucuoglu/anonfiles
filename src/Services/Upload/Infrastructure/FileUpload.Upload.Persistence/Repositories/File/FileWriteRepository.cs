﻿using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
