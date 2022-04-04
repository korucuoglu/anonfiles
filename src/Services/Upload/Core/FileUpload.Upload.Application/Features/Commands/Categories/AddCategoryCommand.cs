﻿using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Mapping;

namespace FileUpload.Upload.Application.Features.Commands.Categories
{
    public class AddCategoryCommand : IRequest<Response<GetCategoryDto>>
    {
        public string Title { get; set; }
        public int ApplicationUserId { get; set; }
    }
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<GetCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        public async Task<Response<GetCategoryDto>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = ObjectMapper.Mapper.Map<Category>(request);
            var entity = await _unitOfWork.WriteRepository<Category>().AddAsync(category);
            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<GetCategoryDto>.Fail("Kaydetme sırasında hata meydana geldi",  500);
            }

            GetCategoryDto dto = ObjectMapper.Mapper.Map<GetCategoryDto>(entity);

            await _redisService.SetAsync($"categories-{entity.Id}", dto);

            return Response<GetCategoryDto>.Success(dto, 200);
        }
    }
}