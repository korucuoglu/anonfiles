using AutoMapper;
using FileUpload.Upload.Application.Interfaces.Services;

namespace FileUpload.Upload.Application.Mapping
{
    public class IdentityResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, int> where TSource : class where TDestination : class
    {
        private readonly ISharedIdentityService _sharedIdentityService;

        public IdentityResolver(ISharedIdentityService sharedIdentityService)
        {
            _sharedIdentityService = sharedIdentityService;
        }

        public int Resolve(TSource source, TDestination destination, int destMember, ResolutionContext context)
        {
            return _sharedIdentityService.GetUserId;
        }
    }
}
