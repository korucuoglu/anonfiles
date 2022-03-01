using Microsoft.AspNetCore.Http;
using System;

namespace FileUpload.Api.Application.Interfaces.Services
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }

   
}
