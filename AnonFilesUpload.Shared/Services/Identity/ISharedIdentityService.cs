﻿using System;
using Microsoft.AspNetCore.Http;

namespace AnonFilesUpload.Shared.Services
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // public string GetUserId => String.IsNullOrEmpty(_httpContextAccessor.HttpContext.User.FindFirst("sub").Value) ? "" : _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;


    }
}
