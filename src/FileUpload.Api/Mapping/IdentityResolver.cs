using AutoMapper;
using FileUpload.Api.Dtos.Categories;
using FileUpload.Data.Entity;
using FileUpload.Shared.Services;

namespace FileUpload.Api.Mapping
{
    public class IdentityResolver : IValueResolver<AddCategoryDto, Category, string>
    {
        private readonly ISharedIdentityService _sharedIdentityService;

        public IdentityResolver(ISharedIdentityService sharedIdentityService)
        {
            _sharedIdentityService = sharedIdentityService;
        }

        public string Resolve(AddCategoryDto source, Category destination, string destMember, ResolutionContext context)
        {
            return _sharedIdentityService.GetUserId;
        }
    }
}
