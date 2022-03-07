using AutoMapper;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Dtos.Files.Pager;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Queries.Files.GetAll
{
    public class GetAllFilesQueryRequest : IRequest<Response<FilesPagerViewModel>>
    {
        public FileFilterModel FilterModel { get; set; }
        public string UserId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetAllFilesQueryRequest, Response<FilesPagerViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllFilesQueryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<FilesPagerViewModel>> Handle(GetAllFilesQueryRequest request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<File>();
            var FileCategoryRepository = _unitOfWork.GetRepository<FileCategory>();
            var Categoryrepository = _unitOfWork.GetRepository<Category>();

            if (await repository.Any(x => x.UserId == request.UserId))
            {
                var files = repository.Where(x => x.UserId == request.UserId).ToList();

                foreach (var file in files)
                {
                    if (await FileCategoryRepository.Any(x=> x.FileId == file.Id))
                    {
                        var catId =  FileCategoryRepository.Where(x => x.FileId == file.Id).Select(x => x.CategoryId).FirstOrDefault();
                        var category = await Categoryrepository.FirstOrDefaultAsync(x => x.Id == catId);
                        file.Categories.Add(category);
                    }
                }

                return await Helper.Filter.FilterFile(files.AsQueryable(), request.FilterModel, _mapper, _unitOfWork);
            }

            return Response<FilesPagerViewModel>.Success(new FilesPagerViewModel(), 200);
        }
    }
}
