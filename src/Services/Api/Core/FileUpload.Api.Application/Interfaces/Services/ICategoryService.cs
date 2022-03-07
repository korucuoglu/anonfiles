﻿using FileUpload.Application.Features.Commands.Categories.Add;
using FileUpload.Application.Features.Commands.Categories.Update;
using FileUpload.Application.Wrappers;
using FileUpload.Shared.Dtos.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<Response<List<GetCategoryDto>>> GetAllAsync();
        public Task<Response<GetCategoryDto>> GetByIdAsync(Guid id);

        public Task<Response<GetCategoryDto>> AddAsync(AddCategoryCommand dto);
        public Task<Response<bool>> UpdateAsync(UpdateCategoryCommand dto);
        public Task<Response<bool>> DeleteByIdAsync(Guid id);
    }
}
