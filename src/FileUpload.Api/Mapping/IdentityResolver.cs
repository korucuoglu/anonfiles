using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Data.Entity;
using FileUpload.Shared.Services;

namespace FileUpload.Api.Mapping
{
    public class IdentityResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string> where TSource : class where TDestination : class
    {
        private readonly ISharedIdentityService _sharedIdentityService;

        public IdentityResolver(ISharedIdentityService sharedIdentityService)
        {
            _sharedIdentityService = sharedIdentityService;
        }

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
        {
            return _sharedIdentityService.GetUserId;
        }
    }
}
