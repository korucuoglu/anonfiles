﻿using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Wrappers;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Interfaces.Hub
{
    public interface IFileHub
    {
        Task FilesUploaded(Response<AddFileDto> model);
        Task FilesUploadStarting(string fileName);
    }
}
