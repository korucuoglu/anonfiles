﻿using Microsoft.AspNetCore.Http;
using System;

namespace FileUpload.Shared.Services
{
    public interface ISharedIdentityService
    {
        public Guid GetUserId { get; }
    }
    
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId => new Guid(_httpContextAccessor.HttpContext.User.FindFirst("sub").Value);


    }
}
