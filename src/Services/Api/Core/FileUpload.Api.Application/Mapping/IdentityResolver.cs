using AutoMapper;
using FileUpload.Application.Interfaces.Services;
using System;

namespace FileUpload.Application.Mapping
{
    public class IdentityResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, Guid> where TSource : class where TDestination : class
    {
        private readonly ISharedIdentityService _sharedIdentityService;

        public IdentityResolver(ISharedIdentityService sharedIdentityService)
        {
            _sharedIdentityService = sharedIdentityService;
        }

        public Guid Resolve(TSource source, TDestination destination, Guid destMember, ResolutionContext context)
        {
            return _sharedIdentityService.GetUserId;
        }

       
    }
}
