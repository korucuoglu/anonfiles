using FileUpload.Api.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;

namespace FileUpload.Api.Infrastructure.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

    }
}
