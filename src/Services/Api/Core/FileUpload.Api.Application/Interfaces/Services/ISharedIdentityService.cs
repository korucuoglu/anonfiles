using Microsoft.AspNetCore.Http;
using System;

namespace FileUpload.Application.Interfaces.Services
{
    public interface ISharedIdentityService
    {
        public Guid GetUserId { get; }
    }

   
}
