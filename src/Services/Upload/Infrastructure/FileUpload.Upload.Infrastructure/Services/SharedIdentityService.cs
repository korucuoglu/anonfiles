﻿using FileUpload.Shared.Const;
using FileUpload.Upload.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FileUpload.Upload.Infrastructure.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("sub"));
        public string GetMail => _httpContextAccessor.HttpContext.User.FindFirstValue(CustomCustomClaimTypes.Mail);
        public string GetUserName => _httpContextAccessor.HttpContext.User.FindFirstValue(CustomCustomClaimTypes.UserName);
    }
}
