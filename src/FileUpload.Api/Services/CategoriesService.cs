using AutoMapper;
using FileUpload.Api.Dtos.Categories;
using FileUpload.Data.Entity;
using FileUpload.Data.Repository;
using FileUpload.Shared.Models;
using FileUpload.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public CategoriesService(ISharedIdentityService sharedIdentityService, IRepository<Data.Entity.File> fileRepository, IRepository<Category> categoryRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _sharedIdentityService = sharedIdentityService;
            _fileRepository = fileRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<GetCategoryDto>>> GetAllAsync()
        {
            var data =  await _categoryRepository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId).Select(x => new GetCategoryDto()
            {   
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();

            return Response<IEnumerable<GetCategoryDto>>.Success(data, 200);
        }

        public async Task<Response<GetCategoryDto>> GetByIdAsync(string id)
        {
            var data = await _categoryRepository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == id).Select(x => new GetCategoryDto()
            {
                Id = x.Id,
                Title = x.Title
            }).FirstOrDefaultAsync();

            return Response<GetCategoryDto>.Success(data, 200);
        }

        public async Task<Response<bool>> AddAsync(AddCategoryDto dto)
        {
            var mapperData = _mapper.Map<Category>(dto);

            await _categoryRepository.AddAsync(mapperData);

            return Response<bool>.Success(true, 200);

        }


        public async Task<Response<bool>> UpdateAsync(UpdateCategory dto)
        {
            if (!_categoryRepository.Any(x=> x.ApplicationUserId ==_sharedIdentityService.GetUserId && x.Id==dto.Id))
            {
                return await Task.FromResult(Response<bool>.Fail(false, 200));
            }

            var mapperData = _mapper.Map<Category>(dto);

            _categoryRepository.Update(mapperData);
            
            return Response<bool>.Success(true, 200);

        }

        public async Task<Response<bool>> DeleteByIdAsync(string id)
        {
            var data = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == id && x.ApplicationUserId == _sharedIdentityService.GetUserId);

            _categoryRepository.Remove(data);

            return Response<bool>.Success(true, 200);

        }

    }
}
