using AutoMapper;
using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Api.Services
{
    public class CategoriesService
    {

        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IRepository<Data.Entity.File> _fileRepository;
        private readonly IRepository<Data.Entity.Category> _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CategoriesService(ISharedIdentityService sharedIdentityService, IRepository<File> fileRepository, IRepository<Category> categoryRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _sharedIdentityService = sharedIdentityService;
            _fileRepository = fileRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCategoriesDto>> GetAllCategories()
        {
            return _mapper.Map<IEnumerable<GetCategoriesDto>>(await _categoryRepository.GetAllAsync(x=> x.ApplicationUserId == _sharedIdentityService.GetUserId));
        }
    }
}
